public class Bar
{
    public int Min;
    public int Max;
    public int Rate;
    public int[] Values;
    private bool CanBuildBar = false;

    public Bar(int min, int max, int rate)
    {
        if(min >= max)
        {
            Console.WriteLine("Min value is bigger than max one");
        }
        else if(((max - min) / rate) < 3)
        {
            Console.WriteLine("To little y coordonates");
        }
        else
        {
            CanBuildBar = true;
        }
        Max = max;
        Min = min;
        Rate = rate;
        Values = new int[0];
    }
    public void ComputeValues()
    {
        if(!CanBuildBar)
        {
            Values = new int[0];
            return;
        } 

        var num = ((Max - Min) / Rate) + 1;
        var values = new int[num];
        int j = 0;
        for(int i = Min; i <= Max; i += Rate)
        {
            values[j] = i;
            j++;
        }
        Values = values;
    }
}
