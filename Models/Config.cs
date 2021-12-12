public class Config
{
    public bool HasColors { get; set; }
    private string _currentFile = "default";
    public string CurrentFile { 
        get {
            return _currentFile;
        }
        set {
            if(!string.IsNullOrEmpty(value)) _currentFile = value;
        }
    }
}
