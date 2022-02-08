public class Bar
{
    public int Min;
    public int Max;
    public int Rate;
    private bool CanBuildBar = false;

    public Bar(int min, int max, int rate)
    {
        if(min >= max)
        {
            Console.WriteLine("Min value is bigger than the max one!");
        }
        else if(((max - min) / rate) < 3)
        {
            Console.WriteLine("The values are smaller than 3");
        }
        else
        {
            CanBuildBar = true;
        }
    }
    public int[] GetValues()
    {
        if(!CanBuildBar) return new int[0];
        var values = new int[((Max - Min) / Rate) + 1];
        int j = 0;
        for(int i = Min; i < Max; i += Rate)
        {
            values[j] = i;
            j++;
        }
        return values;
    }
}

