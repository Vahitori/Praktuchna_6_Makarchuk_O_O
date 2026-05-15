namespace StudentManagement;

/// <summary>
/// Відмінник — студент з середнім балом >= 90
/// </summary>
public class ExcellentStudent : Student
{
    public bool HasScholarshipBonus { get; set; } = true;
    public string AchievementTitle { get; set; } = "Відмінник навчання";

    public ExcellentStudent(string fullName, DateTime dateOfBirth,
        string recordBookNumber, string personalEmail)
        : base(fullName, dateOfBirth, recordBookNumber, personalEmail)
    {
        // Відмінники починають з прогресом 100%
        CourseProgress = 100;
    }

    public override decimal CalculateScholarship()
    {
        // Відмінники отримують підвищену стипендію
        decimal base_ = base.CalculateScholarship();
        return HasScholarshipBonus ? base_ * 1.5m : base_;
    }

    public override void Enroll()
    {
        base.Enroll();
        Console.WriteLine($"  [★] {FullName} зарахований як відмінник. Нагорода: «{AchievementTitle}».");
    }

    public override string GetInfo()
    {
        return $"[★ ExcellentStudent] {FullName} | №{RecordBookNumber} | " +
               $"Бал: {AverageGrade} | Нагорода: «{AchievementTitle}» | " +
               $"Стипендія: {CalculateScholarship():C0}";
    }
}
