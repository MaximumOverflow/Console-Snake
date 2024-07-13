using System.Text;

namespace Snakes;

internal class Screen
{
    private readonly Buffer _buffer;
    private readonly BufferView _view;

    private readonly string _boxChars = Console.OutputEncoding.EncodingName == Encoding.UTF8.EncodingName
        ? " \u2503\u2503 \u2501\u250f\u2513 \u2501\u2517\u251b" // " ┃┃ ━┏┓ ━┗┛"
        : " || -++ -++";
    
    private readonly string _headChars = Console.OutputEncoding.EncodingName == Encoding.UTF8.EncodingName
        ? "\u25c0\u25b6\u25b2\u25bc" // "◀▶▲▼"
        : "OOOO";

    private readonly string _bodyChars = Console.OutputEncoding.EncodingName == Encoding.UTF8.EncodingName
        ? "\u2551\u2554\u255a\u2557\u255d\u2550"
        : "oooooo";

    private readonly char _pelletChar = Console.OutputEncoding.EncodingName == Encoding.UTF8.EncodingName
        ? '\u0f36'
        : '*';
    
    private readonly char _obstacleChar = Console.OutputEncoding.EncodingName == Encoding.UTF8.EncodingName
        ? '\u2388'
        : '#';

    internal Screen(int width, int height)
    {
        _buffer = new Buffer(width + 3, height + 2);
        _view = new BufferView(_buffer, 1, width, 1, height);
        
        // Initialize play area box
        var sx = width + 1;
        var sy = height + 1;
        
        for (var y = 0; y <= sy; y++)
        {
            var maskY = 0;
            if (y == 0) maskY |= 0b0100;
            if (y == sy) maskY |= 0b1000;
            for (var x = 0; x <= sx; x++)
            {
                var mask = maskY;
                if (x == 0) mask |= 0b0001;
                if (x == sx) mask |= 0b0010;
                _buffer[x, y] = _boxChars[mask];
            }
        }

        sx++;
        for (var y = 0; y <= sy; y++)
            _buffer[sx, y] = '\n';
    }

    internal void Render()
    {
        Console.SetCursorPosition(0, 0);
        Console.Out.Write(_buffer.Chars);
    }

    internal void Update(Snake snake)
    {
        _view.Clear();
        _view[snake.Pellet.Position] = _pelletChar;

        foreach (var pos in snake.Obstacles)
            _view[pos] = _obstacleChar;

        for (var i = 0; i < snake.BodyParts.Count; i++)
        {
            var part = snake.BodyParts[i];
            var lastPart = i == 0 ? part : snake.BodyParts[i - 1];
            
            _view[part.Position] = (lastPart.Direction, part.Direction) switch
            {
                (Direction.Up, Direction.Up) or (Direction.Down, Direction.Down) => _bodyChars[0],       // ║
                (Direction.Left, Direction.Down) or (Direction.Up, Direction.Right) => _bodyChars[1],    // ╔
                (Direction.Left, Direction.Up) or (Direction.Down, Direction.Right) => _bodyChars[2],    // ╚
                (Direction.Right, Direction.Down) or (Direction.Up, Direction.Left) => _bodyChars[3],    // ╗
                (Direction.Right, Direction.Up) or (Direction.Down, Direction.Left) => _bodyChars[4],    // ╝
                (Direction.Right, Direction.Right) or (Direction.Left, Direction.Left) => _bodyChars[5], // ═
                _ => throw new NotImplementedException($"{lastPart.Direction} {part.Direction}"),
            };
        }
        
        _view[snake.Position] = _headChars[(int) snake.Direction];
    }
}
