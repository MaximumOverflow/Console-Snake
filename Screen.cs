using System.Text;

namespace Snakes;

internal class Screen
{
    private readonly Buffer _buffer;
    private readonly BufferView _view;

    private readonly string _boxChars = " || -++ -++";

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
        _view[snake.Pellet.Position] = '+';

        foreach (var pos in snake.Obstacles)
            _view[pos] = '&';

        foreach (var pos in snake.BodyParts)
            _view[pos] = 'o';

        _view[snake.Position] = 'O';
    }
}
