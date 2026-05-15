namespace Shapes
{
    public abstract class Shape
    {
        public string Name { get; set; }
        public string Color { get; set; }

        protected Shape(string name, string color)
        {
            Name = name;
            Color = color;
        }

        public virtual double CalculateArea() => 0;
        public virtual double CalculatePerimeter() => 0;
        public abstract string GetDescription();
    }
}
