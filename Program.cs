using static ConsoleHelper;

public class Program
{
    private const string _errorMessage = "> Ooops, what a shame, I broke, and now you can't learn properly, here is the error message: ";

    public static int Main(string[] args)
    {
        try
        {
            return Sessionizer.Initiate(args);
        }
        catch(Exception ex)
        {
            ColorWriteLine(_errorMessage, ConsoleColor.Red);
            ColorWriteLine("> " + ex.Message, ConsoleColor.Magenta);
            return 1;
        }
    }
}
