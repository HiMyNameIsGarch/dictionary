using System.Runtime.InteropServices; 

public abstract class FileParser<T>: IParser<T>
{

    public FileParser(string linuxDir, string windowsDir)
    {
        BaseDirectory = SetBaseDirectory(linuxDir, windowsDir);
        FilePath = "";
        FileText = "";
    }
    public FileParser(string linuxDir, string windowsDir, string fileName) 
    {
        BaseDirectory = SetBaseDirectory(linuxDir, windowsDir);

        FilePath = $"{BaseDirectory}{CurrentOS.GetSeparator()}{fileName}";

        if(!Directory.Exists(BaseDirectory)) 
        {
            Directory.CreateDirectory(BaseDirectory);
        }

        if(!File.Exists(FilePath)) 
        {
            Console.WriteLine($"File {fileName} doesn't exists, creating default at: {FilePath}");
            System.IO.File.WriteAllText(FilePath, DefaultFileText());
        }
        
        // Read the text from the file
        FileText = File.ReadAllText(FilePath);
    }

    private string SetBaseDirectory(string linuxDir, string windowsDir)
    {
        var dir = CurrentOS.GetDirectoryPath(linuxDir, windowsDir);
        if(string.IsNullOrEmpty(dir))
        {
            Console.WriteLine($"Platform not supported! Platform: {RuntimeInformation.OSDescription}");
            Environment.Exit(1);
        }
        return dir;
    }

    public string FileText { get; }
    public string FilePath { get; }
    public string BaseDirectory { get; }
    public abstract T ParseFile();
    protected abstract string DefaultFileText();
}
