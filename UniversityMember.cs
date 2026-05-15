namespace StudentManagement;

/// <summary>
/// Інтерфейс для всіх членів університету (PR5)
/// </summary>
public interface IUniversityMember
{
    /// <summary>
    /// Розрахунок стипендії
    /// </summary>
    decimal CalculateScholarship();

    /// <summary>
    /// Зарахування до університету
    /// </summary>
    void Enroll();
}
