using static System.Console;
using static ConsoleHelper;

public class Average
{
    public Average(string before, string after)
    {
        _before = before;
        _after = after;
    }

    public static int decimals = 2;
    private readonly string _before;
    private readonly string _after;
    private List<double> Values = new List<double>();

    public double LastValue
    {
        get { return Round(Values.Count > 0 ? Values.Last() : 0); }
    }
    public double AvarageNum
    {
        get { return GetAvarage(Values); }
    }

    public void ResetValue()
    {
        Values = new List<double>();
    }
    public void Add(double value)
    {
        Values.Add(value);
    }

    public void DisplayText(double value, ConsoleColor color)
    {
        Write(_before);
        ColorWrite(value.ToString(), color);
        Write(_after + "\n");
    }
    public void DisplayTextOnLast(int num, bool isAverage)
    {
        if(Values.Count < num) return;
        var values = Values.TakeLast(num).ToList();
        if(isAverage)
        {
            DisplayText(GetAvarage(values), ConsoleColor.Yellow);
            return;
        }
        for(int i = 0; i < values.Count; i++)
        {
            Write($"{i + 1}. ");
            DisplayText(values[i], ConsoleColor.DarkYellow);
            Write("\n");
        }
    }
    public double GetLast(int num)
    {
        if(Values.Count < num) return 0;
        var values = Values.TakeLast(num).ToList();
        return GetAvarage(values);
    }
    private double GetAvarage(List<double> list)
    {
        return list.Count > 0 ? Round(list.Average()) : 0.0; 
    }
    private double Round(double num)
    {
        return Math.Round(num, decimals);
    }
}
