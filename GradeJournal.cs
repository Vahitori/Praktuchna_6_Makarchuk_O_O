namespace StudentManagement;

public class GradeJournal
{
    private readonly Dictionary<string, double> _grades = new();

    public string StudentRecordBook { get; init; } = string.Empty;

    public int SubjectCount => _grades.Count;

    public double ComputedAverageGrade
        => _grades.Count > 0 ? Math.Round(_grades.Values.Average(), 2) : 0;

    public void AddOrUpdateGrade(string subject, double grade)
    {
        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Назва предмету не може бути порожньою.");
        if (grade < 0 || grade > 100)
            throw new ArgumentOutOfRangeException(nameof(grade), "Оцінка повинна бути від 0 до 100.");

        _grades[subject] = Math.Round(grade, 2);
        Console.WriteLine($"  [✓] Предмет '{subject}': оцінка {grade} збережена.");
    }

    public void RecalculateAndApply(Student student)
    {
        if (_grades.Count == 0) return;
        double avg = ComputedAverageGrade;
        student.UpdateAverageGrade(avg);
        Console.WriteLine($"  [✓] Середній бал студента '{student.FullName}' перераховано: {avg}");
    }

    public bool RemoveSubject(string subject)
    {
        bool removed = _grades.Remove(subject);
        if (removed) Console.WriteLine($"  [✓] Предмет '{subject}' видалено.");
        else Console.WriteLine($"  [!] Предмет '{subject}' не знайдено.");
        return removed;
    }

    public void PrintGrades()
    {
        if (_grades.Count == 0)
        {
            Console.WriteLine("  Журнал оцінок порожній.");
            return;
        }
        Console.WriteLine($"  ┌─────────────────────────────────────────────┐");
        Console.WriteLine($"  │  Журнал оцінок  (залікова: {StudentRecordBook})        │");
        Console.WriteLine($"  ├──────────────────────────────┬──────────────┤");
        foreach (var kv in _grades.OrderBy(x => x.Key))
            Console.WriteLine($"  │  {kv.Key,-28}│  {kv.Value,10:F2}  │");
        Console.WriteLine($"  ├──────────────────────────────┼──────────────┤");
        string avgLabel = "Середній бал";
        Console.WriteLine($"  │  {avgLabel,-28}│  {ComputedAverageGrade,10:F2}  │");
        Console.WriteLine($"  └──────────────────────────────┴──────────────┘");
    }
}
