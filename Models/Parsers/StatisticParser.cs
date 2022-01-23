using Newtonsoft.Json;

public class StatisticParser : FileParser<StatisticModel>
{
    public StatisticParser(string fileName):
        base(".cache/dictionary","statistics", fileName) { }

    public void StoreModel(StatisticModel model)
    {
        string text = JsonConvert.SerializeObject(model, Formatting.Indented);
        System.IO.File.WriteAllText(FilePath, text);
    }
    protected override string DefaultFileText()
    {
        return "";
    }
    public override StatisticModel ParseFile()
    {
        StatisticModel? model = null;
        try
        {
            model = JsonConvert.DeserializeObject<StatisticModel>(FileText);
        }
        catch(Exception ex)
        {
            Console.WriteLine("Cannot parse the statistics file.");
            Console.WriteLine("Err: {0}", ex.Message);
            Environment.Exit(1);
        }
        if(model == null) {
            return new StatisticModel();
        }
        return model;
    }
}
