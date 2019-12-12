using System;

namespace adv_of_code_2019.Classes
{
    [System.Diagnostics.DebuggerDisplay("X={X} Y={Y}")]
    internal struct Point : IEquatable<Point>
    {
        public int X { get; }

        public int Y { get; }

        public double Angle
        {
            get
            {
                if (myAngle == null)
                {
                    if (X == 0 && Y == 0)
                    {
                        throw new InvalidOperationException("(0, 0) does not have an angle related to (0, 0)!");
                    }
                    else
                    {
                        myAngle = Math.Atan2(-Y, X);
                        myAngle = (Math.PI / 2 - Angle + 2 * Math.PI) % (2 * Math.PI);
                    }
                }

                return myAngle.Value;
            }
        }

        public double Length => myLength ?? (myLength = Math.Sqrt(X * X + Y * Y)).Value;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
            myAngle = null;
            myLength = null;
        }

        public override bool Equals(object obj) => obj is Point other ? Equals(other) : base.Equals(obj);

        public override int GetHashCode() => X ^ Y;

        public bool Equals(Point other) => X == other.X && Y == other.Y;

        public static bool operator ==(Point a, Point b) => a.Equals(b);

        public static bool operator !=(Point a, Point b) => !a.Equals(b);

        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);

        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);

        public static Point operator *(Point a, int b) => new Point(a.X * b, a.Y * b);

        public static Point Empty { get; } = new Point(0, 0);

        private double? myAngle;
        private double? myLength;
    }
}