using System.Text;
using static System.Console;

public static class ConsoleHelper
{
    private static bool _canColor = true;
    public static void EnableColors() { _canColor = true; }
    public static void DisableColors() { _canColor = false; }

    public static void ColorWriteLine(string text, ConsoleColor color)
    {
        if(_canColor) ForegroundColor = color;
        WriteLine(text);
        ResetColor();
    }

    public static void ColorWrite(string text, ConsoleColor color)
    {
        if(_canColor) ForegroundColor = color;
        Write(text);
        ResetColor();
    }

    public static void ClearScreen(int initialCursorPosition)
    {
        int currentCursor = Console.CursorTop;
        int linesToClear = initialCursorPosition - currentCursor - 1;
        linesToClear = linesToClear * -1;
        SetCursorPosition(0, initialCursorPosition);
        for (int i = 0; i < linesToClear; i++)
        {
            Write(new string(' ', Console.BufferWidth));
        }
        SetCursorPosition(0, currentCursor - (linesToClear - 1));
    }

    public static void PressKeyToContinue(string prompt)
    {
        Write(prompt);
        ReadKey(true);
        Write("\n");
    }

    public static void DisplayColumn(string input, char separator = '|')
    {
        string[] lines = input.Split('\n');
        // Loop to see what is the biggest line
        int biggestLine = -1;
        List<Tuple<string,string>> pairs = new List<Tuple<string, string>>();
        foreach(string line in lines)
        {
            if(string.IsNullOrEmpty(line)) continue;
            string[] parts = line.Split(separator);
            string first = parts[0];
            string second = parts[1];
            if(first.Length > biggestLine) biggestLine = first.Length;
            second = separator + second;
            pairs.Add(new Tuple<string,string>(first,second));
        }
        biggestLine = biggestLine * -1;
        string format = "{0," + biggestLine + "}{1," + biggestLine + "}";

        StringBuilder sb = new StringBuilder();
        foreach(var pair in pairs)
        {
            string line = String.Format(format, pair.Item1, pair.Item2);
            sb.AppendLine(line);
        }
        WriteLine(sb.ToString());
    }

}
