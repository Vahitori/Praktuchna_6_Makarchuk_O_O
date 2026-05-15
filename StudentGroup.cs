using System.Text;
using System.Text.Json;
using Shapes;

namespace StudentManagement;

/// <summary>
/// Оновлений StudentGroup: зберігає List&lt;Person&gt;, підтримує generics (PR5)
/// </summary>
public class StudentGroup
{
    private readonly List<Person> _members = new();
    private readonly Dictionary<string, (int row, int col)> _studentPorts = new();

    public string GroupName { get; set; } = string.Empty;
    public string Specialty  { get; set; } = string.Empty;
    public int    Course     { get; set; } = 1;

    public int GroupSize => _members.Count;

    public double AverageGroupGrade
    {
        get
        {
            var students = _members.OfType<Student>()
                                   .Where(s => s.Status == StudentStatus.Active)
                                   .ToList();
            if (students.Count == 0) return 0;
            return Math.Round(students.Average(s => s.AverageGrade), 2);
        }
    }

    public StudentGroup(string groupName, string specialty, int course)
    {
        GroupName = groupName;
        Specialty = specialty;
        Course    = course;
    }

    public StudentGroup() { }

    // ─── Indexer ──────────────────────────────────────────────────────────────
    public Student? this[string recordBookNumber]
        => _members.OfType<Student>()
                   .FirstOrDefault(s => s.RecordBookNumber == recordBookNumber);

    // ─── Generic method (PR5 requirement) ────────────────────────────────────
    /// <summary>
    /// Повертає всіх членів групи певного типу T.
    /// </summary>
    public List<T> GetMembersByType<T>() where T : Person
        => _members.OfType<T>().ToList();

    // ─── Scholarship (PR5 requirement) ───────────────────────────────────────
    /// <summary>
    /// Загальна сума стипендій усіх членів групи, що реалізують IUniversityMember.
    /// </summary>
    public decimal GetTotalScholarship()
        => _members.OfType<IUniversityMember>()
                   .Sum(m => m.CalculateScholarship());

    // ─── Add / Remove ─────────────────────────────────────────────────────────
    public void AddMember(IUniversityMember member)
    {
        if (member is Person p && !_members.Contains(p))
            _members.Add(p);
    }

    public bool AddStudent(Student s)
    {
        if (_members.OfType<Student>().Any(x => x.RecordBookNumber == s.RecordBookNumber))
            return false;
        _members.Add(s);
        return true;
    }

    public bool RemoveStudent(string recordBookNumber)
    {
        var student = _members.OfType<Student>()
                              .FirstOrDefault(s => s.RecordBookNumber == recordBookNumber);
        if (student == null)
        {
            Console.WriteLine($"  [!] Студента з номером {recordBookNumber} не знайдено.");
            return false;
        }
        _members.Remove(student);
        Console.WriteLine($"  [✓] Студента '{student.FullName}' видалено.");
        return true;
    }

    // ─── Operator Overloading ─────────────────────────────────────────────────
    public static StudentGroup operator +(StudentGroup g1, StudentGroup g2)
    {
        var merged = new StudentGroup($"{g1.GroupName}_{g2.GroupName}", g1.Specialty, g1.Course);
        foreach (var s in g1.GetAllStudents()) merged.AddStudent((Student)s.Clone());
        foreach (var s in g2.GetAllStudents()) merged.AddStudent((Student)s.Clone());
        return merged;
    }

    // ─── Queries ──────────────────────────────────────────────────────────────
    public Student? GetBestStudent()
    {
        var students = _members.OfType<Student>().ToList();
        if (students.Count == 0) return null;
        var best = students[0];
        foreach (var s in students.Skip(1))
            if (s > best) best = s;
        return best;
    }

    public List<Student> GetAllStudents()
        => _members.OfType<Student>().ToList();

    public List<Student> GetExcellentStudents()
        => _members.OfType<Student>()
                   .Where(s => s.IsExcellent() && s.Status == StudentStatus.Active)
                   .ToList();

    public List<Student> GetStudentsByStatus(StudentStatus status)
        => _members.OfType<Student>()
                   .Where(s => s.Status == status)
                   .ToList();

