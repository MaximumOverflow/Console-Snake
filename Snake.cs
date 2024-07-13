namespace Snakes;

internal record struct BodyPart(Position Position, Direction Direction);

internal class Snake
{
    private readonly int _baseSpeed;
    internal Position Position { get; set; }
    internal bool Alive { get; private set; } = true;
    internal Direction Direction { get; private set; }
    internal float Speed => _baseSpeed + 0.1f * Length;
    internal int Length { get; set; }
    internal Pellet Pellet { get; }
    internal Position UpperLeftBound { get; }
    internal Position LowerRightBound { get; }
    internal List<Position> Obstacles { get; } = new();
    internal List<BodyPart> BodyParts { get; } = new();

    internal Snake(Direction direction, int baseSpeed, int length, Position upperLeftBound, Position lowerRightBound)
    {
        _baseSpeed = baseSpeed;
        Length = length;
        Direction = direction;
        UpperLeftBound = upperLeftBound;
        LowerRightBound = lowerRightBound;
        Pellet = new Pellet(UpperLeftBound, LowerRightBound, Obstacles, BodyParts);
        Position = Pellet.Position;

        var area = (lowerRightBound.X - upperLeftBound.X) * (lowerRightBound.Y - upperLeftBound.Y);
        var count = Random.Shared.Next(area / 100 * 1, area / 100 * 3);
        for (int i = 0; i < count; i++)
        {
            Position obstacle;
            do
            {
                var x = Random.Shared.Next(upperLeftBound.X + 1, lowerRightBound.X - 1);
                var y = Random.Shared.Next(upperLeftBound.Y + 1, lowerRightBound.Y - 1);
                obstacle = new(x, y);
            } while (obstacle == Position);
            Obstacles.Add(obstacle);
        }
    }

    public void Move() => Alive = MoveInternal();

    private bool MoveInternal()
    {
        HandleInput();
        BodyParts.Add(new(Position, Direction));

        Position = Direction switch
        {
            Direction.Up => Position with { Y = Position.Y - 1 },
            Direction.Down => Position with { Y = Position.Y + 1 },
            Direction.Left => Position with { X = Position.X - 1 },
            Direction.Right => Position with { X = Position.X + 1 },
            _ => Position,
        };

        if(BodyParts.Count > Length)
        {
            BodyParts.RemoveRange(0, BodyParts.Count - Length);
        }

        if (Position.X < UpperLeftBound.X
            || Position.X > LowerRightBound.X
            || Position.Y < UpperLeftBound.Y
            || Position.Y > LowerRightBound.Y)
        {
            return false;
        }
        
        if(BodyParts.Any(Position, (in Position a, in BodyPart b) => a == b.Position))
            return false;
        
        if(Obstacles.Any(Position, (in Position a, in Position b) => a == b))
            return false;

        if (Position == Pellet.Position)
        {
            Pellet.Respawn(UpperLeftBound, LowerRightBound, Obstacles, BodyParts);
            Length++;
        }

        return true;
    }

    private void HandleInput()
    {
        if(!Console.KeyAvailable) return;
        Direction = Console.ReadKey(true).KeyChar switch
        {
            'w' => Direction == Direction.Down ? Direction : Direction.Up,
            'a' => Direction == Direction.Right ? Direction : Direction.Left,
            's' => Direction == Direction.Up ? Direction : Direction.Down,
            'd' => Direction == Direction.Left ? Direction : Direction.Right,
            _ => Direction
        };
    }
}
