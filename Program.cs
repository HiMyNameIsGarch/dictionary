using System.Diagnostics;
using System.Runtime.InteropServices; 


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
                if(args.Length < 2)
                {
                    Console.WriteLine("No argument provided, please choose between 'config' or 'words'!");
                    return 1;
                } 
                else 
                {
                    switch(args[1])
                    {
                        case "config":
                            string config = ConfigParser.baseDirs.GetFullFilePath("json");
                            OpenFile(config);
                            break;
                        case "words":
                            string words = WordsParser.baseDirs.GetFullFilePath("txt");
                            OpenFile(words);
                            break;
                        default:
                            Console.WriteLine($"Invalid argument {args[1]}");
                            HelpMenu();
                            return 1;
                    }
                }
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

    private static void OpenFile(string filePath)
    {
        Process process = new Process();
        string defaultProgram = "";
        
        // Configure the process
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) 
            defaultProgram = "nvim";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
            defaultProgram = "notepad";

        process.StartInfo.FileName = defaultProgram;
        process.StartInfo.Arguments = filePath;
        process.Start();
        process.WaitForExit();// Waits here for the process to exit.
        Console.WriteLine($"Process exited with status code: {process.ExitCode}");
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

    private static SessionData GetInitialData(string[] args)
    {
        // Parse config file
        ConfigParser cParser = new ConfigParser();
        Config config = cParser.ParseFile();
        string wordsFile = 
            args[0] == "select" ? 
                WordsParser.baseDirs.GetFileName("txt") :
                config.CurrentFile;
        config.CurrentFile = wordsFile;
        config.SetFileExtension(wordsFile);
        // Parse words file
        WordsParser wParser = new WordsParser($"{config.CurrentFile}.txt");
        var words = wParser.ParseFile();
        // Return the values
        return new SessionData(config, words);
    }
}
