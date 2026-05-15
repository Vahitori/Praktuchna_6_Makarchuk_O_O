namespace Shapes
{
    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(string name, string color, double width, double height) : base(name, color)
        {
            Width = width;
            Height = height;
        }

        public override double CalculateArea() => Width * Height;
        public override double CalculatePerimeter() => 2 * (Width + Height);
        public override string GetDescription() => $"{Name} (Колір: {Color}, Ширина: {Width:F2}, Висота: {Height:F2})";
    }
}
