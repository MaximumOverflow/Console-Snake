using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Snakes;

internal class Snake
{
    private object syncLock = new object();
    private Direction direction;

    internal Position Position { get; set; }
    internal int Speed { get; set; } = 1;
    internal int Length { get; set; }
    internal Pellet Pellet { get; set; }
    internal Position UpperLeftBound { get; set; }
    internal Position LowerRightBound { get; set; }
    internal List<Position> BodyParts { get; set; } = new List<Position>();
    internal List<Position> Obstacles { get; set; } = new List<Position>();
    internal Direction Direction
    {
        get
        {
            lock (syncLock)
            {
                return direction;
            }
        }

        set
        {
            lock (syncLock)
            {
                direction = value;
            }
        }
    }
    
    internal Snake(Direction direction, int speed, int length, Position upperLeftBound, Position lowerRightBound)
    {
        Direction = direction;
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

        switch (Direction)
        {
            case Direction.Up:
                Position = new Position(Position.X, Position.Y - 1);
                break;
            case Direction.Down:
                Position = new Position(Position.X, Position.Y + 1);
                break;
            case Direction.Left:
                Position = new Position(Position.X - 1, Position.Y);
                break;
            case Direction.Right:
                Position = new Position(Position.X + 1, Position.Y);
                break;
        }

        while (BodyParts.Count > Length)
        {
            BodyParts.RemoveAt(0);
        }

        if (Position.X < UpperLeftBound.X
            || Position.X > LowerRightBound.X
            || Position.Y < UpperLeftBound.Y
            || Position.Y > LowerRightBound.Y)
        {
            return false;
        }

        foreach (var part in BodyParts)
        {
            if (part.X == Position.X
                && part.Y == Position.Y)
            {
                return false;
            }
        }

        foreach (var obstacle in Obstacles)
        {
            if (Position.X == obstacle.X
                && Position.Y == obstacle.Y)
            {
                return false;
            }
        }

        if (Position.X == Pellet.Position.X
            && Position.Y == Pellet.Position.Y)
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
                        Direction = Direction == Direction.Down ? Direction : Direction.Up;
                        break;
                    case 'a':
                        Direction = Direction == Direction.Right ? Direction : Direction.Left;
                        break;
                    case 's':
                        Direction = Direction == Direction.Up ? Direction : Direction.Down;
                        break;
                    case 'd':
                        Direction = Direction == Direction.Left ? Direction : Direction.Right;
                        break;
                }
            }
        }).Start();
    }
}
