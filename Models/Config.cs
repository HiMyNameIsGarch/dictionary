using System.Text.RegularExpressions;

public class Config
{
    public bool HasColors { get; set; }

    public bool DisplayFinalStats { get; set; }

    public bool DisplayOnPairStats { get; set; }

    public FileExtension FileExtension { get; private set; }

    private string _currentFile = "default.words";

    public string CurrentFile { 
        get { return _currentFile; }
        set { if(!string.IsNullOrEmpty(value)) _currentFile = value; }
    }

    public void SetFileExtension(string fileName)
    {
        fileName = fileName + ".txt";
        string extension = Regex.Match(fileName, @"(?<=\w+\.)\w+(?=\.txt)").Value;
        FileExtension newExtension = FileExtension.Words;
        if(Enum.TryParse<FileExtension>(extension, true, out newExtension))
        {
            FileExtension = newExtension;
        } 
        else 
        {
            Console.WriteLine($"Cannot find an extension from '{fileName}' file, setting the default one, <words>\n");
        }
    }
}
