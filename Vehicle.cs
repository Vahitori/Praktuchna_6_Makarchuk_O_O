namespace Variant2;

/// <summary>
/// Варіант 2: Абстрактний клас Vehicle — базовий транспортний засіб.
/// Демонструє: virtual/override, abstract, поліморфізм.
/// </summary>
public abstract class Vehicle
{
    private string _make = string.Empty;
    private int    _year;
    private double _mileage;

    public string Make
    {
        get => _make;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Марка не може бути порожньою.");
            _make = value.Trim();
        }
    }

    public string Model { get; set; } = string.Empty;

    public int Year
    {
        get => _year;
        set
        {
            if (value < 1886 || value > DateTime.Now.Year + 1)
                throw new ArgumentOutOfRangeException(nameof(Year), "Невірний рік виробництва.");
            _year = value;
        }
    }

    public double Mileage
    {
        get => _mileage;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(Mileage), "Пробіг не може бути від'ємним.");
            _mileage = value;
        }
    }

    public string Color { get; set; } = "Білий";

    protected Vehicle(string make, string model, int year, double mileage = 0, string color = "Білий")
    {
        Make    = make;
        Model   = model;
        Year    = year;
        Mileage = mileage;
        Color   = color;
    }

    // ─── Virtual methods ──────────────────────────────────────────────────────

    /// <summary>
    /// Розрахунок вартості обслуговування (virtual — можна перевизначати).
    /// </summary>
    public virtual decimal CalculateMaintenanceCost()
    {
        // Базова формула: вік * 500 + пробіг * 0.1
        int age = DateTime.Now.Year - Year;
        return age * 500m + (decimal)(Mileage * 0.1);
    }

    /// <summary>
    /// Опис транспортного засобу (virtual).
    /// </summary>
    public virtual string GetInfo()
    {
        return $"[Vehicle] {Make} {Model} ({Year}) | Пробіг: {Mileage:N0} км | " +
               $"Колір: {Color} | Обслуговування: {CalculateMaintenanceCost():C0}";
    }

    /// <summary>
    /// Розрахунок витрат пального — абстрактний (обов'язково перевизначити).
    /// </summary>
    public abstract double CalculateFuelCost(double distanceKm);

    // ─── Operators ────────────────────────────────────────────────────────────

    public static bool operator >(Vehicle v1, Vehicle v2)  => v1.Mileage > v2.Mileage;
    public static bool operator <(Vehicle v1, Vehicle v2)  => v1.Mileage < v2.Mileage;
    public static bool operator >=(Vehicle v1, Vehicle v2) => v1.Mileage >= v2.Mileage;
    public static bool operator <=(Vehicle v1, Vehicle v2) => v1.Mileage <= v2.Mileage;

    public static bool operator ==(Vehicle v1, Vehicle v2)
    {
        if (ReferenceEquals(v1, v2)) return true;
        if (v1 is null || v2 is null) return false;
        return v1.Make == v2.Make && v1.Model == v2.Model && v1.Year == v2.Year;
    }
    public static bool operator !=(Vehicle v1, Vehicle v2) => !(v1 == v2);

    // ─── Type conversions ─────────────────────────────────────────────────────

    /// <summary>
    /// Explicit: перетворення на вік транспортного засобу (int).
    /// </summary>
    public static explicit operator int(Vehicle v) => DateTime.Now.Year - v.Year;

    /// <summary>
    /// Implicit: перетворення на рядкове представлення.
    /// </summary>
    public static implicit operator string(Vehicle v) => v.GetInfo();

    public override bool Equals(object? obj) => obj is Vehicle v && this == v;
    public override int GetHashCode() => HashCode.Combine(Make, Model, Year);
    public override string ToString() => GetInfo();
}
