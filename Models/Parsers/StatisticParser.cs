using Newtonsoft.Json;

public class StatisticParser : FileParser<StatisticModel>
{
    private static string _directory = CurrentOS.GetDirectoryPath(".cache/dictionary","statistics");

    public StatisticParser(string fileName): base(_directory, fileName) { }

    public StatisticParser(): base(_directory) { }

    public void StoreModel(StatisticModel model)
    {
        string text = JsonConvert.SerializeObject(model, Formatting.Indented);
        System.IO.File.WriteAllText(FilePath, text);
    }
    protected override string DefaultFileText()
    {
        return "";
    }

    public ICollection<StatisticModel> ParseFiles()
    {
        var di = new DirectoryInfo(BaseDirectory);
        var files = di.GetFiles("*.json").OrderByDescending(s => s.Name).ToArray();
        ICollection<StatisticModel> stats = new List<StatisticModel>();
        if(files.Length < 1) return stats;
        foreach(var file in files)
        {
            var text = File.ReadAllText(file.FullName);
            var model = ParseText(text);
            if(model.FileName != "<empty>") stats.Add(model);
        }
        return stats;
    }

    private StatisticModel ParseText(string text)
    {
        StatisticModel? model = null;
        try
        {
            model = JsonConvert.DeserializeObject<StatisticModel>(text);
        }
        catch(Exception ex)
        {
            Console.WriteLine("Cannot parse the statistics file.");
            Console.WriteLine("Err: {0}", ex.Message);
            model = new StatisticModel();
            model.FileName = "<empty>";
        }
        if(model == null) {
            return new StatisticModel();
        }
        return model;
    }

    public override StatisticModel ParseFile()
    {
        return ParseText(FileText);
    }
}
