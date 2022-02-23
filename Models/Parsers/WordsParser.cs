using System.Text;

public class WordsParser: PairsParser
{
    private const string MatchPairRegex = @"^\w+.*\|\s?\w+.*";

    public WordsParser(ConfigOptions config): base(config, MatchPairRegex) { }

    public override Dictionary<string[], string[]> ParseFile()
    {
        Dictionary<string[], string[]> PairsData = new Dictionary<string[], string[]>();
        using (StringReader reader = new StringReader(FileText))
        {
            string? line = null;
            while ((line = reader.ReadLine()) != null)
            {
                if(IsInvalidLine(line)) continue;
                var pairs = GetPairs(line);

                if(Config.ReverseWords)
                    PairsData.Add(pairs.Item2, pairs.Item1);
                else
                    PairsData.Add(pairs.Item1, pairs.Item2);
            }
        }
        return PairsData;
    }

    protected override string DefaultFileText()
    {
        Console.WriteLine("Creating the default text for: '{0}'", FilePath);
        Console.WriteLine("Consider editing it before starting the session");
        StringBuilder sb = new StringBuilder();
        for(int i = 1; i < 10; i++)
        {
            sb.AppendLine($"word{i}, word{i+1} | meaning{i}, meaning{i+1}");
        }
        return sb.ToString();
    }
}
