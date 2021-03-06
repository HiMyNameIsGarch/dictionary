using System.Diagnostics;
using static System.Console;
using static ConsoleHelper;

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
            case "select": return Select(config, args);
            case "start":  return Start(config, args);
            case "add":    return Add();
            case "delete": return Delete();
            case "edit":   return Edit(args);
            case "status": return Status();
            case "list":   return ShowOptionList();
            case "help":   HelpMenu(); return 0;
            default: 
                Console.WriteLine($"Sorry but '{args[0]}' is an invalid option, check this help menu:\n");
                HelpMenu();
                return 1;
        }
    }

    private static int ShowOptionList()
    {
        var options = CurrentOS.GetAndDisplayFilesFrom(new DirectoryInfo(
                    new WordsParser().BaseDirectory), false);
        if(options.Length > 0) return 0;

        ColorWriteLine("No files found!", ConsoleColor.Red);
        return 1;
    }

    private static int Select(ConfigOptions config, string[] args)
    {
        string opt = "";
        if(args.Length > 1) opt = args[1];
        if(!int.TryParse(opt, out int num)) num = -1;

        string wordsFile = CurrentOS.GetFileName(
                new WordsParser().BaseDirectory, 
                "txt", num);
        if(string.IsNullOrEmpty(wordsFile)) return 1;

        config.CurrentFile = wordsFile;
        config.SetFileExtension(wordsFile);
        return Start(config, args);
    }

    private static int Start(ConfigOptions config, string[] args)
    {
        // Initiate the data
        SessionData data = new SessionData(config);
        IMode mode = data.GetCurrentMode();
        ISession mainSession = data.GetCurrentSession();

        mode.Start(mainSession);
        return 0;
    }

    private static string GetExtension()
    {
        var types = CurrentOS.GetEnumList<FileExtension>();
        int i = 0;
        foreach(var type in types)
        {
            Console.WriteLine(i + " -> " + type.ToFormattedString());
            i++;
        }
        string? key = "";
        FileExtension extension = FileExtension.Words;
        do
        {
            Write("Enter number of extension: ");
            key = ReadLine();
            if(!Enum.TryParse<FileExtension>(key, true, out extension))
            {
                key = "";
                continue;
            }
            int numExtension = (int)extension;
            if(numExtension > i - 1 || numExtension < 0)
            {
                key = "";
                continue;
            }
        }
        while(string.IsNullOrWhiteSpace(key));

        return extension.ToString().ToLower();
    }

    private static string GetFullFilePath(string extension)
    {
        string baseDirectory = new WordsParser().BaseDirectory;
        string? fileName = "";
        do
        {
            Write("Enter name of file: ");
            fileName = ReadLine();
            string fullName = fileName + "." + extension + ".txt";
            string fullPath = Path.Join(baseDirectory, fullName);
            if(File.Exists(fullPath))
            {
                fileName = "";
                ColorWriteLine($"File name {fullName} already exists!", ConsoleColor.Red);
                continue;
            }
            fileName = fullPath;
        }
        while(string.IsNullOrWhiteSpace(fileName));
        return fileName;
    }

    private static int Add()
    {
        string extension = GetExtension();

        string fullFileName = GetFullFilePath(extension);

        OpenFile(fullFileName);

        return 0;
    }

    private static int Delete()
    {
        string baseDirectory = new WordsParser().BaseDirectory;
        string fileName = CurrentOS.GetFileName(baseDirectory, "txt") + ".txt";
        string fullPath = Path.Join(baseDirectory, fileName);

        WriteLine("Deleting: {0}", fileName);
        Write("Are you sure? [y/n] ");
        char key = Console.ReadKey().KeyChar;
        Write("\n");
        
        if(key != 'y') return 1;

        if(!DeleteFile(fullPath))
        {
            ColorWriteLine($"Cannot delete: {fullPath}", ConsoleColor.Red);
            return 1;
        }
        ColorWriteLine("File was deleted succesfully", ConsoleColor.Green);
        return 0;
    }

    private static bool DeleteFile(string fullPath)
    {
        if(!File.Exists(fullPath))
        {
            ColorWriteLine($"File: {fullPath} does not exists", ConsoleColor.Red);
            return false;
        }
        try
        {
            File.Delete(fullPath);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        return true;
    }

    private static int Edit(string[] args)
    {
        if(args.Length < 2)
        {
            Console.WriteLine("No argument provided, please choose between 'config' or 'words'!");
            return 1;
        }
        switch(args[1])
        {
            case "config":
                var cParser = new ConfigParser();
                OpenFile(cParser.FilePath);
                break;
            case "words":
                var wordsDir = new WordsParser().BaseDirectory;
                string words = CurrentOS.GetFullFilePath(wordsDir, "txt");
                if(!string.IsNullOrEmpty(words)) OpenFile(words);
                break;
            default:
                Console.WriteLine($"Invalid argument {args[1]}");
                HelpMenu();
                return 1;
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
        var stats = new Statistics();
        stats.ShowOptions();
        return 1;
    }

    private static void HelpMenu()
    {
        WriteLine("dictionary - is a simple program to help you get better at words.\n");
        WriteLine("Usage: dictionary <options>\n");
        WriteLine("Options:");
        WriteLine("start   - Starts a session with the current file in config.");
        WriteLine("edit    - Opens an editor to edit either your config or words file.");
        WriteLine("              Example: 'dictionary edit config' - To edit your config file.");
        WriteLine("                   Or: 'dictionary edit words'  - To edit one of your words file.");
        WriteLine("select  - Select the current words file and start a session with it.");
        WriteLine("list    - Lists all the available files and their number.");
        WriteLine("status  - Display statistics ( point / word accuracy and response time ) about your sessions.");
        WriteLine("add     - Adds a file.");
        WriteLine("delete  - Deletes a file.");
        WriteLine("help    - Displays this help menu.");
    }
}
