using System;

namespace Shapes
{
    public class Triangle : Shape
    {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }

        public Triangle(string name, string color, double a, double b, double c) : base(name, color)
        {
            SideA = a;
            SideB = b;
            SideC = c;
        }

        public override double CalculateArea()
        {
            double s = (SideA + SideB + SideC) / 2;
            return Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
        }

        public override double CalculatePerimeter() => SideA + SideB + SideC;
        public override string GetDescription() => $"{Name} (Колір: {Color}, Сторони: {SideA:F2}, {SideB:F2}, {SideC:F2})";
    }
}
