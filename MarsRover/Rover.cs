using System.Collections.Generic;
using System.Linq;

namespace MarsRover
{
    public class Rover
    {
        public string Id { get; }
        public Coordinate Coordinate { get; }
        public Direction Direction { get; }

        public Rover(Coordinate coordinate, Direction direction)
        {
            Coordinate = coordinate;
            Direction = direction;
            Id = $"{coordinate.X}-{coordinate.Y}-{direction}";
        }

        public override string ToString() => $"{Coordinate.X} {Coordinate.Y} {Direction}";

        public Result<Rover> Move(Command[] commands, Coordinate[] obstacles, Coordinate minimum, Coordinate maximum)
        {
            var newCoordinate = Coordinate;
            var newDirection = Direction;

            foreach (var command in commands)
            {
                if (command == Command.Move)
                {
                    newCoordinate = newCoordinate.IncrementInDirection(newDirection);

                    if (newCoordinate.IsWithin(minimum, maximum) == false)
                        return Error.OutOfBoundRover(Id);
                    
                    if (obstacles.Any(o => o.Equals(newCoordinate)))
                        return Error.Obstacle(Id, newCoordinate);
                }
                else
                {
                    newDirection = newDirection.TurnTowards(command);
                }
            }

            return new Rover(newCoordinate, newDirection);
        }
    }
}