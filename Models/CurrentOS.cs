using System.Runtime.InteropServices; 
using System.Text;

public static class CurrentOS
{

    public static bool IsPlatformSupported() 
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || 
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }

    public static string GetSeparator()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) 
            return "/";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "\\";
        else 
            return "/";
    }

    public static string GetEditor()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
            string? editor = System.Environment.GetEnvironmentVariable("EDITOR");
            if(editor != null) 
                return editor;
            else 
                return "nano";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "notepad";
        else 
            return "";
    }

    public static string GetDirectoryPath(string linux, string windows)
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) 
            return $"/home/{Environment.UserName}/{linux}";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return $"C:\\Users\\{Environment.UserName}\\AppData\\Local\\dictionary\\{windows}";
        else 
            return "";
    }

    public static string GetFileName(string path, string extension)
    {
        return System.IO.Path.GetFileNameWithoutExtension(GetFullFilePath(path, extension));
    }

    public static string GetFullFilePath(string path, string extension)
    {
        int currentCursor = Console.CursorTop;
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        var fileNames = dirInfo.GetFiles("*." + extension);
        var fileName = OrderAndDisplayFiles(fileNames.Select(s => s.Name).ToArray());

        if(fileNames.Length == 1){
            Console.WriteLine("Found just one file, selecting it...");
            return fileNames[0].FullName;
        }
        
        string? filePath = "";
        do 
        {
            Console.Write("Number of your file: ");
            string? keys = "1";//Console.ReadLine();
            if(!int.TryParse(keys, out int value)) continue;
            if(value > fileNames.Length || value == 0) continue;
            filePath = fileNames.Where(s => s.Name == fileName[value]).FirstOrDefault()?.Name;
        }
        while(string.IsNullOrEmpty(filePath));

        ConsoleHelper.ClearScreen(currentCursor);

        return filePath;
    }

    private static Tuple<IEnumerable<string>, IEnumerable<string>> OrderFiles(string[] files)
    {
        ICollection<string> words = new List<string>();
        ICollection<string> irregularVerbs = new List<string>();
        foreach(var file in files)
        {
            if(file.Contains(".irregularverbs.txt"))
            {
                irregularVerbs.Add(file.Split('.')[0]);
            }
            else if(file.Contains(".words.txt"))
            {
                words.Add(file.Split('.')[0]);
            }
        }
        return new Tuple<IEnumerable<string>, IEnumerable<string>>(words, irregularVerbs);
    }

    private static string[] OrderAndDisplayFiles(string[] files)
    {
        var names = OrderFiles(files);
        var words = names.Item1;
        var verbs = names.Item2;
        string[] finalList = new string[words.Count() + verbs.Count() + 2];
        int i = 1; 
        Console.WriteLine("Irregular Verbs:");
        foreach(var verb in verbs)
        {
            finalList[i - 1] = verb;
            DisplayFileWithColor(i, verb);
            i++;
        }
        Console.WriteLine("\nWords:");
        foreach(var word in words)
        {
            finalList[i - 1] = word;
            DisplayFileWithColor(i, word);
            i++;
        }
        return finalList;
    }

    private static void DisplayFileWithColor(int num, string name)
    {
        ConsoleHelper.ColorWrite(num.ToString(), ConsoleColor.Yellow);
        ConsoleHelper.ColorWrite(" -> ", ConsoleColor.DarkYellow);
        ConsoleHelper.ColorWrite(name + "\n", ConsoleColor.Magenta);
    }
}
