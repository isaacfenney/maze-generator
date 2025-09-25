namespace maze_generator
{
    class Coordinate
    {
        public int x; public int y;

        // constructor
        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        // other
        public static bool Equals(Coordinate a, Coordinate b)
        {
            return a.x == b.x && a.y == b.y;
        }
    }
}