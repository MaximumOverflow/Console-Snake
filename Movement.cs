namespace Snakes;

internal enum Direction
{
	Left,
	Right,
	Up,
	Down
}

internal record struct Position(int X, int Y);
