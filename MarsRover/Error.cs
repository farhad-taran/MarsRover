namespace MarsRover
{
    public class Error
    {
        public string Code { get; }

        public Error(string code)
        {
            Code = code;
        }

        public static readonly Error MessageIsEmpty = new Error("error.message.missing");
        public static readonly Error MessageIsInvalid = new Error("error.message.invalid");
        public static readonly Error InvalidGridSize = new Error("error.grid.size.invalid");
        public static readonly Error InvalidRoverDetails = new Error("error.rover.details.invalid");
        public static readonly Error InvalidDirection = new Error("error.direction.invalid");

        public static Error OutOfBoundRover(string roverId) => new Error($"error.rover.[{roverId}].out.of.bound");
        public static Error RoverExists(string roverId) => new Error($"error.rover.[{roverId}].exists");
        public static Error Obstacle(string roverId, Coordinate coordinate) => new Error($"error.rover.[{roverId}].met.obstacle.[{coordinate.X},{coordinate.Y}]");

        public override string ToString() => Code;
    }
}