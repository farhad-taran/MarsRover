using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover
{
    public class Grid
    {
        private readonly Coordinate _maximumBoundaries;
        private readonly Coordinate _minimumBoundaries;
        private readonly List<Rover> _rovers;

        public Grid(Coordinate minimumBoundaries, Coordinate maximumBoundaries)
        {
            _maximumBoundaries = maximumBoundaries;
            _minimumBoundaries = minimumBoundaries;
            _rovers = new List<Rover>();
        }
        
        public Result AddRover(Rover rover, Command[] movements)
        {
            if (_rovers.Any(r => r.Coordinate.Equals(rover.Coordinate)))
                return Error.RoverExists(rover.Id);
            
            if (rover.Coordinate.IsWithin(_minimumBoundaries, _maximumBoundaries) == false)
                return Error.OutOfBoundRover(rover.Id);
            
            var obstacles = _rovers
                .Where(r => r.Id != rover.Id)
                .Select(r => r.Coordinate).ToArray();

            var moveResult = rover.Move(movements, obstacles, _minimumBoundaries, _maximumBoundaries);

            if (moveResult.IsSuccess == false)
                return moveResult;
            
            _rovers.Add(moveResult.Value);

            return moveResult;
        }

        public string[] GetRoverStatuses()
        {
            return _rovers.Select(r => r.ToString()).ToArray();
        }
    }
}
