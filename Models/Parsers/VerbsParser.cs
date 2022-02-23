using System.Text;

public class VerbsParser : PairsParser
{
    private const string MatchPairRegex = @"^\w+.*\|\s?\w+\s?\,\s?\w+\s?\,\s?\w+";

    public VerbsParser(ConfigOptions config): base(config, MatchPairRegex) { }

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
            sb.AppendLine($"verb{i} | first_form, second_form, third_form");
        }
        return sb.ToString();
    }
}
