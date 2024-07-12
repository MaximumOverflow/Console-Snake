using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snakes;

internal class Buffer
{
    internal int Width { get; set; }
    internal int Height { get; set; }
    internal char[,] Chars { get; set; }

    internal Buffer(int width, int height)
    {
        Width = width;
        Height = height;
        Chars = new char[Height, Width];
    }

    internal char GetPos(Position pos)
    {
        return Chars[pos.Y, pos.X];
    }

    internal void SetPos(Position pos, char ch)
    {
        Chars[pos.Y, pos.X] = ch;
    }

    internal void Reset()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                SetPos(new Position(x, y), ' ');
            }
        }
    }
}
