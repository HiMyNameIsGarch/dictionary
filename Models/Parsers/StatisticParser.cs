using Newtonsoft.Json;

public class StatisticParser : FileParser<StatisticModel>
{
    public StatisticParser(string fileName):
        base(".cache/dictionary","statistics", fileName) { }
    public StatisticParser():
        base(".cache/dictionary","statistics") { }

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
        var files = di.GetFiles("*.json");
        ICollection<StatisticModel> stats = new List<StatisticModel>();
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
