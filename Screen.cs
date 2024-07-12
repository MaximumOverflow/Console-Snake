namespace Snakes;

internal class Screen
{
    internal Buffer Buffer { get; set; }

    internal Screen(int width, int height)
    {
        Buffer = new Buffer(width, height);
    }

    internal void Render()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write('+');

        for (int i = 0; i < Buffer.Width; i++)
        {
            Console.Write('-');
        }

        Console.WriteLine('+');
        
        int linePos = 0;

        foreach (char ch in Buffer.Chars)
        {
            if (linePos == 0)
            {
                Console.Write('|');
            }

            Console.Write(ch);
            linePos++;

            if (linePos == Buffer.Width)
            {
                Console.WriteLine('|');
                linePos = 0;
            }
        }

        Console.Write('+');

        for (int i = 0; i < Buffer.Width; i++)
        {
            Console.Write('-');
        }

        Console.WriteLine('+');
    }

    internal void Update(Snake snake)
    {
        Buffer.Reset();
        Buffer.SetPos(snake.Pellet.Position, '+');

        foreach (Position pos in snake.Obstacles)
        {
            Buffer.SetPos(pos, '&');
        }

        foreach (Position pos in snake.BodyParts)
        {
            Buffer.SetPos(pos, 'o');
        }

        Buffer.SetPos(snake.Position, 'O');
    }
}
