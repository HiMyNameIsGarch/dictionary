using System.Text;

public class SessionData 
{
    public SessionData(ConfigOptions config) 
    {
        Config = config;
        Pairs = GetCurrentPairs().ParseFile();
        WrongPairs = new Dictionary<string[], string[]>();
        ResponseTime = new Average("Took -> ", " seconds.");
        Accuracy = new Average("Accuracy -> ", "%.");

        if(config.OutputHasColors)
            ConsoleHelper.EnableColors();
        else 
            ConsoleHelper.DisableColors();
    }

    public TimeSpan TimeOnSession { get; set; }

    public int Points { get; set; }

    public int TotalPairs { get; set; }

    public int TotalPoints { get; set; }

    public Average ResponseTime { get; }

    public Average Accuracy { get; }

    public ConfigOptions Config { get; }

    public Dictionary<string[], string[]> Pairs { get; private set; }

    public Dictionary<string[], string[]> WrongPairs { get; private set; }

    public int GetExpectedPoints(string[] keys, string[] values)
    {
        if(Config.FileExtension == FileExtension.IrregularVerbs)
        {
            return IrregularVerbs.MaxVerbs;
        }
        if(Config.ReverseWords)
        {
            return keys.Length;
        }
        else
        {
            return values.Length;
        }
    }
    public void SetPairs(Dictionary<string[], string[]> newPairs)
    {
        if(newPairs.Count != 0 && newPairs != null) Pairs = newPairs;
    }
    public List<Dictionary<string[], string[]>> SplitPairsIn(int num)
    {
        var list = new List<Dictionary<string[], string[]>>();
        int currentNumber = 0;
        int maxPairs = Pairs.Count;
        int maxIterations = (int)Math.Ceiling((double)maxPairs / (double)num);
        for(int i = 0; i < maxIterations; i++)
        {
            list.Add(Pairs.Skip(currentNumber).Take(num).ToDictionary(k => k.Key, v => v.Value));
            currentNumber += list.Last().Count;
        }
        return list;
    }

    public void DisplayPairs(Dictionary<string[], string[]> pairs)
    {
        StringBuilder sb = new StringBuilder();
        foreach(var pair in pairs)
        {
            string line = $"{pair.Key.Combine(", ")} | {pair.Value.Combine(", ")}";
            sb.AppendLine(line);
        }
        ConsoleHelper.DisplayColumn(sb.ToString());
    }

    public void ResetWrongPairs()
    {
        WrongPairs = new Dictionary<string[], string[]>();
    }

    public Dictionary<string[], string[]> ShufflePairs(Dictionary<string[], string[]> pairs)
    {
        Random rand = new Random();
        return pairs.OrderBy(x => rand.Next())
            .ToDictionary(item => item.Key, item => item.Value);
    }

    public void ShufflePairs()
    {
        Random rand = new Random();
        Pairs = Pairs.OrderBy(x => rand.Next())
            .ToDictionary(item => item.Key, item => item.Value);
    }

    public PairsParser GetCurrentPairs()
    {
        switch(Config.FileExtension)
        {
            case FileExtension.Words:
                return new WordsParser(Config);
            case FileExtension.IrregularVerbs:
                return new VerbsParser(Config);
            default:
                Console.WriteLine("File Extension not found, setting default ( Words )");
                return new WordsParser(Config);
        }
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

