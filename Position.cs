using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snakes;

internal struct Position
{
    internal int X { get; set; }
    internal int Y { get; set; }

    internal Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}
