using System.Runtime.InteropServices; 

public class RuntimeDirectory
{
    public RuntimeDirectory(string linux, string windows) 
    {
        Linux = linux;
        Windows = windows;
    }
    private string Linux { get; }
    private string Windows { get; }

    public string GetOSPath() 
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) 
        {
            return $"/home/{Environment.UserName}/{Linux}";
        } 
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
        {
            return $"C:\\Users\\{Environment.UserName}\\{Windows}";
        }
        else 
        {
            return "";
        }
    }
}
