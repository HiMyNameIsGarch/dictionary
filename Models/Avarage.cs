public class Avarage
{
    public Avarage(string before, string after)
    {
        _before = before;
        _after = after;
    }

    public static int decimals = 2;
    private readonly string _before;
    private readonly string _after;

    public List<double> Values = new List<double>();
    public double LastValue
    {
        get { return Round(Values.Count > 0 ? Values.Last() : 0); }
    }
    public double AvarageNum
    {
        get { return GetAvarage(Values); }
    }

    public string GetText()
    {
        return _before + LastValue + _after;
    }
    public string GetText(double value)
    {
        return _before + Round(value) + _after;
    }
    public string GetTextOnLast(int num, bool avarage)
    {
        if(Values.Count < num) return "";
        var values = Values.TakeLast(num).ToList();
        if(avarage)
            return GetText(values.Average());

        string text = "";
        for(int i = 0; i < values.Count; i++)
        {
            text += i + 1 + ". " + GetText(values[i]);
            if(i + 1 != values.Count) text += "\n";
        }
        return text;
    }

    private double Round(double num)
    {
        return Math.Round(num, decimals);
    }

    private double GetAvarage(List<double> list)
    {
        return list.Count > 0 ? Round(list.Average()) : 0.0; 
    }
}
