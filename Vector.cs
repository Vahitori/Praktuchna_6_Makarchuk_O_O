using System;

namespace Praktuchna_Makarchuk_O_O
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

        // Arithmetic operators
        public static Vector operator +(Vector v1, Vector v2) => new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vector operator -(Vector v1, Vector v2) => new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static Vector operator *(Vector v, double scalar) => new Vector(v.X * scalar, v.Y * scalar, v.Z * scalar);
        public static Vector operator *(double scalar, Vector v) => v * scalar;

        // Unary operators
        public static Vector operator ++(Vector v) => new Vector(v.X + 1, v.Y + 1, v.Z + 1);
        public static Vector operator --(Vector v) => new Vector(v.X - 1, v.Y - 1, v.Z - 1);

        // Comparison operators (by length)
        public static bool operator >(Vector v1, Vector v2) => v1.Length > v2.Length;
        public static bool operator <(Vector v1, Vector v2) => v1.Length < v2.Length;
        public static bool operator >=(Vector v1, Vector v2) => v1.Length >= v2.Length;
        public static bool operator <=(Vector v1, Vector v2) => v1.Length <= v2.Length;

        // Equality operators
        public static bool operator ==(Vector v1, Vector v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if (v1 is null || v2 is null) return false;
            return Math.Abs(v1.X - v2.X) < 1e-9 && Math.Abs(v1.Y - v2.Y) < 1e-9 && Math.Abs(v1.Z - v2.Z) < 1e-9;
        }

        public static bool operator !=(Vector v1, Vector v2) => !(v1 == v2);

        // Type conversion
        public static explicit operator double(Vector v) => v.Length;

        public override bool Equals(object obj) => obj is Vector v && this == v;
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public override string ToString() => $"Vector({X}, {Y}, {Z}), Length: {Length:F2}";
    }
}
