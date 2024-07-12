namespace Snakes;

internal readonly struct Buffer(int width, int height)
{
    public readonly int Width = width;
    public readonly int Height = height;
    public readonly char[] Chars = new char[width * height];
    
    public ref char this[int x, int y] => ref Chars[x + y * Width];
    public ref char this[Position pos] => ref Chars[pos.X + pos.Y * Width];

    public void Clear() => Chars.AsSpan().Fill(' ');
}

internal readonly struct BufferView
{
    private readonly int _stride;
    private readonly char[] _chars;
    private readonly int _offsetX, _offsetY;
    
    public readonly int Width, Height;
    
    public ref char this[int x, int y] => ref _chars[x + _offsetX + (y + _offsetY) * _stride];
    public ref char this[Position pos] => ref _chars[pos.X + _offsetX + (pos.Y + _offsetX) * _stride];

    public BufferView(Buffer buffer, int startX, int width, int startY, int height)
    {
        if (startX < 0 || width < 0 || startY < 0 || height < 0)
            throw new ArgumentOutOfRangeException();
        
        if (startX + width > buffer.Width || startY + height > buffer.Height)
            throw new ArgumentOutOfRangeException();

        Width = width;
        Height = height;
        _offsetX = startX;
        _offsetY = startY;
        _chars = buffer.Chars;
        _stride = buffer.Width;
    }

    public void Clear()
    {
        var ey = _offsetY + Height;
        for (var y = _offsetY; y < ey; y++)
            _chars.AsSpan(y * _stride + _offsetX, Width).Fill(' ');
    }
}
