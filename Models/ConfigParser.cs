using Newtonsoft.Json;

public class ConfigParser: FileParser<Config>
{

    public ConfigParser() : 
        base(new RuntimeDirectory(".config/dictionary", ""), "config.json") {}

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
        return "{ \n\t\"hasColors\": true \n}";
    }
}
