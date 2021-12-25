using static System.Console;
using static ConsoleHelper;

public class VerbsSession : BaseSession
{

    private string _firstResponse = "";
    private string _secondResponse = "";
    private string _thirdResponse = "";

    private int _currentPoints = 0;
    private IrregularVerbs _currentVerbs = new IrregularVerbs("","","");
    private IrregularVerbs _currentInputVerbs = new IrregularVerbs("","","");

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

    public bool AskQuestion(string[] values, IrregularVerbs verbs) 
    {
        string prompt = CombineWords(values);
        _currentVerbs = verbs;

        string firstVerb = GetForm(1, prompt);
        _firstResponse = "First " + ResponseTimeText;
        string secondVerb = GetForm(2, prompt);
        _secondResponse = "Second " + ResponseTimeText;
        string thirdVerb = GetForm(3, prompt);
        _thirdResponse = "Third " + ResponseTimeText;

        _currentInputVerbs = new IrregularVerbs(firstVerb, secondVerb, thirdVerb);

        _currentPoints = verbs.GetPointsFrom(_currentInputVerbs);
        Points += _currentPoints;

        bool IsCorrect = verbs.ArePairsCorrect(_currentInputVerbs);

        ShowResponseStatus(IsCorrect);

        return IsCorrect;
    }

    private string GetForm(int num, string prompt)
    {
        return GetUserResponse($"Write <{num}ST> form of -> {prompt}: ");
    }

    private protected override void OnPositiveResponse()
    {
        if(config.Layout == LayoutType.Card)
        {
            ColorWriteLine(_firstResponse, ConsoleColor.Cyan, config.HasColors);
            ColorWriteLine(_secondResponse, ConsoleColor.Cyan, config.HasColors);
            ColorWriteLine(_thirdResponse, ConsoleColor.Cyan, config.HasColors);
        }
    }

    private protected override void OnNegativeResponse()
    {
        DisplayCorrectAnswer(_currentVerbs, _currentInputVerbs);
        WriteLine($"You got {_currentPoints} of {IrregularVerbs.MaxVerbs} pairs");
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

    public override void DisplayStatusFor(string logs) { throw new NotImplementedException(); }
}
