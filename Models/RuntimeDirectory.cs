using System.Runtime.InteropServices; 

public class RuntimeDirectory
{
    public RuntimeDirectory(string linux, string windows) 
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) 
            Path = $"/home/{Environment.UserName}/{linux}";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
            Path = $"C:\\Users\\{Environment.UserName}\\{windows}";
        else 
            Path = "";
    }

    public string Path { get; }

    public string GetFileName(string extension)
    {
        return System.IO.Path.GetFileNameWithoutExtension(GetFileName(extension));
    }

    public string GetFullFilePath(string extension)
    {
        DirectoryInfo d = new DirectoryInfo(Path);
        int i = 0;
        var allFiles = d.GetFiles("*." + extension);
        foreach(var file in allFiles) {
            i++;
            Console.WriteLine($"{i} -> {file.Name}");
        }

        if(allFiles.Length == 1){
            Console.WriteLine("Found just one file, selecting it...");
            return allFiles[0].FullName;//GetFileNameWithoutExtension(allFiles[0].FullName);
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
