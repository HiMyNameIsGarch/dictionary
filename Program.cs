public class Program
{
    public static int Main(string[] args)
    {
        if(args.Length == 0) 
        {
            Console.WriteLine("Ooops, you didn't specified any argument...\n");
            HelpMenu();
            return 1;
        }
        SessionData? data = null;
        try
        {
            data = GetInitialData(args);
        }
        catch(Exception ex)
        {
            // Temp messages
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            return 1;
        }
        // Start the session
        ISession mainSession = data.GetCurrentSession();

        switch(args[0]) {
            case "select":
            case "start": 
                mainSession.DisplayBeforeSession();
                mainSession.Start();
                mainSession.DisplayAfterSession();
                break;
            case "edit":
                break;
            case "status": 
                mainSession.DisplayStatusFor("");
                break;
            case "help": 
                HelpMenu();
                break;
            default: 
                Console.WriteLine($"Sorry but '{args[0]}' is an invalid option, check this help menu:\n");
                HelpMenu();
                break;
        }
        return 0;
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

    private static string SelectCustomFile()
    {
        string path = WordsParser.baseDirs.GetOSPath();
        DirectoryInfo d = new DirectoryInfo(path);
        int i = 0;
        var allFiles = d.GetFiles("*.txt");
        foreach(var file in allFiles) {
            i++;
            Console.WriteLine($"{i} -> {file.Name}");
        }
        string filePath = "";
        do 
        {
            Console.WriteLine("Enter the number of your file: ");
            char key = Console.ReadKey(true).KeyChar;
            if(!Char.IsNumber(key)) continue;
            int value = key - '0';
            if(value > i || value == 0) continue;
            filePath = allFiles[value - 1].FullName;
        }
        while(string.IsNullOrEmpty(filePath));

        return Path.GetFileNameWithoutExtension(filePath);
    }

    private static SessionData GetInitialData(string[] args)
    {
        // Parse config file
        ConfigParser cParser = new ConfigParser();
        Config config = cParser.ParseFile();
        string wordsFile = 
            args[0] == "select" ? SelectCustomFile() : config.CurrentFile;
        config.CurrentFile = wordsFile;
        config.SetFileExtension(wordsFile);
        // Parse words file
        WordsParser wParser = new WordsParser($"{config.CurrentFile}.txt");
        var words = wParser.ParseFile();
        // Return the values
        return new SessionData(config, words);
    }
}
