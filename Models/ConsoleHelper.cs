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

}
