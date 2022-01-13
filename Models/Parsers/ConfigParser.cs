using Newtonsoft.Json;

public class ConfigParser: FileParser<ConfigOptions>
{

    public ConfigParser() : base(".config/dictionary", "config", "config.json") {}

    public override ConfigOptions ParseFile()
    {
        ConfigOptions? conf = JsonConvert.DeserializeObject<ConfigOptions>(FileText);
        if(conf == null) {
            return new ConfigOptions();
        }
        return conf;
    }
    protected override string DefaultFileText()
    {
        return JsonConvert.SerializeObject(new ConfigOptions());
    }
}
