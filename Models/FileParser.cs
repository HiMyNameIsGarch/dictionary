using System.Runtime.InteropServices; 

public record RuntimeDirectory(string Linux, string Windows);

public abstract class FileParser<T>: IParser<T>
{

    public FileParser(RuntimeDirectory baseDirectoryNames, string fileName) 
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) 
        {
            BaseDirectory = $"/home/{Environment.UserName}/{baseDirectoryNames.Linux}";
        } 
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
        {
            BaseDirectory= $"C:\\Users\\{Environment.UserName}\\{baseDirectoryNames.Windows}";
        }
        else 
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
