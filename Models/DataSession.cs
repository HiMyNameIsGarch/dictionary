public class DataSession 
{
    public DataSession(Config config, Dictionary<string[], string[]> pairs) 
    {
        Config = config;
        Pairs = pairs;
    }

    public Config Config { get; }
    public Dictionary<string[], string[]> Pairs { get; }
}

