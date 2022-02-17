public class Axis
{
    public int Min;
    public int Max;
    public int Rate;
    public int[] Values;
    private bool CanBuildAxis = false;

    public Axis(int min, int max, int rate)
    {
        Max = max;
        Min = min;
        Rate = rate;
        if(Min >= Max)
        {
            Console.WriteLine("Mininum values is bigger than the maximum one");
        }
        else if(GetValue() < 3)
        {
            Console.WriteLine("The values on axis are too small");
        }
        else
        {
            CanBuildAxis = true;
        }
        Values = new int[0];
    }

    public void ComputeValues()
    {
        if(!CanBuildAxis)
        {
            Values = new int[0];
            return;
        } 

        var num = GetValue();
        var values = new int[num];
        int j = 0;
        for(int i = Min; i <= Max; i += Rate)
        {
            values[j] = i;
            j++;
        }
        Values = values;
    }
    private int GetValue()
    {
        return ((Max - Min) / Rate) + 1;
    }
}
