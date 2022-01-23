using Newtonsoft.Json;

public class ConfigParser: FileParser<ConfigOptions>
{

    public ConfigParser() : base(".config/dictionary", "config", "config.json") {}

    public override ConfigOptions ParseFile()
    {
        ConfigOptions? conf = null;
        try
        {
            conf = JsonConvert.DeserializeObject<ConfigOptions>(FileText);
        }
        catch(Exception ex)
        {
            Console.WriteLine("Cannot parse the config file.");
            Console.WriteLine("Err: {0}", ex.Message);
            Environment.Exit(1);
        }
        if(conf == null) {
            return new ConfigOptions();
        }
        return conf;
    }
    protected override string DefaultFileText()
    {
        return JsonConvert.SerializeObject(new ConfigOptions(), Formatting.Indented);
    }
}
