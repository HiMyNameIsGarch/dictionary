using System.Text;

public class WordsParser: FileParser<string>
{

    public WordsParser(string defaultFile): 
        base(new RuntimeDirectory(".local/share/dictionary",""), defaultFile) { }

    public override string ParseFile()
    {
        return FileText;
    }

    protected override string DefaultFileText()
    {
        StringBuilder sb = new StringBuilder();
        for(int i = 1; i < 10; i++)
        {
            sb.AppendLine($"word{i}, word{i+1} | synonym{i}, synonym{i+1}");
        }
        return sb.ToString();
    }
}
