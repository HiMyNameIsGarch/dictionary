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
        return "word1, word2 | sinonym1\nword3, word4 | synonim2";
    }
}
