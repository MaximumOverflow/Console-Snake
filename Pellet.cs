namespace Snakes;

internal class Pellet
{
    internal Position Position { get; set; } 

    internal Pellet(Position upperLeftBound, Position lowerRightBound, List<Position> obstacles, List<Position> snakeParts)
    {
        Respawn(upperLeftBound, lowerRightBound, obstacles, snakeParts);
    }

    internal void Respawn(Position upperLeftBound, Position lowerRightBound, List<Position> obstacles, List<Position> snakeParts)
    {
        var random = new Random();
        bool b = true;

        while (b)
        {
            b = false;
            Position = new Position((int)random.NextInt64(upperLeftBound.X, lowerRightBound.X),
                (int)random.NextInt64(upperLeftBound.Y, lowerRightBound.Y));

            foreach (var position in obstacles)
            {
                if (position.X == Position.X && position.Y == Position.Y)
                {
                    b = true;
                }
            }

            foreach (var position in snakeParts)
            {
                if (position.X == Position.X && position.Y == Position.Y)
                {
                    b = true;
                }
            }
        }

    }
}
