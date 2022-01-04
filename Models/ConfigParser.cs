using Newtonsoft.Json;

public class ConfigParser: FileParser<Config>
{

    public ConfigParser() : base(".config/dictionary", "config", "config.json") {}

    public override Config ParseFile()
    {
        Config? conf = JsonConvert.DeserializeObject<Config>(FileText);
        if(conf == null) {
            return new Config();
        }
        return conf;
    }
    protected override string DefaultFileText()
    {
        return JsonConvert.SerializeObject(new Config());
    }
}
