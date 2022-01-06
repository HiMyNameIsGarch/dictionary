using static System.Console;

public static class ConsoleHelper
{
    public static void ColorWriteLine(string text, ConsoleColor color, bool canColor)
    {
        if(canColor) ForegroundColor = color;
        WriteLine(text);
        ResetColor();
    }

    public static void ColorWrite(string text, ConsoleColor color, bool canColor)
    {
        if(canColor) ForegroundColor = color;
        Write(text);
        ResetColor();
    }

    public static void ClearScreen(int initialCursorPosition)
    {
        int currentCursor = Console.CursorTop;
        int linesToClear = initialCursorPosition - currentCursor - 1;
        linesToClear = linesToClear * -1;
        Console.SetCursorPosition(0, initialCursorPosition);
        for (int i = 0; i < linesToClear; i++)
        {
            Console.Write(new string(' ', Console.BufferWidth));
        }
        Console.SetCursorPosition(0, currentCursor - (linesToClear - 1));
    }

    public static void PressKeyToContinue(string prompt)
    {
        Console.Write(prompt);
        Console.ReadKey(true);
        Console.Write("\n");
    }

}
