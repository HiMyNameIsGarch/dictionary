public abstract class FileParser<T>
{

    public string BaseDirectory { get; }
    protected string FileText { get; }
    public string FilePath { get; }
    public abstract T ParseFile();
    protected abstract string DefaultFileText();

    public FileParser(string directory)
    {
        BaseDirectory = SetBaseDirectory(directory);
        FilePath = "";
        FileText = "";
    }

    public FileParser(string directory, string fileName) 
    {
        BaseDirectory = SetBaseDirectory(directory);

        FilePath = $"{BaseDirectory}{CurrentOS.GetSeparator()}{fileName}";

        if(!Directory.Exists(BaseDirectory)) 
            Directory.CreateDirectory(BaseDirectory);

        if(!File.Exists(FilePath)) 
            System.IO.File.WriteAllText(FilePath, DefaultFileText());
        
        FileText = File.ReadAllText(FilePath);
    }

    private string SetBaseDirectory(string dir)
    {
        if(string.IsNullOrEmpty(dir))
        {
            Console.WriteLine($"Directory is empty, exiting...");
            Environment.Exit(1);
        }
        return dir;
    }

}
