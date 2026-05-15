namespace Variant2;

/// <summary>
/// Варіант 2: Вантажівка — успадковує Vehicle.
/// </summary>
public class Truck : Vehicle
{
    public double MaxPayloadTons  { get; set; }   // макс. вантаж у тоннах
    public double CurrentLoadTons { get; set; }   // поточний вантаж
    public bool   HasRefrigerator { get; set; }   // наявність рефрижератора
    public double FuelConsumption { get; set; } = 30.0; // л/100км

    public Truck(string make, string model, int year, double maxPayloadTons,
                 bool hasRefrigerator = false,
                 double mileage = 0, string color = "Сірий")
        : base(make, model, year, mileage, color)
    {
        MaxPayloadTons  = maxPayloadTons;
        HasRefrigerator = hasRefrigerator;
    }

    public double LoadPercent =>
        MaxPayloadTons > 0 ? Math.Round(CurrentLoadTons / MaxPayloadTons * 100, 1) : 0;

    public override double CalculateFuelCost(double distanceKm)
    {
        // Витрата зростає пропорційно завантаженості + рефрижератор +15%
        double loadFactor = 1.0 + (LoadPercent / 100.0) * 0.5;
        double fridgeFactor = HasRefrigerator ? 1.15 : 1.0;
        double liters = distanceKm / 100.0 * FuelConsumption * loadFactor * fridgeFactor;
        return Math.Round(liters * 54.0, 2);
    }

    public override decimal CalculateMaintenanceCost()
    {
        // Вантажівка: базова * 3 + рефрижератор 5000 грн/рік
        decimal baseCost = base.CalculateMaintenanceCost() * 3m;
        return HasRefrigerator ? baseCost + 5000m : baseCost;
    }

    public bool CanLoad(double extraTons) =>
        CurrentLoadTons + extraTons <= MaxPayloadTons;

    public override string GetInfo()
    {
        return $"[🚚 Truck] {Make} {Model} ({Year}) | Вантажопідйомність: {MaxPayloadTons} т | " +
               $"Вантаж: {CurrentLoadTons} т ({LoadPercent}%) | " +
               $"Рефрижератор: {(HasRefrigerator ? "✓" : "✗")} | " +
               $"Пробіг: {Mileage:N0} км | Обслуговування: {CalculateMaintenanceCost():C0}";
    }
}
