using static System.Console;
using static ConsoleHelper;

public class WordsSession : BaseSession
{

    public WordsSession(SessionData sessionData) : base(sessionData) { }

    public override void Start()
    {
        var cursorTop = Console.CursorTop;
        foreach(var words in pairs)
        {
            CurrentPair++;
            WriteLine(Delimiter);
            if(AskQuestion(words.Key, words.Value)) Points++;
            ClearScreen(cursorTop);
        }
    }

    private bool AskQuestion(string[] words, string[] synonyms)
    {
        string response = GetUserResponse(GetQuestionString(words));

        bool isCorrect = synonyms.Any(response.Equals);

        if(config.DisplayOnPairStats) ShowReponseStatus(synonyms, isCorrect);

        return isCorrect;
    }

    private void DisplayStatistics() 
    {
        if(config.Layout == LayoutType.Card)
        {
            ColorWriteLine(ResponseTimeText, ConsoleColor.Cyan, config.HasColors);
        }
    }

    private void ShowReponseStatus(string[] values, bool isPositive)
    {
        if(isPositive)
        {
            ColorWriteLine("Correct!", ConsoleColor.Green, config.HasColors);
            DisplayStatistics();
        } 
        else 
        {
            ColorWriteLine("Incorrect!", ConsoleColor.Red, config.HasColors);
            ColorWrite("The answer is: ", ConsoleColor.Blue, config.HasColors);
            Write(CombineWords(values));
            Write("\n");
        }
        PressKeyToContinue();
    }

    private string GetQuestionString(string[] words)
    {
        string question = "What means -> ";
        question += CombineWords(words);
        question += ": ";
        return question;
    }

    public override void DisplayStatusFor(string logs)
    {
        throw new NotImplementedException();
    }
}
