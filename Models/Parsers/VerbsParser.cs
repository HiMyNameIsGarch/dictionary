using System.Text;
using System.Text.RegularExpressions;

public class VerbsParser : PairsParser
{
    private readonly string MatchPairRegex = @"^\w+.*\|\s?\w+\s?\,\s?\w+\s?\,\s?\w+";

    public VerbsParser(ConfigOptions config): base(config) { }

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

    private Tuple<string[], string[]> GetPairs(string line) 
    {
        string[] pairs = line.Split('|');
        string[] keys = pairs[0].Split(',').RemoveWhiteSpace();
        string[] values = pairs[1].Split(',').RemoveWhiteSpace();

        return new Tuple<string[], string[]>(keys, values);
    }

    private bool IsInvalidLine(string line)
    {
        return string.IsNullOrWhiteSpace(line) || !Regex.Match(line, MatchPairRegex).Success;
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
