using static System.Console;
using static ConsoleHelper;

public class Session : BaseSession
{

    public Session(DataSession sessionData) : base(sessionData) { }

    public override void Start()
    {
        // Create a loop based on shuffled pairs
        foreach(var words in pairs)
        {
            WriteLine("-----------------------");
            if(AskQuestion(words.Key, words.Value)) Points++;
        }
    }

    public override void DisplayStatusFor(string logs)
    {
        throw new NotImplementedException();
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
}
