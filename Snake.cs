namespace Snakes;

internal class Snake
{
    private volatile Direction _direction;
    internal Direction Direction => _direction;

    internal Position Position { get; set; }
    internal int Speed { get; set; } = 1;
    internal int Length { get; set; }
    internal Pellet Pellet { get; set; }
    internal Position UpperLeftBound { get; set; }
    internal Position LowerRightBound { get; set; }
    internal List<Position> BodyParts { get; set; } = new();
    internal List<Position> Obstacles { get; set; } = new();
    
    internal Snake(Direction direction, int speed, int length, Position upperLeftBound, Position lowerRightBound)
    {
        _direction = direction;
        Speed = speed;
        Length = length;
        UpperLeftBound = upperLeftBound;
        LowerRightBound = lowerRightBound;
        //TODO: gen obstacles
        Pellet = new Pellet(UpperLeftBound, LowerRightBound, Obstacles, BodyParts);
        Position = Pellet.Position;
        BeginInputHandle();
    }

    internal bool Move()
    {
        BodyParts.Add(Position);

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
        
        if(BodyParts.Any(Position, (in Position a, in Position b) => a == b))
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

    internal void BeginInputHandle()
    {
        new Thread(() =>
        {
            while (true)
            {
                var key = Console.ReadKey(true);

                switch (key.KeyChar)
                {
                    case 'w':
                        _direction = Direction == Direction.Down ? Direction : Direction.Up;
                        break;
                    case 'a':
                        _direction = Direction == Direction.Right ? Direction : Direction.Left;
                        break;
                    case 's':
                        _direction = Direction == Direction.Up ? Direction : Direction.Down;
                        break;
                    case 'd':
                        _direction = Direction == Direction.Left ? Direction : Direction.Right;
                        break;
                }
            }
        }).Start();
    }
}
