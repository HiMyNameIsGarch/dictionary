using System.Text.RegularExpressions;

public class ConfigOptions
{
    public bool OutputHasColors { get; set; }

    public bool AskMeSynonyms { get; set; } // on words type

    public bool ReverseWords { get; set; } // on words type

    public bool DisplayOneRandomSynonym { get; set; }

    public bool Over80IamCorrect { get; set; }

    public bool DisplayAvarageStatistics { get; set; } // Only on irregular verbs

    public bool DisplayFinalStatistics { get; set; }

    public bool DisplayOnPairStatistics { get; set; }

    public LayoutType Layout { get; set; }

    public ModeType Mode { get; set; }

    public FileExtension FileExtension { get; private set; }

    private string _currentFile = "default.words.txt";

    public string CurrentFile { 
        get { return _currentFile.EndsWith(".txt") ? _currentFile : _currentFile + ".txt"; }
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
            Console.WriteLine($"Cannot find an extension for '{fileName}' file, setting the default one, <words>\n");
        }
    }
}
