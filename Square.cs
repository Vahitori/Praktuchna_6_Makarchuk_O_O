namespace Shapes
{
    public class Square : Rectangle
    {
        public double Side => Width;

        public Square(string name, string color, double side) : base(name, color, side, side)
        {
        }

        public override string GetDescription() => $"{Name} (Колір: {Color}, Сторона: {Side:F2})";
    }
}
