public class ConfigParser: IParser<string>
{

    public ConfigParser(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; }

    public string ParseFile()
    {
        return "";
    }
    public bool HasErrors()
    {
        return true;
    }
}
