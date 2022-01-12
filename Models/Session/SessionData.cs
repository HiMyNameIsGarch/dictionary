using System.Text;

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

