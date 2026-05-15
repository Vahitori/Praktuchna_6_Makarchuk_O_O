namespace StudentManagement;

/// <summary>
/// Студент, що працює — поєднує навчання з роботою
/// </summary>
public class WorkingStudent : Student
{
    public string Employer { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public int WorkHoursPerWeek { get; set; }

    public WorkingStudent(string fullName, DateTime dateOfBirth,
        string recordBookNumber, string personalEmail,
        string employer, string jobTitle, int workHoursPerWeek = 20)
        : base(fullName, dateOfBirth, recordBookNumber, personalEmail)
    {
        Employer = employer;
        JobTitle = jobTitle;
        WorkHoursPerWeek = Math.Clamp(workHoursPerWeek, 0, 60);
    }

    public override decimal CalculateScholarship()
    {
        // Студенти, що працюють >= 40 год/тиж, втрачають право на стипендію
        if (WorkHoursPerWeek >= 40) return 0m;
        // Часткова зайнятість — 50% від звичайної
        return base.CalculateScholarship() * 0.5m;
    }

    public override void Enroll()
    {
        base.Enroll();
        Console.WriteLine($"  [💼] {FullName} навчається та працює ({JobTitle} в {Employer}, {WorkHoursPerWeek} год/тиж).");
    }

    public override string GetInfo()
    {
        return $"[💼 WorkingStudent] {FullName} | №{RecordBookNumber} | " +
               $"Бал: {AverageGrade} | Роботодавець: {Employer} | {WorkHoursPerWeek} год/тиж";
    }
}
