using System;
using FluentAssertions;
using Xunit;

namespace MarsRover.Tests
{
    public class AcceptanceTests
    {
        private readonly MessageParser _sut;

        public AcceptanceTests()
        {
            _sut = new MessageParser();
        }

        [Fact]
        public void WhenMessageIsValid_ItAddsAndMovesTheRovers()
        {
            var input = Lines(
                "5 5",
                "1 2 N",
                "LMLMLMLMM",
                "3 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be(Lines(
                "1 3 N",
                "5 1 E"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void WhenMessageIsNullOrEmpty_ReturnsErrorCode(string input)
        {
            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.missing");
        }

        [Fact]
        public void WhenMessageHasMissingGridSize_ReturnsErrorCode()
        {
            var input = Lines(
                "1 2 N",
                "LMLMLMLMM",
                "3 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageGridSizeTooSmall_ReturnsErrorCode()
        {
            var input = Lines(
                "0 0",
                "1 2 N",
                "LMLMLMLMM",
                "3 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.grid.size.invalid");
        }

        [Fact]
        public void WhenMessageHasInvalidXForGridSize_ReturnsErrorCode()
        {
            var input = Lines(
                "x 5",
                "1 2 N",
                "LMLMLMLMM",
                "3 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasInvalidYForGridSize_ReturnsErrorCode()
        {
            var input = Lines(
                "x 5",
                "1 2 N",
                "LMLMLMLMM",
                "3 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasNegativeGridSize_ReturnsErrorCode()
        {
            var input = Lines(
                "5 -5",
                "1 2 N",
                "LMLMLMLMM",
                "3 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasMissingFirstRoverCoordinates_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "LMLMLMLMM",
                "3 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasOutOfBoundFirstRoverCoordinates_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "6 2 N",
                "LMLMLMLMM",
                "3 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.rover.[6-2-N].out.of.bound");
        }

        [Fact]
        public void WhenMessageHasMissingFirstRoverMovements_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "1 2 N",
                "3 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasInvalidXForFirstRoverCoordinates_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "X 2 N",
                "LMLMLMLMM",
                "3 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasInvalidYForFirstRoverCoordinates_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "1 Y N",
                "LMLMLMLMM",
                "3 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasInvalidDirectionForFirstRoverCoordinates_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "1 2 9",
                "LMLMLMLMM",
                "3 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasMissingSecondRoverCoordinates_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "1 2 N",
                "LMLMLMLMM",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasOutOfBoundSecondRoverCoordinates_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "1 2 N",
                "LMLMLMLMM",
                "3 6 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.rover.[3-6-E].out.of.bound");
        }

        [Fact]
        public void WhenMessageHasInvalidXForSecondRoverCoordinates_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "1 2 N",
                "LMLMLMLMM",
                "X 3 E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasInvalidYForSecondRoverCoordinates_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "1 2 N",
                "LMLMLMLMM",
                "3 Y E",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasInvalidDirectionForSecondRoverCoordinates_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "1 2 N",
                "LMLMLMLMM",
                "3 3 2",
                "MMRMMRMRRM");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasMissingSecondRoverMovements_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "1 2 N",
                "LMLMLMLMM",
                "3 3 E");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.message.invalid");
        }

        [Fact]
        public void WhenMessageHasOverlappingRovers_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "1 2 N",
                "L",
                "1 2 N",
                "R");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.rover.[1-2-N].exists");
        }

        [Fact]
        public void WhenMessageCausesRoversToCrash_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "1 2 N",
                "L",
                "1 1 N",
                "M");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.rover.[1-1-N].met.obstacle.[1,2]");
        }

        [Fact]
        public void WhenMessageCausesRoverToGoOffGrid_ReturnsErrorCode()
        {
            var input = Lines(
                "5 5",
                "1 2 N",
                "MMMM",
                "1 1 N",
                "M");

            var output = _sut.HandleMessage(input);

            output.Should().Be("error.rover.[1-2-N].out.of.bound");
        }

        private static string Lines(params string[] parts)
        {
            return string.Join(Environment.NewLine, parts);
        }
    }
}
