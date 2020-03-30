namespace MarsRover
{
    public struct Coordinate
    {
        public int X { get; }
        public int Y { get; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsWithin(Coordinate minimum, Coordinate maximum)
        {
            return maximum.X >= X && maximum.Y >= Y && minimum.X <= X && minimum.Y <= Y;
        }

        public Coordinate IncrementInDirection(Direction direction)
        {
            if (direction.Equals(Direction.North))
                return new Coordinate(X, Y + 1);
            
            if (direction.Equals(Direction.East))
                return new Coordinate(X + 1, Y);

            if (direction.Equals(Direction.South))
                return new Coordinate(X, Y - 1);

            if (direction.Equals(Direction.West))
                return new Coordinate(X - 1, Y);

            return this;
        }
    }
}