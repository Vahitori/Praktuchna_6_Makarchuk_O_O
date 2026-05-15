namespace Variant2;

/// <summary>
/// Варіант 2: Автобус — успадковує Vehicle.
/// </summary>
public class Bus : Vehicle
{
    public int    SeatCount     { get; set; } = 40;
    public string RouteNumber   { get; set; } = string.Empty;
    public bool   IsIntercity   { get; set; }
    public double FuelConsumption { get; set; } = 25.0; // л/100км

    public Bus(string make, string model, int year, int seatCount,
               string routeNumber, bool isIntercity = false,
               double mileage = 0, string color = "Жовтий")
        : base(make, model, year, mileage, color)
    {
        SeatCount    = seatCount;
        RouteNumber  = routeNumber;
        IsIntercity  = isIntercity;
    }

    public override double CalculateFuelCost(double distanceKm)
    {
        // Міжміський автобус споживає на 20% більше
        double consumption = IsIntercity ? FuelConsumption * 1.2 : FuelConsumption;
        double liters = distanceKm / 100.0 * consumption;
        return Math.Round(liters * 54.0, 2);
    }

    public override decimal CalculateMaintenanceCost()
    {
        // Автобус: базова * 2 (важче обслуговування)
        return base.CalculateMaintenanceCost() * 2m;
    }

    /// <summary>
    /// Розрахунок прибутку за рейс.
    /// </summary>
    public decimal CalculateRouteRevenue(double distanceKm, decimal ticketPrice)
    {
        double fuelCost = CalculateFuelCost(distanceKm);
        decimal revenue = SeatCount * ticketPrice;
        return revenue - (decimal)fuelCost;
    }

    public override string GetInfo()
    {
        return $"[🚌 Bus] {Make} {Model} ({Year}) | Маршрут: №{RouteNumber} | " +
               $"Місць: {SeatCount} | {(IsIntercity ? "Міжміський" : "Міський")} | " +
               $"Пробіг: {Mileage:N0} км | Обслуговування: {CalculateMaintenanceCost():C0}";
    }
}
