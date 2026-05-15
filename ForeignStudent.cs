namespace StudentManagement;

/// <summary>
/// Іноземний студент — студент з іноземного університету
/// </summary>
public class ForeignStudent : Student
{
    public string Country { get; set; } = string.Empty;
    public string HomeUniversity { get; set; } = string.Empty;
    public bool HasLanguageCertificate { get; set; }

    public ForeignStudent(string fullName, DateTime dateOfBirth,
        string recordBookNumber, string personalEmail,
        string country, string homeUniversity)
        : base(fullName, dateOfBirth, recordBookNumber, personalEmail)
    {
        Country = country;
        HomeUniversity = homeUniversity;
    }

    public override decimal CalculateScholarship()
    {
        // Іноземні студенти не отримують стипендію від університету
        return 0m;
    }

    public override void Enroll()
    {
        base.Enroll();
        Console.WriteLine($"  [🌍] {FullName} зарахований як іноземний студент з {Country} ({HomeUniversity}).");
        if (!HasLanguageCertificate)
            Console.WriteLine($"  [!] Увага: відсутній мовний сертифікат для {FullName}.");
    }

    public override string GetInfo()
    {
        return $"[🌍 ForeignStudent] {FullName} | №{RecordBookNumber} | " +
               $"Бал: {AverageGrade} | Країна: {Country} | Вуз: {HomeUniversity}";
    }
}
