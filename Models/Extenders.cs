public static class Extenders
{
    public static string ToFormattedString(this FileExtension extension)
    {
        switch(extension)
        {
            case FileExtension.Words: 
                return "Words";
            case FileExtension.IrregularVerbs: 
                return "Irregular Verbs";
            default:
                return "Default";
        }
    }

    public static string ToFormattedString(this ModeType extension)
    {
        switch(extension)
        {
            case ModeType.LearnAndAnswer: 
                return "Learn And Answer";
            case ModeType.Persistent: 
                return "Persistent";
            default:
                return "None";
        }
    }

    public static int Length(this int num)
    {
        return num.ToString().Length;
    }
}
