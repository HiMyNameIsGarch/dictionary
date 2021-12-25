using static System.Console;
using static ConsoleHelper;

public class WordsSession : BaseSession
{

    private string[] _currentValues = new string[0];

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

        _currentValues = synonyms;

        ShowResponseStatus(isCorrect);

        return isCorrect;
    }

    private protected override void OnPositiveResponse() 
    {
        if(config.Layout == LayoutType.Card)
        {
            ColorWriteLine(ResponseTimeText, ConsoleColor.Cyan, config.HasColors);
        }
    }

    private protected override void OnNegativeResponse() 
    {
        ColorWrite("The answer is: ", ConsoleColor.Blue, config.HasColors);
        Write(CombineWords(_currentValues));
        Write("\n");
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