    public List<Student> FindStudent(string fullName)
        => _members.OfType<Student>()
                   .Where(s => s.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase))
                   .ToList();

    public string SearchByNameFragment(string fragment)
    {
        var found = _members.OfType<Student>()
                            .Where(s => s.FullName.Contains(fragment, StringComparison.OrdinalIgnoreCase))
                            .ToList();
        if (found.Count == 0) return "Нічого не знайдено.";
        var sb = new StringBuilder();
        sb.AppendLine($"Результати пошуку для '{fragment}':");
        foreach (var s in found)
            sb.AppendLine($"- {s.FullName} ({s.RecordBookNumber}) [{s.GetType().Name}]");
        return sb.ToString();
    }

    // ─── MergeGroups ──────────────────────────────────────────────────────────
    public void MergeGroups(StudentGroup other)
    {
        foreach (var s in other.GetAllStudents())
            AddStudent((Student)s.Clone());
    }

    // ─── CSV / Text ───────────────────────────────────────────────────────────
    public string ExportToCsv()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Type;FullName;RecordBookNumber;AverageGrade;Status;Notes");
        foreach (var s in GetAllStudents())
            sb.AppendLine($"{s.GetType().Name};{s.FullName};{s.RecordBookNumber};{s.AverageGrade};{s.Status};{s.Notes}");
        return sb.ToString();
    }

    public void ImportStudentsFromText(string rawText)
    {
        var lines = rawText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int imported = 0;
        foreach (var line in lines)
        {
            try
            {
                var parts = line.Split('|').Select(p => p.Trim()).ToArray();
                if (parts.Length >= 4)
                {
                    string name  = parts[0];
                    string rb    = parts[1];
                    string email = parts[2];
                    DateTime dob = DateTime.ParseExact(parts[3], "dd.MM.yyyy", null);
                    if (!_members.OfType<Student>().Any(s => s.RecordBookNumber == rb))
                    {
                        _members.Add(new Student(name, dob, rb, email));
                        imported++;
                    }
                }
            }
            catch { }
        }
        Console.WriteLine($"  [✓] Імпортовано {imported} студентів.");
    }

    // ─── Persistence ──────────────────────────────────────────────────────────
    public void SaveToFile(string filePath = "group_data.json")
    {
        var data = new GroupData
        {
            GroupName = GroupName,
            Specialty = Specialty,
            Course    = Course,
            Students  = GetAllStudents()
        };
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
        Console.WriteLine($"  [✓] Дані збережено у файл '{filePath}'.");
    }

    public void LoadFromFile(string filePath = "group_data.json")
    {
        if (!File.Exists(filePath)) { Console.WriteLine($"  [!] Файл '{filePath}' не знайдено."); return; }
        var data = JsonSerializer.Deserialize<GroupData>(File.ReadAllText(filePath));
        if (data == null) { Console.WriteLine("  [!] Помилка читання файлу."); return; }
        GroupName = data.GroupName;
        Specialty = data.Specialty;
        Course    = data.Course;
        _members.Clear();
        _members.AddRange(data.Students);
        Console.WriteLine($"  [✓] Завантажено {_members.Count} студентів з файлу '{filePath}'.");
    }

    // ─── Statistics ───────────────────────────────────────────────────────────
    public void PrintStatistics()
    {
        var students = GetAllStudents();
        int total     = students.Count;
        int active    = students.Count(s => s.Status == StudentStatus.Active);
        int excellent = GetExcellentStudents().Count;
        int failing   = students.Count(s => s.IsFailing() && s.Status == StudentStatus.Active);
        double avg    = AverageGroupGrade;
        double pct    = active > 0 ? Math.Round((double)excellent / active * 100, 1) : 0;
        decimal totalScholarship = GetTotalScholarship();

        Console.WriteLine($"  ┌─────────────────────────────────────────────┐");
        Console.WriteLine($"  │  Статистика групи: {GroupName,-26}│");
        Console.WriteLine($"  │  Спеціальність:    {Specialty,-26}│");
        Console.WriteLine($"  │  Курс:             {Course,-26}│");
        Console.WriteLine($"  ├─────────────────────────────────────────────┤");
        Console.WriteLine($"  │  Всього студентів: {total,-26}│");
        Console.WriteLine($"  │  Активних:         {active,-26}│");
        Console.WriteLine($"  │  Середній бал:     {avg,-26}│");
        Console.WriteLine($"  │  Відмінників:      {excellent} ({pct}%)                      │");
        Console.WriteLine($"  │  На межі відрах.:  {failing,-26}│");
        Console.WriteLine($"  │  Фонд стипендій:   {totalScholarship:C0,-25}│");
        Console.WriteLine($"  ├─────────────────────────────────────────────┤");

        // Generic breakdown by type
        var types = students.GroupBy(s => s.GetType().Name);
        foreach (var g in types)
            Console.WriteLine($"  │  {g.Key + ":",-20}{g.Count(),-26}│");

        Console.WriteLine($"  └─────────────────────────────────────────────┘");
    }

        _studentPorts[s.RecordBookNumber] = (row, col);
    }

    // ─── PR6: Polymorphic Shape Methods ──────────────────────────────────────

    public List<Shape> GetAllShapes()
        => _members.OfType<Student>().SelectMany(s => s.ScientificShapes).ToList();

    public double GetTotalAreaOfAllShapes()
        => GetAllShapes().Sum(s => s.CalculateArea());

    public void DrawAllShapes()
    {
        Console.WriteLine("\n  ── Малювання всіх наукових проєктів (фігур) ──");
        var shapes = GetAllShapes();
        if (shapes.Count == 0) { Console.WriteLine("  [!] Фігур не знайдено."); return; }
        foreach (var s in shapes) s.Draw();
    }

    public void ResizeAllShapes(double factor)
    {
        Console.WriteLine($"\n  [i] Зміна розміру всіх фігур на коефіцієнт: {factor}");
        foreach (var s in GetAllShapes())
        {
            if (s is IResizable r) r.Resize(factor);
        }
    }

    public void ShowAllShapesInfo()
    {
        Console.WriteLine("\n  ── Інформація про всі фігури (IPrintable) ──");
        foreach (var s in GetAllShapes())
        {
            if (s is IPrintable p) Console.WriteLine($"  → {p.GetPrintInfo()}");
        }
    }
}

public class GroupData
{
    public string GroupName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public int    Course    { get; set; }
    public List<Student> Students { get; set; } = new();
}
