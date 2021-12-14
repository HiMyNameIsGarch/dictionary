public class Program
{
    public static void Main(string[] args)
    {
        if(args.Length == 0) 
        {
            Console.WriteLine("Ooops, you didn't specified any argument...\n");
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
                Console.WriteLine($"Sorry but '{args[0]}' is an invalid option, check this help menu:\n");
                HelpMenu();
                break;
        }
    }

    private static void HelpMenu() 
    {
        Console.WriteLine("dictionary - is a simple program to help you get better with words.\n");
        Console.WriteLine("Usage: dictionary <options>\n");
        Console.WriteLine("Options:");
        Console.WriteLine("start - Starts a session with the default configuration.");
        Console.WriteLine("edit  - Opens an editor to edit either your config or words file.");
        Console.WriteLine("             Example: 'dictionary edit config'.");
        Console.WriteLine("select - Select the current words file.");
        Console.WriteLine("status - Display status information about your sessions.");
        Console.WriteLine("help   - Displays this help menu.");
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
