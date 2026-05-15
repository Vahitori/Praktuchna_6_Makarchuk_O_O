namespace StudentManagement;

/// <summary>
/// Аспірант — sealed клас (не може бути успадкований далі)
/// </summary>
public sealed class GraduateStudent : Student
{
    public string ResearchTopic { get; set; } = string.Empty;
    public string Supervisor { get; set; } = string.Empty;
    public int YearOfStudy { get; set; } = 1;

    public GraduateStudent(string fullName, DateTime dateOfBirth,
        string recordBookNumber, string personalEmail,
        string researchTopic, string supervisor)
        : base(fullName, dateOfBirth, recordBookNumber, personalEmail)
    {
        ResearchTopic = researchTopic;
        Supervisor = supervisor;
        CourseProgress = 0;
    }

    public override decimal CalculateScholarship()
    {
        // Аспіранти отримують академічну стипендію незалежно від балів
        return 5000m + (YearOfStudy > 2 ? 1000m : 0m);
    }

    public override void Enroll()
    {
        base.Enroll();
        Console.WriteLine($"  [🎓] {FullName} зарахований до аспірантури.");
        Console.WriteLine($"       Тема: «{ResearchTopic}» | Науковий керівник: {Supervisor}");
    }

    public override string GetInfo()
    {
        return $"[🎓 GraduateStudent] {FullName} | №{RecordBookNumber} | " +
               $"Рік навчання: {YearOfStudy} | Тема: «{ResearchTopic}» | " +
               $"Керівник: {Supervisor} | Стипендія: {CalculateScholarship():C0}";
    }
}
