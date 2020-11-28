using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace adv_of_code_.Classes
{
    [DebuggerDisplay ("X={X} Y={Y}")]
    public struct Point : IEquatable<Point>
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
                        throw new InvalidOperationException ("(0, 0) does not have an angle related to (0, 0)!");
                    }
                    else
                    {
                        myAngle = Math.Atan2 (-Y, X);
                        myAngle = (Math.PI / 2 - Angle + 2 * Math.PI) % (2 * Math.PI);
                    }
                }

                return myAngle.Value;
            }
        }

        public double Length => myLength ?? (myLength = Math.Sqrt (X * X + Y * Y)).Value;

        public Point (int x, int y)
        {
            X = x;
            Y = y;
            myAngle = null;
            myLength = null;
        }

        public override bool Equals (object obj) => obj is Point other ? Equals (other) : base.Equals (obj);

        public override int GetHashCode () => X ^ Y;

        public bool Equals (Point other) => X == other.X && Y == other.Y;

        public static bool operator == (Point a, Point b) => a.Equals (b);

        public static bool operator != (Point a, Point b) => !a.Equals (b);

        public static Point operator + (Point a, Point b) => new Point (a.X + b.X, a.Y + b.Y);

        public static Point operator - (Point a, Point b) => new Point (a.X - b.X, a.Y - b.Y);

        public static Point operator * (Point a, int b) => new Point (a.X * b, a.Y * b);

        public static Point Empty { get; } = new Point (0, 0);

        private double? myAngle;
        private double? myLength;

        public List<Point> moveUp (int spaces)
        {
            var traveled_spaces = new List<Point> ();

            for (int i = 1; i <= spaces; i++)
            {
                traveled_spaces.Add (new Point (this.X, this.Y + i));
            }

            return traveled_spaces;
        }

        public List<Point> moveDown (int spaces)
        {
            var traveled_spaces = new List<Point> ();

            for (int i = 1; i <= spaces; i++)
            {
                traveled_spaces.Add (new Point (this.X, this.Y - i));
            }

            return traveled_spaces;
        }

        public List<Point> moveRight (int spaces)
        {
            var traveled_spaces = new List<Point> ();

            for (int i = 1; i <= spaces; i++)
            {
                traveled_spaces.Add (new Point (this.X + i, this.Y));
            }

            return traveled_spaces;
        }

        public List<Point> moveLeft (int spaces)
        {
            var traveled_spaces = new List<Point> ();

            for (int i = 1; i <= spaces; i++)
            {
                traveled_spaces.Add (new Point (this.X - i, this.Y));
            }

            return traveled_spaces;
        }
    }

    public struct PSet : IEquatable<PSet>
    {
        public P P1 { get; set; }

        public P P2 { get; set; }

        public P P3 { get; set; }

        public P P4 { get; set; }

        public P this [int index]
        {
            get
            {
                switch (index)
                {
                case 1:
                    return P1;
                case 2:
                    return P2;
                case 3:
                    return P3;
                case 4:
                    return P4;
                default:
                    throw new KeyNotFoundException ();
                }
            }
            set
            {
                switch (index)
                {
                case 1:
                    P1 = value;
                    break;
                case 2:
                    P2 = value;
                    break;
                case 3:
                    P3 = value;
                    break;
                case 4:
                    P4 = value;
                    break;
                }
            }
        }

        public PSet Clone ()
        {
            return new PSet
            {
                P1 = P1,
                    P2 = P2,
                    P3 = P3,
                    P4 = P4
            };
        }

        public bool Equals (PSet other)
        {
            if (ReferenceEquals (null, other)) return false;
            if (ReferenceEquals (this, other)) return true;
            return Equals (P1, other.P1) && Equals (P2, other.P2) && Equals (P3, other.P3) && Equals (P4, other.P4);
        }

        public override bool Equals (object obj)
        {
            if (ReferenceEquals (null, obj)) return false;
            if (ReferenceEquals (this, obj)) return true;
            if (obj.GetType () != this.GetType ()) return false;
            return Equals ((PSet) obj);
        }

        public override int GetHashCode ()
        {
            return HashCode.Combine (P1, P2, P3, P4);
        }
    }

    [DebuggerDisplay ("{X}, {Y}")]
    public struct P : IEquatable<P>
    {
        public int X { get; set; }

        public int Y { get; set; }

        public bool Equals (P other)
        {
            if (ReferenceEquals (null, other)) return false;
            if (ReferenceEquals (this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals (object obj)
        {
            if (ReferenceEquals (null, obj)) return false;
            if (ReferenceEquals (this, obj)) return true;
            if (obj.GetType () != this.GetType ()) return false;
            return Equals ((P) obj);
        }

        public override int GetHashCode ()
        {
            return HashCode.Combine (X, Y);
        }

        public IEnumerable<P> Around ()
        {
            yield return new P { X = X, Y = Y - 1 };
            yield return new P { X = X - 1, Y = Y };
            yield return new P { X = X + 1, Y = Y };
            yield return new P { X = X, Y = Y + 1 };
        }
    }
}