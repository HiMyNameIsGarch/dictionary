using System.Diagnostics;
using static System.Console;

public static class Sessionizer
{
    public static int Initiate(string[] args)
    {
        if(args.Length == 0)
        {
            WriteLine("Ooops, you didn't specified any argument...\n");
            HelpMenu();
            return 1;
        }
        return HandleArgs(args);
    }

    private static ConfigOptions ParseConfig()
    {
        ConfigParser cParser = new ConfigParser();
        ConfigOptions config = cParser.ParseFile();
        return config;
    }

    private static int HandleArgs(string [] args)
    {
        var config = ParseConfig();

        switch(args[0]) {
            case "select":
                string wordsFile = CurrentOS.GetFileName(new WordsParser(config.CurrentFile).BaseDirectory,"txt");
                config.CurrentFile = wordsFile;
                config.SetFileExtension(wordsFile);
                return Start(config);
            case "start": 
                return Start(config);
            case "edit":
                return Edit(args);
            case "status": 
                return Status();
            case "help": 
                HelpMenu();
                return 0;
            default: 
                Console.WriteLine($"Sorry but '{args[0]}' is an invalid option, check this help menu:\n");
                HelpMenu();
                return 1;
        }
    }

    private static int Start(ConfigOptions config)
    {
        // Get all words
        WordsParser wParser = new WordsParser($"{config.CurrentFile}.txt");
        var words = wParser.ParseFile();

        // Initiate the data
        SessionData data = new SessionData(config, words);
        IMode mode = data.GetCurrentMode();
        ISession mainSession = data.GetCurrentSession();

        mode.Start(mainSession);
        return 0;
    }
    private static int Edit(string[] args)
    {
        if(args.Length < 2)
        {
            Console.WriteLine("No argument provided, please choose between 'config' or 'words'!");
            return 1;
        } 
        else 
        {
            string baseDirectory = "";
            string extension = "";
            switch(args[1])
            {
                case "config":
                    baseDirectory = new ConfigParser().BaseDirectory;
                    extension = "json";
                    break;
                case "words":
                    baseDirectory = new WordsParser().BaseDirectory;
                    extension = "txt";
                    break;
                default:
                    Console.WriteLine($"Invalid argument {args[1]}");
                    HelpMenu();
                    return 1;
            }
            string words = CurrentOS.GetFullFilePath(baseDirectory, extension);
            OpenFile(words);

        }
        return 0;
    }
    private static void OpenFile(string filePath)
    {
        Process process = new Process();
        // Configure the process
        process.StartInfo.FileName = CurrentOS.GetEditor();
        process.StartInfo.Arguments = filePath;
        process.Start();
        process.WaitForExit();// Waits here for the process to exit.
        Console.WriteLine($"Process exited with status code: {process.ExitCode}");
    }
    private static int Status()
    {
        return 1;
    }

    private static void HelpMenu()
    {
        WriteLine("dictionary - is a simple program to help you get better with words.\n");
        WriteLine("Usage: dictionary <options>\n");
        WriteLine("Options:");
        WriteLine("start - Starts a session with the default configuration.");
        WriteLine("edit  - Opens an editor to edit either your config or words file.");
        WriteLine("             Example: 'dictionary edit config'.");
        WriteLine("select - Select the current words file.");
        WriteLine("status - Display status information about your sessions.");
        WriteLine("help   - Displays this help menu.");
    }
}