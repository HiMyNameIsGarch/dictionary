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
        get { return Round(Values.Count > 1 ? Values.Last() : 0); }
    }

    public double AvarageNum
    {
        get { return GetAvarage(Values); }
    }

    public string GetText()
    {
        return _before + LastValue + _after;
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
