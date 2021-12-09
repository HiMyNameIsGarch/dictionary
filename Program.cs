public class Program
{
    public static void Main(string[] args)
    {
        ConfigParser config = new ConfigParser("somefilename");
        if(config.HasErrors())
        {
            Console.WriteLine("Sorry, but your config has errors");
            Environment.Exit(0);
        }
        else { string anotherConf = config.ParseFile(); }

        switch(args[0]) {
            case "start": 
                break;
            case "edit": 
                break;
            case "select": 
                break;
            case "status": 
                break;
            default: 
                Console.WriteLine($"Sorry but {args[0]} is an invalid option");
                break;
        }
    }
}
