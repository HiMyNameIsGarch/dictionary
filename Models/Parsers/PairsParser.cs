using System.Text.RegularExpressions;

public abstract class PairsParser : FileParser<Dictionary<string[], string[]>>
{
    public static string _directory = CurrentOS.GetDirectoryPath(".local/share/dictionary","data");

    private string _matchPairRegex = ".*";

    protected ConfigOptions Config;

    protected PairsParser() : base(_directory) 
    { Config = new ConfigOptions(); }

    protected PairsParser(ConfigOptions config, string pairsRegex) 
        : base(_directory, config.CurrentFile) 
    { 
        Config = config;
        _matchPairRegex = pairsRegex;
    }

    protected virtual Tuple<string[], string[]> GetPairs(string line) 
    {
        string[] pairs = line.Split('|');
        string[] keys = pairs[0].Split(',').RemoveWhiteSpace();
        string[] values = pairs[1].Split(',').RemoveWhiteSpace();

        return new Tuple<string[], string[]>(keys, values);
    }

    protected virtual bool IsInvalidLine(string line)
    {
        return string.IsNullOrWhiteSpace(line) || 
            !Regex.Match(line, _matchPairRegex).Success;
    }
}
