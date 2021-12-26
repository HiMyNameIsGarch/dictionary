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

    public bool AskQuestion(string[] values, IrregularVerbs verbs) 
    {
        string prompt = CombineWords(values);
        var verbsArray = verbs.ToArray();

        var firstVerb = GetValue(1, prompt, verbsArray);
        var secondVerb = GetValue(2, prompt, verbsArray);
        var thirdVerb = GetValue(3, prompt, verbsArray);

        var currentInputVerbs = new IrregularVerbs(firstVerb, secondVerb,
                thirdVerb);

        var currentPoints = verbs.GetPointsFrom(currentInputVerbs);
        Points += currentPoints;

        bool IsCorrect = verbs.ArePairsCorrect(currentInputVerbs);

        ShowResponseStatus(IsCorrect, OnPositiveResponse, 
                () => OnNegativeResponse(verbs, currentInputVerbs, currentPoints));

        return IsCorrect;
    }

    private string GetValue(int num, string prompt, string[] values)
    {
        string numString = num == 1 ? "First " : num == 2 ? "Second " : num == 3 ? "Third " : "";
        string verb = GetForm(num, prompt);
        string response = numString + ResponseTime.GetText().ToLower();
        double accuracy = CalculateAccuracy(values[num - 1], verb);
        Accuracy.Values.Add(accuracy);
        return verb;
    }

    private string GetForm(int num, string prompt)
    {
        string termination = num == 1 ? "ST" : num == 2 ? "ND" : num == 3 ? "RD" : "";
        return GetUserResponse($"Write <{num}{termination}> form of -> {prompt}: ");
    }

    private void OnPositiveResponse()
    {
        if(config.Layout == LayoutType.Card)
        {
            ColorWriteLine(ResponseTime.GetTextOnLast(3,
                        config.DisplayAvarageStatistics), ConsoleColor.Cyan,
                    config.HasColors);
        }
        if(!config.DisplayAvarageStatistics) WriteLine("-----");
        ColorWriteLine(Accuracy.GetTextOnLast(3,
                    config.DisplayAvarageStatistics), ConsoleColor.Cyan,
                config.HasColors);
    }

    private void OnNegativeResponse(IrregularVerbs currentVerbs, IrregularVerbs
            inputVerbs, int currentPoints)
    {
        DisplayCorrectAnswer(currentVerbs, inputVerbs);
        WriteLine($"You got {currentPoints} of {IrregularVerbs.MaxVerbs} pairs");
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
        WriteLine($"Avarage response time -> {ResponseTime.AvarageNum} seconds.");
        WriteLine($"Avarage accuracy -> {Accuracy.AvarageNum}.");
    }

    public override void DisplayStatusFor(string logs) { throw new NotImplementedException(); }
}
