﻿namespace Snakes;

internal record struct BodyPart(Position Position, Direction Direction);

internal class Snake
{
    private volatile Direction _direction;
    internal Direction Direction => _direction;

    private volatile bool _alive = true;
    internal bool Alive => _alive;

    internal Position Position { get; set; }
    internal int Speed { get; } = 1;
    internal int Length { get; set; }
    internal Pellet Pellet { get; }
    internal Position UpperLeftBound { get; }
    internal Position LowerRightBound { get; }
    internal List<Position> Obstacles { get; } = new();
    internal List<BodyPart> BodyParts { get; } = new();

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

    public void Move() => _alive = MoveInternal();

    private bool MoveInternal()
    {
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

    private void BeginInputHandle()
    {
        new Thread(() =>
        {
            while (Alive)
            {
                _direction = Console.ReadKey(true).KeyChar switch
                {
                    'w' => Direction == Direction.Down ? Direction : Direction.Up,
                    'a' => Direction == Direction.Right ? Direction : Direction.Left,
                    's' => Direction == Direction.Up ? Direction : Direction.Down,
                    'd' => Direction == Direction.Left ? Direction : Direction.Right,
                    _ => _direction
                };
            }
        }).Start();
    }
}
