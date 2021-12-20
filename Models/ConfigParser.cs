using Newtonsoft.Json;

public class ConfigParser: FileParser<Config>
{

    public static RuntimeDirectory baseDirs = new RuntimeDirectory(".config/dictionary", "");

    public ConfigParser() : base(baseDirs, "config.json") {}

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
