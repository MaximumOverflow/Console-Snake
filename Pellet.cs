namespace Snakes;

internal class Pellet
{
    private readonly Random _random = new();
    internal Position Position { get; set; } 

    internal Pellet(Position upperLeftBound, Position lowerRightBound, List<Position> obstacles, List<BodyPart> snakeParts)
    {
        Respawn(upperLeftBound, lowerRightBound, obstacles, snakeParts);
    }

    internal void Respawn(Position upperLeftBound, Position lowerRightBound, List<Position> obstacles, List<BodyPart> snakeParts)
    {
        var retry = true;
        while (retry)
        {
            Position = new Position(
                _random.Next(upperLeftBound.X, lowerRightBound.X),
                _random.Next(upperLeftBound.Y, lowerRightBound.Y)
            );
            retry = obstacles.Any(Position, (in Position posA, in Position posB) => posA == posB) ||
                snakeParts.Any(Position, (in Position posA, in BodyPart posB) => posA == posB.Position);
        }
    }
}
