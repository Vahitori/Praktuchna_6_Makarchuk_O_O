namespace StudentManagement;

/// <summary>
/// Абстрактний базовий клас Person (PR5 - Успадкування)
/// </summary>
public abstract class Person
{
    private string _fullName = string.Empty;
    private string _personalEmail = string.Empty;

    public string FullName
    {
        get => _fullName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("ПІБ не може бути порожнім.");
            _fullName = value.Trim();
        }
    }

    public DateTime DateOfBirth { get; init; }

    public string PersonalEmail
    {
        get => _personalEmail;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email не може бути порожнім.");
            if (!value.Contains('@') || !value.Contains('.'))
                throw new ArgumentException("Невірний формат email.");
            _personalEmail = value.Trim().ToLower();
        }
    }

    public string Notes { get; set; } = string.Empty;

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

    protected Person() { }

    protected Person(string fullName, DateTime dateOfBirth, string personalEmail)
    {
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        PersonalEmail = personalEmail;
    }

    /// <summary>
    /// Віртуальний метод отримання інформації (можна перевизначати)
    /// </summary>
    public virtual string GetInfo()
    {
        return $"[Person] {FullName} | DOB: {DateOfBirth:dd.MM.yyyy} | Email: {PersonalEmail}";
    }

    public override string ToString() => GetInfo();
}
