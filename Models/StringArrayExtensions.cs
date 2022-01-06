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

    public static string Combine(this string[] values, string separator)
    {
        string combinedWords = "";
        for(int i = 0; i < values.Length; i++) 
        {
            combinedWords += values[i];
            if(i + 1 != values.Length) combinedWords += separator;
        }
        return combinedWords;
    }

}
