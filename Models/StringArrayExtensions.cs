public static class StringArrayExtensions 
{
    public static string[] RemoveWhiteSpace(this string[] values)
    {
        string[] cleanedValues = new string[values.Length];
        for(int i = 0; i < values.Length; i++)
        {
            cleanedValues[i] = values[i].Trim();
        }
        return cleanedValues;
    }

}
