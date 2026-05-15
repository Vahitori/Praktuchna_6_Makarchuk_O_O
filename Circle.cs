using System;

namespace Shapes
{
    public class Circle : Shape, IResizable
    {
        public double Radius { get; set; }

        public Circle(string name, string color, double radius) : base(name, color)
        {
            Radius = radius;
        }

        public override double CalculateArea() => Math.PI * Radius * Radius;
        public override double CalculatePerimeter() => 2 * Math.PI * Radius;
        public override string GetDescription() => $"{Name} (Колір: {Color}, Радіус: {Radius:F2})";
        public override void Draw() => Console.WriteLine($"[O] Малювання кола: {GetDescription()}");
        public void Resize(double factor) => Radius *= factor;
    }
}
