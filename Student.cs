using System.Text;
using Praktuchna_Makarchuk_O_O;
using Shapes;

namespace StudentManagement;

/// <summary>
/// Клас Student успадковує Person та реалізує IUniversityMember (PR5)
/// </summary>
public class Student : Person, IUniversityMember, ICloneable
{
    private string _recordBookNumber = string.Empty;
    private double _averageGrade;

    public byte[] LabGrades { get; private set; } = new byte[10];

    // Fields from PR4
    private int _courseProgress;
    public int CourseProgress
    {
        get => _courseProgress;
        set => _courseProgress = Math.Clamp(value, 0, 100);
    }
    public List<GradePoint> Grades { get; private set; } = new List<GradePoint>();

    // PR6: Polymorphic collection of shapes (Scientific projects)
    public List<Shape> ScientificShapes { get; set; } = new List<Shape>();

    public string RecordBookNumber
    {
        get => _recordBookNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Номер залікової не може бути порожнім.");
            if (value.Trim().Length != 8 || !value.Trim().All(char.IsDigit))
                throw new ArgumentException("Номер залікової книжки повинен складатися рівно з 8 цифр.");
            _recordBookNumber = value.Trim();
        }
    }

    public double AverageGrade
    {
        get
        {
            if (Grades.Count > 0)
                return Math.Round(Grades.Average(g => g.Value) * 10, 2);
            return _averageGrade;
        }
        private set
        {
            if (value < 0 || value > 100)
                throw new ArgumentOutOfRangeException(nameof(value), "Середній бал повинен бути від 0 до 100.");
            _averageGrade = Math.Round(value, 2);
        }
    }

    public StudentStatus Status { get; set; } = StudentStatus.Active;

    public DateTime EnrollmentDate { get; init; } = DateTime.Today;

    // ----- Constructors -----

    public Student(string fullName, DateTime dateOfBirth, string recordBookNumber,
                   string personalEmail, DateTime? enrollmentDate = null)
        : base(fullName, dateOfBirth, personalEmail)
    {
        RecordBookNumber = recordBookNumber;
        EnrollmentDate = enrollmentDate ?? DateTime.Today;
        _averageGrade = 0;
        LabGrades = new byte[10];
        CourseProgress = 0;
    }

    public Student() { }

    // ----- IUniversityMember -----

    public virtual decimal CalculateScholarship()
    {
        if (AverageGrade >= 90) return 3500m;
        if (AverageGrade >= 75) return 2500m;
        if (AverageGrade >= 60) return 1800m;
        return 0m;
    }

    public virtual void Enroll()
    {
        Status = StudentStatus.Active;
        Console.WriteLine($"  [✓] Студент {FullName} (№{RecordBookNumber}) зарахований до університету.");
    }

    // ----- Override GetInfo (from Person) -----

    public override string GetInfo()
    {
        return $"[Student] {FullName} | №{RecordBookNumber} | Бал: {AverageGrade} | " +
               $"Прогрес: {CourseProgress}% | {StatusToUkrainian()}";
    }

    // ----- Business Methods -----

    public void AddGrade(GradePoint grade) => Grades.Add(grade);

    public void UpdateAverageGrade(double newGrade)
    {
        if (newGrade < 0 || newGrade > 100)
        {
            Console.WriteLine($"  [!] Помилка: Бал {newGrade} виходить за межі 0-100.");
            return;
        }
        AverageGrade = newGrade;
    }

    public bool IsExcellent() => AverageGrade >= 90;
    public bool IsFailing()   => AverageGrade < 60;

    public int GetYearsToGraduation()
    {
        int yearsStudied = DateTime.Today.Year - EnrollmentDate.Year;
        int remaining = 4 - yearsStudied;
        return remaining < 0 ? 0 : remaining;
    }

    public string GetFormattedInfo(bool detailed = false)
    {
        var sb = new StringBuilder();
        if (detailed)
        {
            sb.AppendLine($"  ╔══════════════════════════════════════════════╗");
            sb.AppendLine($"  ║  ПІБ:            {FullName,-30}║");
            sb.AppendLine($"  ║  Тип:            {GetType().Name,-30}║");
            sb.AppendLine($"  ║  Залікова №:     {RecordBookNumber,-30}║");
            sb.AppendLine($"  ║  Дата нар.:      {DateOfBirth:dd.MM.yyyy}  (Вік: {Age} р.)         ║");
            sb.AppendLine($"  ║  Email:          {PersonalEmail,-30}║");
            sb.AppendLine($"  ║  Сер. бал:       {AverageGrade,-30}║");
            sb.AppendLine($"  ║  Прогрес:        {CourseProgress}%                            ║");
            sb.AppendLine($"  ║  Статус:         {StatusToUkrainian(),-30}║");
            sb.AppendLine($"  ║  Стипендія:      {CalculateScholarship(),-30:C0}║");
            sb.AppendLine($"  ╚══════════════════════════════════════════════╝");
        }
        else
        {
            sb.Append($"{FullName} | №{RecordBookNumber} | Бал: {AverageGrade} | Прогрес: {CourseProgress}% | Стипендія: {CalculateScholarship():C0}");
        }
        return sb.ToString();
    }

    protected string StatusToUkrainian() => Status switch
    {
        StudentStatus.Active        => "Активний",
        StudentStatus.AcademicLeave => "Академвідпустка",
        StudentStatus.Expelled      => "Відрахований",
        StudentStatus.Graduated     => "Закінчив",
        _                           => Status.ToString()
    };

    // ----- Operator Overloading (from PR4) -----

    public static bool operator >(Student s1, Student s2) =>
        (s1.AverageGrade + s1.CourseProgress) > (s2.AverageGrade + s2.CourseProgress);

    public static bool operator <(Student s1, Student s2) =>
        (s1.AverageGrade + s1.CourseProgress) < (s2.AverageGrade + s2.CourseProgress);

    public static bool operator >=(Student s1, Student s2) =>
        (s1.AverageGrade + s1.CourseProgress) >= (s2.AverageGrade + s2.CourseProgress);

    public static bool operator <=(Student s1, Student s2) =>
        (s1.AverageGrade + s1.CourseProgress) <= (s2.AverageGrade + s2.CourseProgress);

    public static bool operator ==(Student s1, Student s2)
    {
        if (ReferenceEquals(s1, s2)) return true;
        if (s1 is null || s2 is null) return false;
        return s1.RecordBookNumber == s2.RecordBookNumber;
    }

    public static bool operator !=(Student s1, Student s2) => !(s1 == s2);

    public static Student operator +(Student s1, Student s2)
    {
        var teamName = $"Team: {s1.FullName.Split(' ')[0]} & {s2.FullName.Split(' ')[0]}";
        var team = new Student(teamName, DateTime.Today, "00000000", "team@edu.ua")
        {
            CourseProgress = (s1.CourseProgress + s2.CourseProgress) / 2
        };
        team.UpdateAverageGrade((s1.AverageGrade + s2.AverageGrade) / 2);
        return team;
    }

    public override bool Equals(object? obj) => obj is Student s && this == s;
    public override int GetHashCode() => RecordBookNumber.GetHashCode();

    public object Clone()
    {
        var clone = (Student)this.MemberwiseClone();
        clone.LabGrades = (byte[])this.LabGrades.Clone();
        clone.Grades = new List<GradePoint>(this.Grades);
        return clone;
    }

    public override string ToString() => GetInfo();
}
