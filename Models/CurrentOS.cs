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
            return $"C:\\Users\\{Environment.UserName}\\{windows}";
        else 
            return "";

    }

    public static string GetFileName(string path, string extension)
    {
        return System.IO.Path.GetFileNameWithoutExtension(GetFullFilePath(path, extension));
    }

    public static string GetFullFilePath(string path, string extension)
    {
        DirectoryInfo d = new DirectoryInfo(path);
        int i = 0;
        var allFiles = d.GetFiles("*." + extension);
        foreach(var file in allFiles) {
            i++;
            Console.WriteLine($"{i} -> {file.Name}");
        }

        if(allFiles.Length == 1){
            Console.WriteLine("Found just one file, selecting it...");
            return allFiles[0].FullName;
        }
        
        string filePath = "";
        do 
        {
            Console.Write("Enter the number of your file: ");
            char key = Console.ReadKey(true).KeyChar;
            if(!Char.IsNumber(key)) continue;
            int value = key - '0';
            if(value > i || value == 0) continue;
            filePath = allFiles[value - 1].FullName;
        }
        while(string.IsNullOrEmpty(filePath));

        return filePath;
    }

}
