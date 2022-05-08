using System.Runtime.InteropServices; 

public static class CurrentOS
{

    public static bool IsPlatformSupported() 
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || 
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
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

    public static string GetFileName(string path, string extension, int opt = -1)
    {
        return System.IO.Path.GetFileNameWithoutExtension(GetFullFilePath(path, extension, opt));
    }

    public static string GetFullFilePath(string path, string extension, int opt = -1)
    {
        int currentCursor = Console.CursorTop;
        var allFiles = GetAndDisplayFilesFrom(new DirectoryInfo(path), opt > 0);

        if(allFiles.Length == 1)
        {
            Console.WriteLine("Found just one file, selecting it...");
            return allFiles[0];
        }
        if(allFiles.Length < 1)
        {
            Console.WriteLine("No files found!");
            return "";
        }
        if(opt > allFiles.Length)
        {
            Console.WriteLine("Invalid option: {0}!", opt);
            return "";
        }
        opt--;
        if(opt > -1 && opt < allFiles.Length) return allFiles[opt];

        string filePath = "";
        do 
        {
            Console.Write("Enter the number of your file: ");
            string? keys = Console.ReadLine();
            if(!int.TryParse(keys, out int value)) continue;
            if(value > allFiles.Length || value <= 0) continue;
            filePath = allFiles[value - 1];
        }
        while(string.IsNullOrEmpty(filePath));

        ConsoleHelper.ClearScreen(currentCursor);

        return filePath;
    }

    public static string[] GetAndDisplayFilesFrom(DirectoryInfo di, bool silent)
    {
        var types = GetEnumList<FileExtension>();
        ICollection<string> lFiles = new List<string>();
        int idx = 1;
        foreach(var type in types)
        {
            string pattern = "*." + type.ToString().ToLower() + ".txt";
            var files = di.GetFiles(pattern).OrderBy(s => s.Name).OrderBy(s => s.Name.Length).ToArray();
            // Display header
            if(!silent) ConsoleHelper.ColorWriteLine(type.ToFormattedString(), ConsoleColor.Blue);
            // Display the list of the files
            foreach(var file in files)
            {
                if(!silent) DisplayFile(file.Name, idx);
                lFiles.Add(file.FullName);
                idx++;
            }
        }
        return lFiles.ToArray();
    }

    private static void DisplayFile(string fileName, int num)
    {
        ConsoleHelper.ColorWrite(num.ToString(), ConsoleColor.Yellow);
        ConsoleHelper.ColorWrite(" -> ", ConsoleColor.DarkYellow);
        fileName = fileName.Split('.')[0]; // remove the extension of the file
        ConsoleHelper.ColorWrite(fileName + "\n", ConsoleColor.Magenta);
    }

    public static List<T> GetEnumList<T>()
    {
        T[] array = (T[])Enum.GetValues(typeof(T));
        List<T> list = new List<T>(array);
        return list;
    }

}
