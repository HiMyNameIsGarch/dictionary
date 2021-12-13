using static System.Console;
using static ConsoleHelper;

public class Session 
{
    private readonly DataSession _data;

    private Config config 
    { 
        get { return _data.Config; } 
    }
    private Dictionary<string[], string[]> pairs 
    { 
        get { return _data.Pairs; }
    }

    public Session(DataSession dataSession) 
    {
        _data = dataSession;
    }

    public void Start()
    {
        WriteLine($"Session started on file '{config.CurrentFile}'");
        // Shuffle pairs
        int points = 0;
        foreach(var words in ShufflePairs(pairs))
        {
            WriteLine("-----------------------");
            if(AskQuestion(words.Key, words.Value)) points++;
        }

        WriteLine($"\nWow, you got {points} points out of {pairs.Count}");
    }

    private bool AskQuestion(string[] words, string[] synonyms)
    {
        string? response = "";
        do 
        {
            Write(GetQuestionString(words));
            response = ReadLine()?.Trim();
        }
        while(string.IsNullOrWhiteSpace(response));

        bool isCorrect = synonyms.Any(response.Equals);

        ShowReponseStatus(synonyms, isCorrect);

        return isCorrect;
    }

    private void ShowReponseStatus(string[] values, bool isPositive)
    {
        if(isPositive)
        {
            ColorWriteLine("Correct!", ConsoleColor.Green, config.HasColors);
        } 
        else 
        {
            ColorWriteLine("Incorrect!", ConsoleColor.Red, config.HasColors);
            ColorWrite("The answer is: ", ConsoleColor.Blue, config.HasColors);
            for(int i = 0; i < values.Length; i++)
            {
                Write(values[i]);
                if(i + 1 != values.Length) Write(" or ");
            }
            Write("\n");
        }
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

    private Dictionary<string[], string[]> 
        ShufflePairs(Dictionary<string[], string[]> pairs)
    {
        Random rand = new Random();
        return pairs.OrderBy(x => rand.Next())
            .ToDictionary(item => item.Key, item => item.Value);
    }
}
