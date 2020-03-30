using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover
{
    public struct Direction
    {
        public static readonly Direction North = new Direction("N", "E", "W");
        public static readonly Direction East = new Direction("E", "S", "N");
        public static readonly Direction South = new Direction("S", "W", "E");
        public static readonly Direction West = new Direction("W", "N", "S");

        private static readonly List<Direction> ValidDirections = new List<Direction>()
        {
            North,East,South,West
        };

        public string Current { get; }
        public string Right { get; }
        public string Left { get; }

        private Direction(string current, string right, string left)
        {
            Current = current;
            Right = right;
            Left = left;
        }
        
        public override string ToString() => Current;

        public Direction TurnTowards(Command command)
        {
            var right = Right;
            if (command == Command.TurnRight)
                return ValidDirections.Single(d => d.Current == right);

            var left = Left;
            if (command == Command.TurnLeft)
                return ValidDirections.Single(d => d.Current == left);

            return this;
        }
    }
}