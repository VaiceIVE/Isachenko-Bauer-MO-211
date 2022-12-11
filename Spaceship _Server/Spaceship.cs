using System;

namespace Spaceship__Server
{
    public interface ICommand
    {
        public void Execute();
    }
    public class Fraction
    {
        int numerator { get; set; }
        int denominator { get; set; }
        public Fraction(int num, int den)
        {
            numerator = num;
            denominator = den;
        }
        public static Fraction operator +(Fraction a, Fraction b)
        {
            if (a.denominator != b.denominator)
            {
                int denom = a.denominator*b.denominator;
                int numer_a = a.numerator*b.denominator;
                int numer_b = b.numerator*a.denominator;
                int numer = numer_a + numer_b;
                if (denom<numer)
                {
                    for (int i = denom; i>1; i--)
                    {
                        if ((numer % i==0) && (denom % i==0))
                        {
                            numer = numer/i;
                            denom = denom/i;
                        }
                    }
                }
                else
                {
                    for (int i = numer; i>1; i--)
                    {
                        if ((numer % i==0) && (denom % i==0))
                        {
                            numer = numer/i;
                            denom = denom/i;
                        }
                    }
                }
                return new Fraction(numer, denom);
            }
            else
            {
                return new Fraction(a.numerator+b.numerator, a.denominator);
            }
        }
        public static bool operator ==(Fraction a, Fraction b)
        {
            if (a.denominator<a.numerator)
            {
                for (int i = a.denominator; i>2; i--)
                {
                    if (a.numerator%i==0 && a.denominator%i==0)
                    {
                        a.numerator = a.numerator/i;
                        a.denominator = a.denominator/i;
                    }
                }
            }
            else
            {
                for (int i = a.numerator; i>2; i--)
                {
                    if (a.numerator%i==0 && a.denominator%i==0)
                    {
                        a.numerator = a.numerator/i;
                        a.denominator = a.denominator/i;
                    }
                }
            }
            if (b.denominator<b.numerator)
            {
                for (int i = b.denominator; i>2; i--)
                {
                    if (b.numerator%i==0 && b.denominator%i==0)
                    {
                        b.numerator = b.numerator/i;
                        b.denominator = b.denominator/i;
                    }
                }
            }
            else
            {
                for (int i = b.numerator; i>2; i--)
                {
                    if (b.numerator%i==0 && b.denominator%i==0)
                    {
                        b.numerator = b.numerator/i;
                        b.denominator = b.denominator/i;
                    }
                }
            }
            if (a.numerator != b.numerator || a.denominator != b.denominator )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool operator !=(Fraction a, Fraction b)
        {
            if (a.denominator<a.numerator)
            {
                for (int i = a.denominator; i>2; i--)
                {
                    if (a.numerator%i==0 && a.denominator%i==0)
                    {
                        a.numerator = a.numerator/i;
                        a.denominator = a.denominator/i;
                    }
                }
            }
            else
            {
                for (int i = a.numerator; i>2; i--)
                {
                    if (a.numerator%i==0 && a.denominator%i==0)
                    {
                        a.numerator = a.numerator/i;
                        a.denominator = a.denominator/i;
                    }
                }
            }
            if (b.denominator<b.numerator)
            {
                for (int i = b.denominator; i>2; i--)
                {
                    if (b.numerator%i==0 && b.denominator%i==0)
                    {
                        b.numerator = b.numerator/i;
                        b.denominator = b.denominator/i;
                    }
                }
            }
            else
            {
                for (int i = b.numerator; i>2; i--)
                {
                    if (b.numerator%i==0 && b.denominator%i==0)
                    {
                        b.numerator = b.numerator/i;
                        b.denominator = b.denominator/i;
                    }
                }
            }
            if (a.numerator != b.numerator || a.denominator != b.denominator)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
#nullable enable
        public override bool Equals(object? obj)
        {
            if (!(obj is Fraction objec)) return false;
            if (this == objec) return true;
            else return false;
        }
#nullable disable
        public override int GetHashCode()
        {
            return HashCode.Combine(numerator, denominator);
        }

        public override string ToString()
        {
            return numerator + "/" + denominator;
        }

    }

    public interface IRotatable
    {
        Fraction[] angle { get; set; }
        Fraction[] angle_velocity { get; }
    }
    public class RotateCommand : ICommand
    {
        IRotatable _object;
        public RotateCommand(IRotatable obj)
        {
            _object = obj;
        }

        public void Execute()
        {
            _object.angle[0] = _object.angle[0] + _object.angle_velocity[0];
            _object.angle[1] = _object.angle[1] + _object.angle_velocity[1];
            Fraction[] resultAngle = new Fraction[2] { _object.angle[0], _object.angle[1] };
            _object.angle = resultAngle;
        }
    }
}
