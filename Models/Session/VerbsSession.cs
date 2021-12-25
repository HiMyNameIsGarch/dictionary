using static System.Console;
using static ConsoleHelper;

public class VerbsSession : BaseSession
{
    public VerbsSession(SessionData sessionData) : base(sessionData) {}

    public override void Start()
    {
        foreach(var verbs in pairs)
        {
            var cursorTop = Console.CursorTop;
            CurrentPair++;
            if(IrregularVerbs.CanBuildVerbPairs(verbs.Value))
            {
                var verbsPairs = new IrregularVerbs(
                            verbs.Value[0], 
                            verbs.Value[1],
                            verbs.Value[2]);
                WriteLine(Delimiter);
                AskQuestion(verbs.Key, verbsPairs);
                ClearScreen(cursorTop);
            }
        }
    }

    private string firstResponse = "";
    private string secondResponse = "";
    private string thirdResponse = "";

    public bool AskQuestion(string[] values, IrregularVerbs verbs) 
    {
        string prompt = CombineWords(values);

        string firstVerb = GetUserResponse($"Write <1ST> form of -> {prompt}: ");
        firstResponse = "First " + ResponseTimeText;
        string secondVerb = GetUserResponse($"Write <2ND> form of -> {prompt}: ");
        secondResponse = "Second " + ResponseTimeText;
        string thirdVerb = GetUserResponse($"Write <3RD> form of -> {prompt}: ");
        thirdResponse = "Third " + ResponseTimeText;

        var InputVerbs = new IrregularVerbs(firstVerb, secondVerb, thirdVerb);
        int CurrentPoints = verbs.GetPointsFrom(InputVerbs);
        Points += CurrentPoints;

        bool IsCorrect = verbs.ArePairsCorrect(InputVerbs);

        if(config.DisplayOnPairStats) 
            ShowResponseStatus(verbs, InputVerbs, CurrentPoints, IsCorrect);

        return IsCorrect;
    }

    private void DisplayStatistics()
    {
        if(config.Layout == LayoutType.Card)
        {
            ColorWriteLine(firstResponse, ConsoleColor.Cyan, config.HasColors);
            ColorWriteLine(secondResponse, ConsoleColor.Cyan, config.HasColors);
            ColorWriteLine(thirdResponse, ConsoleColor.Cyan, config.HasColors);
        }
    }

    private void ShowResponseStatus(IrregularVerbs verbs, IrregularVerbs input, 
            int currentPoints, bool IsPositive)
    {
        if(IsPositive)
        {
            ColorWriteLine("Correct!", ConsoleColor.Green, config.HasColors);
            DisplayStatistics();
        }
        else
        {
            ColorWriteLine("Incorrect!", ConsoleColor.Red, config.HasColors);
            DisplayCorrectAnswer(verbs, input);
            WriteLine($"You got {currentPoints} of {IrregularVerbs.MaxVerbs} pairs");
        }
        PressKeyToContinue();
    }

    private void DisplayCorrectAnswer(IrregularVerbs wanted, IrregularVerbs got)
    {
        Write("The answer is: ");
        ColorOutput(wanted.First, got.First);
        Write(" | ");
        ColorOutput(wanted.Second, got.Second);
        Write(" | ");
        ColorOutput(wanted.Third, got.Third);
        Write("\n");
    }

    private void ColorOutput(string wanted, string got)
    {
        ColorWrite(wanted, 
                wanted == got ? ConsoleColor.Green : ConsoleColor.Red, 
                config.HasColors);
    }

    public override void DisplayAfterSession() 
    {
        int totalPoints = pairs.Count * IrregularVerbs.MaxVerbs;
        WriteLine($"Wow, you got {Points} of {totalPoints} points!");
        WriteLine($"Avarage response time -> {GetAvarageResponseTime} seconds.");
    }

    public override void DisplayBeforeSession() { base.DisplayBeforeSession(); }

    public override void DisplayStatusFor(string logs) { throw new NotImplementedException(); }
}
