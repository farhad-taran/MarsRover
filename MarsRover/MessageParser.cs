using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MarsRover
{
    public class MessageParser
    {
        private static readonly Regex GridDetailsRegex = new Regex(@"\d\s\d");
        private static readonly Regex RoverDetailsRegex = new Regex(@"\d\s\d\s(N|E|S|W)");
        private static readonly Regex MovementDetailsRegex = new Regex(@"(L|R|M)");
        private static readonly string[] LineSeparators = new[] { "\r\n", "\r", "\n" };

        private static readonly Dictionary<char, Command> MovementsMap = new Dictionary<char, Command>
            {
                {'L', Command.TurnLeft},
                {'R', Command.TurnRight},
                {'M', Command.Move},
            };

        public string HandleMessage(string message)
        {
            var result = ParseMessage(message);

            return result;
        }

        private Result<string[]> SplitLines(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return Error.MessageIsEmpty;

            var lines = message.Split(LineSeparators, StringSplitOptions.None);

            if (lines.Length < 5)
                return Error.MessageIsInvalid;

            var gridDetailsIsValid = GridDetailsRegex.IsMatch(lines[0].Trim());
            var roverParts = lines.Skip(1).ToArray();
            var roverDetailsAreValid = roverParts.Where((rp, index) => index % 2 == 0)
                .All(rd => RoverDetailsRegex.IsMatch(rd));
            var roverMovementsAreValid = roverParts.Where((rp, index) => index % 2 > 0)
                .All(rm => MovementDetailsRegex.IsMatch(rm));

            return gridDetailsIsValid && roverDetailsAreValid && roverMovementsAreValid ?
                Result<string[]>.Success(lines) :
                Result<string[]>.Failure(Error.MessageIsInvalid);
        }

        private string ParseMessage(string message)
        {
            var partsResult = SplitLines(message);

            if (partsResult.IsSuccess == false)
                return Lines(partsResult.Errors);

            var lineParts = partsResult.Value;
            var gridDetails = lineParts.First();
            var grid = CreateGrid(gridDetails);

            if (grid.IsSuccess == false)
                return Lines(grid.Errors);

            var roverDetails = lineParts.Skip(1).ToArray();
            var result = AddRovers(grid.Value, roverDetails);

            return result.IsSuccess ? Lines(grid.Value.GetRoverStatuses()) : Lines(result.Errors);
        }

        private Result AddRovers(Grid grid, string[] roverParts)
        {
            for (int i = 0; i < roverParts.Length - 1; i += 2)
            {
                var rover = CreateRover(roverParts[i]);

                if (rover.IsSuccess == false)
                    return rover;

                var movements = roverParts[i + 1].ToArray().Select(m => MovementsMap[m]).ToArray();

                var addResult = grid.AddRover(rover.Value, movements);
                if (addResult.IsSuccess == false)
                    return addResult;
            }

            return Result.Success();
        }

        private static Result<Grid> CreateGrid(string gridDetails)
        {
            var parts = gridDetails.Split(" ");
            var hasX = int.TryParse(parts.FirstOrDefault(), out var gridX);
            var hasY = int.TryParse(parts.LastOrDefault(), out var gridY);

            if (!hasX || !hasY || gridX == 0 && gridY == 0)
                return Error.InvalidGridSize;

            var minimumBoundaries = new Coordinate(0, 0);
            var maximumBoundaries = new Coordinate(gridX, gridY);

            return new Grid(minimumBoundaries, maximumBoundaries);
        }

        private static Result<Rover> CreateRover(string roverDetails)
        {
            var parts = roverDetails.Split(" ");
            var hasX = int.TryParse(parts[0], out var x);
            var hasY = int.TryParse(parts[1], out var y);

            if (!hasX || !hasY)
                return Error.InvalidRoverDetails;

            var direction = CreateDirection(parts[2]);
            if (direction.IsSuccess == false)
                return direction.Errors;

            return new Rover(new Coordinate(x, y), direction.Value);
        }

        public static Result<Direction> CreateDirection(string direction)
        {
            switch (direction)
            {
                case "N":
                    return Direction.North;
                case "E":
                    return Direction.East;
                case "S":
                    return Direction.South;
                case "W":
                    return Direction.West;
                default:
                    return Error.InvalidDirection;
            }
        }

        private static string Lines(IEnumerable<Error> errors) => Lines(errors.Select(e => e.Code));

        private static string Lines(IEnumerable<string> lines) => string.Join(Environment.NewLine, lines);
    }
}