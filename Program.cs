public class Program
{
    public static void Main(string[] args)
    {
        Tuple<Config, string>? data = null;
        try
        {
            data = GetInitialData();
        }
        catch(Exception ex)
        {
            // Temp messages
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            return;
        }
        
        // Start the session
        switch(args[0]) {
            case "start": 
                break;
            case "edit": 
                break;
            case "select": 
                break;
            case "status": 
                break;
            default: 
                Console.WriteLine($"Sorry but {args[0]} is an invalid option");
                break;
        }
    }

    private static Tuple<Config, string> GetInitialData()
    {
        // Parse config file
        ConfigParser cParser = new ConfigParser();
        Config config = cParser.ParseFile();
        // Parse words file based on config
        WordsParser wParser = new WordsParser($"{config.CurrentFile}.txt");
        string words = wParser.ParseFile();
        // Return the values
        return new Tuple<Config, string>(config, words);
    }
}
