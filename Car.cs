namespace Variant2;

/// <summary>
/// Варіант 2: Легковий автомобіль — успадковує Vehicle.
/// </summary>
public class Car : Vehicle
{
    public int   PassengerCount { get; set; } = 5;
    public bool  HasAirCon      { get; set; }
    public double FuelConsumption { get; set; } // л/100км

    public Car(string make, string model, int year,
               double mileage = 0, double fuelConsumption = 8.0,
               bool hasAirCon = false, string color = "Білий")
        : base(make, model, year, mileage, color)
    {
        FuelConsumption = fuelConsumption;
        HasAirCon       = hasAirCon;
    }

    public override double CalculateFuelCost(double distanceKm)
    {
        // Кондиціонер збільшує витрату на 10%
        double consumption = HasAirCon ? FuelConsumption * 1.1 : FuelConsumption;
        double liters = distanceKm / 100.0 * consumption;
        return Math.Round(liters * 54.0, 2); // 54 грн/л
    }

    public override decimal CalculateMaintenanceCost()
    {
        // Легковик: базова + надбавка за пробіг понад 100к км
        decimal baseCost = base.CalculateMaintenanceCost();
        return Mileage > 100000 ? baseCost * 1.2m : baseCost;
    }

    public override string GetInfo()
    {
        return $"[🚗 Car] {Make} {Model} ({Year}) | Пробіг: {Mileage:N0} км | " +
               $"Колір: {Color} | Пасажирів: {PassengerCount} | " +
               $"Витрата: {FuelConsumption} л/100км | AC: {(HasAirCon ? "✓" : "✗")} | " +
               $"Обслуговування: {CalculateMaintenanceCost():C0}";
    }
}
