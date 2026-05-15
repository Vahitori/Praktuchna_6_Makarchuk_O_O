using System;

namespace Praktuchna_Makarchuk_O_O
{
    public class Fraction
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
                throw new ArgumentException("Denominator cannot be zero.");

            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            int common = Gcd(Math.Abs(numerator), denominator);
            Numerator = numerator / common;
            Denominator = denominator / common;
        }

        private static int Gcd(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static Fraction operator +(Fraction f1, Fraction f2)
            => new Fraction(f1.Numerator * f2.Denominator + f2.Numerator * f1.Denominator, f1.Denominator * f2.Denominator);

        public static Fraction operator -(Fraction f1, Fraction f2)
            => new Fraction(f1.Numerator * f2.Denominator - f2.Numerator * f1.Denominator, f1.Denominator * f2.Denominator);

        public static Fraction operator *(Fraction f1, Fraction f2)
            => new Fraction(f1.Numerator * f2.Numerator, f1.Denominator * f2.Denominator);

        public static Fraction operator /(Fraction f1, Fraction f2)
        {
            if (f2.Numerator == 0) throw new DivideByZeroException();
            return new Fraction(f1.Numerator * f2.Denominator, f1.Denominator * f2.Numerator);
        }

        public static bool operator >(Fraction f1, Fraction f2) => (double)f1 > (double)f2;
        public static bool operator <(Fraction f1, Fraction f2) => (double)f1 < (double)f2;
        public static bool operator >=(Fraction f1, Fraction f2) => (double)f1 >= (double)f2;
        public static bool operator <=(Fraction f1, Fraction f2) => (double)f1 <= (double)f2;

        public static bool operator ==(Fraction f1, Fraction f2)
        {
            if (ReferenceEquals(f1, f2)) return true;
            if (f1 is null || f2 is null) return false;
            return f1.Numerator == f2.Numerator && f1.Denominator == f2.Denominator;
        }

        public static bool operator !=(Fraction f1, Fraction f2) => !(f1 == f2);

        public static explicit operator double(Fraction f) => (double)f.Numerator / f.Denominator;

        public override bool Equals(object obj) => obj is Fraction f && this == f;
        public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);

        public override string ToString() => Denominator == 1 ? $"{Numerator}" : $"{Numerator}/{Denominator}";
    }
}
