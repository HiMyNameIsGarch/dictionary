using System.Runtime.InteropServices; 

public abstract class FileParser<T>: IParser<T>
{

    public FileParser(string linuxDir, string windowsDir, string fileName) 
    {

        BaseDirectory = CurrentOS.GetDirectoryPath(linuxDir, windowsDir);

        if(string.IsNullOrEmpty(BaseDirectory))
        {
            Console.WriteLine($"Platform not supported! Platform: {RuntimeInformation.OSDescription}");
            Environment.Exit(1);
        }

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

    public string FileText { get; }
    public string FilePath { get; }
    public string BaseDirectory { get; }
    public abstract T ParseFile();
    protected abstract string DefaultFileText();
}
