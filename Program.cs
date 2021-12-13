public class Program
{
    public static void Main(string[] args)
    {
        if(args.Length == 0) 
        {
            Console.WriteLine("Ooops, you didn't specified any argument...");
            HelpMenu();
            return;
        }
        DataSession? data = null;
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
        Session mainSession = new Session(data);
        
        // Start the session
        switch(args[0]) {
            case "start": 
                mainSession.Start();
                break;
            case "edit": 
                break;
            case "select": 
                break;
            case "status": 
                break;
            case "help": 
                HelpMenu();
                break;
            default: 
                Console.WriteLine($"Sorry but {args[0]} is an invalid option");
                break;
        }
    }

    private static void HelpMenu() 
    {
        Console.WriteLine("this is a help menu");
    }

    private static DataSession GetInitialData()
    {
        // Parse config file
        ConfigParser cParser = new ConfigParser();
        Config config = cParser.ParseFile();
        // Parse words file based on config
        WordsParser wParser = new WordsParser($"{config.CurrentFile}.txt");
        var words = wParser.ParseFile();
        // Return the values
        return new DataSession(config, words);
    }
}
