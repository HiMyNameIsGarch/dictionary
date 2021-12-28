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

        var acc = CalculateAccuracy(synonyms, response);
        Accuracy.Values.Add(acc.Item2);

        bool isCorrect = synonyms.Any(response.Equals);

        ShowResponseStatus(isCorrect, OnPositiveResponse, 
                () => OnNegativeResponse(synonyms, acc.Item1));

        return isCorrect;
    }

    private void OnPositiveResponse()
    {
        if(config.Layout == LayoutType.Card)
        {
            ColorWriteLine(ResponseTime.GetText(), ConsoleColor.Cyan, config.HasColors);
        }
    }

    private void OnNegativeResponse(string[] correctWords, string mostAccurate)
    {
        if(correctWords.Length > 1)
        {
            ColorWrite("Most accurate word: ", ConsoleColor.Blue, config.HasColors);
            Write(mostAccurate + "\n");
        }
        ColorWrite("The answer can be: ", ConsoleColor.Blue, config.HasColors);
        Write(CombineWords(correctWords) + "\n");

        ColorWriteLine(Accuracy.GetText(), ConsoleColor.Cyan, config.HasColors);
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
