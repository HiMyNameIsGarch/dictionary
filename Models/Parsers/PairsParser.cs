public abstract class PairsParser : FileParser<Dictionary<string[], string[]>>
{
    public static string _directory = CurrentOS.GetDirectoryPath(".local/share/dictionary","data");

    protected PairsParser() : base(_directory) 
    { Config = new ConfigOptions(); }

    protected PairsParser(ConfigOptions config) 
        : base(_directory, config.CurrentFile) { Config = config; }

    protected ConfigOptions Config;
}
