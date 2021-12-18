using System.Text.RegularExpressions;

public class Config
{
    public bool HasColors { get; set; }

    public FileType FileType { get; private set; }

    private string _currentFile = "default.words";

    public string CurrentFile { 
        get { return _currentFile; }
        set { if(!string.IsNullOrEmpty(value)) _currentFile = value; }
    }

    public void SetFileType(string fileName)
    {
        fileName = fileName + ".txt";
        string extension = Regex.Match(fileName, @"(?<=\w+\.)\w+(?=\.txt)").Value;
        FileType newExtension = FileType.Words;
        if(Enum.TryParse<FileType>(extension, true, out newExtension))
        {
            FileType = newExtension;
        } 
        else 
        {
            Console.WriteLine($"Cannot find an extension from '{fileName}' file, setting the default one, <words>\n");
        }
    }
}
