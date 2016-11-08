namespace ArtificialLife
{
    public class Coord
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Coord(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Coord operator +(Coord a, Coord b)
        {
            return new Coord(a.X + b.X, a.Y + b.Y);
        }

        public static bool operator ==(Coord a, Coord b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Coord a, Coord b)
        {
            return !(a == b);
        }

        public static Coord operator *(int r, Coord c)
        {
            return new Coord(r * c.X, r * c.Y);
        }
    }
}
