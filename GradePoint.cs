using System;

namespace Praktuchna_Makarchuk_O_O
{
    public class GradePoint
    {
        private double _value;
        public double Value
        {
            get => _value;
            set => _value = Math.Clamp(value, 0, 10);
        }

        public GradePoint(double value)
        {
            Value = value;
        }

        // Arithmetic operators
        public static GradePoint operator +(GradePoint g1, GradePoint g2) => new GradePoint(g1.Value + g2.Value);
        public static GradePoint operator +(GradePoint g, double val) => new GradePoint(g.Value + val);

        // Unary operators
        public static GradePoint operator ++(GradePoint g)
        {
            g.Value++;
            return g;
        }

        public static GradePoint operator --(GradePoint g)
        {
            g.Value--;
            return g;
        }

        // Boolean operators
        public static bool operator true(GradePoint g) => g.Value >= 8;
        public static bool operator false(GradePoint g) => g.Value < 8;

        // Comparison operators
        public static bool operator >(GradePoint g1, GradePoint g2) => g1.Value > g2.Value;
        public static bool operator <(GradePoint g1, GradePoint g2) => g1.Value < g2.Value;
        public static bool operator >=(GradePoint g1, GradePoint g2) => g1.Value >= g2.Value;
        public static bool operator <=(GradePoint g1, GradePoint g2) => g1.Value <= g2.Value;

        // Conversion operators
        public static implicit operator double(GradePoint g) => g.Value;
        public static implicit operator GradePoint(double d) => new GradePoint(d);

        public override string ToString() => $"{Value:F1}";
    }
}
