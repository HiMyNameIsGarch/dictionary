public class WordsParser: IParser<Dictionary<string, string>>
{

    public WordsParser(string filePath) 
    {
        FilePath = filePath;
    }

    public string FilePath { get; }

    public Dictionary<string, string> ParseFile()
    {
        return null;
    }
    public bool HasErrors() 
    {
        return true;
    }
}
