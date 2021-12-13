using static System.Console;

public class Session 
{
    private Config config { get; }
    private Dictionary<string[], string[]> pairs { get; }

    public Session(DataSession data) 
    {
        config = data.Config;
        pairs = data.Pairs;
    }

    public void Start()
    {
        WriteLine($"Session started on file '{config.CurrentFile}'");
        int points = 0;
        foreach(var words in pairs)
        {
            WriteLine("-----------------------");
            if(AskQuestion(words.Key, words.Value)) points++;
        }

        WriteLine($"\nWow, you got {points} points out of {pairs.Count}");
    }

    private bool AskQuestion(string[] words, string[] synonyms)
    {
        // Display words
        string? response = "";
        do 
        {
            Write(GetQuestionString(words));
            response = ReadLine()?.Trim();
        }
        while(string.IsNullOrWhiteSpace(response));

        bool isCorrect = synonyms.Any(response.Contains);
        if(isCorrect)
        {
            WriteLine("yay, it's correct");
        } else {
            WriteLine("nope, that wasn't the answer");
            Write("The answer is: ");
            for(int i = 0; i < synonyms.Length; i++)
            {
                Write(synonyms[i]);
                if(i + 1 != synonyms.Length) Write(" or ");
            }
            Write("\n");
        }
        return isCorrect;
    }

    private string GetQuestionString(string[] words)
    {
        string question = "What means -> ";
        for(int i = 0; i < words.Length; i++) 
        {
            question += words[i];
            if(i + 1 != words.Length) question += " or ";
        }
        question += ": ";
        return question;
    }
}
