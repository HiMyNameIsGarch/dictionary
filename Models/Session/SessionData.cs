public class SessionData 
{
    public SessionData(Config config, Dictionary<string[], string[]> pairs) 
    {
        Config = config;
        Pairs = pairs;
    }

    public Config Config { get; }

    public Dictionary<string[], string[]> Pairs { get; private set; }

    public void ShufflePairs()
    {
        Random rand = new Random();
        Pairs = Pairs.OrderBy(x => rand.Next())
            .ToDictionary(item => item.Key, item => item.Value);
    }

    public ISession GetCurrentSession()
    {
        switch(Config.FileExtension) {
            case FileExtension.Words:
                return new WordsSession(this);
            case FileExtension.IrregularVerbs:
                return new VerbsSession(this);
            default:
                Console.WriteLine("File Extension not found, setting default");
                return new WordsSession(this);
        }
    }

}

