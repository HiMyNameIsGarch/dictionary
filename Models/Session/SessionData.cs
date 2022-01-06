public class SessionData 
{
    public SessionData(Config config, Dictionary<string[], string[]> pairs) 
    {
        Config = config;
        Pairs = pairs;
        WrongPairs = new Dictionary<string[], string[]>();
    }

    public Config Config { get; }

    public Dictionary<string[], string[]> Pairs { get; private set; }

    public Dictionary<string[], string[]> WrongPairs { get; private set; }

    public void DisplayPairsOf(int num)
    {
    }

    public void ResetWrongPairs()
    {
        WrongPairs = new Dictionary<string[], string[]>();
    }

    public void ShufflePairs()
    {
        Random rand = new Random();
        Pairs = Pairs.OrderBy(x => rand.Next())
            .ToDictionary(item => item.Key, item => item.Value);
    }

    public IMode GetCurrentMode()
    {
        switch(Config.Mode)
        {
            case ModeType.None:
                return new None();
            case ModeType.LearnAndAnswer:
                return new LearnAndAnswer();
            case ModeType.Persistent:
                return new Persistent();
            default: 
                Console.WriteLine("Mode type not found, setting default ( None )");
                return new None();
        }
    }

    public ISession GetCurrentSession()
    {
        switch(Config.FileExtension) {
            case FileExtension.Words:
                return new WordsSession(this);
            case FileExtension.IrregularVerbs:
                return new VerbsSession(this);
            default:
                Console.WriteLine("File Extension not found, setting default ( Words )");
                return new WordsSession(this);
        }
    }
}

