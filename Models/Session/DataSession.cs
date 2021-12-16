public class DataSession 
{
    public DataSession(Config config, Dictionary<string[], string[]> pairs) 
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
}

