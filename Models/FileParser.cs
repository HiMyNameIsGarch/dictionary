using System.Runtime.InteropServices; 

public abstract class FileParser<T>: IParser<T>
{

    public FileParser(RuntimeDirectory baseDirectoryNames, string fileName) 
    {
        BaseDirectory = baseDirectoryNames.GetOSPath();

        if(string.IsNullOrEmpty(BaseDirectory))
        {
            Console.WriteLine($"Platform not supported! Platform: {RuntimeInformation.OSDescription}");
            Environment.Exit(1);
        }

        FilePath = $"{BaseDirectory}/{fileName}";

        if(!Directory.Exists(BaseDirectory)) 
        {
            Directory.CreateDirectory(BaseDirectory);
        }

        if(!File.Exists(FilePath)) 
        {
            System.IO.File.WriteAllText(FilePath, DefaultFileText());
        }
        
        // Read the text from the file
        FileText = File.ReadAllText(FilePath);
    }
    public string FileText { get; }
    public string FilePath { get; }
    public string BaseDirectory { get; }
    public abstract T ParseFile();
    protected abstract string DefaultFileText();
}
