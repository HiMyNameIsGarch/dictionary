using static System.Console;
using static ConsoleHelper;

public class VerbsSession : BaseSession
{
    public VerbsSession(DataSession sessionData) : base(sessionData) {}

    public override void Start()
    {
        foreach(var verbs in pairs)
        {
            CurrentPair++;
            if(IrregularVerbs.CanBuildVerbPairs(verbs.Value))
            {
                var verbsPairs = new IrregularVerbs(
                            verbs.Value[0], 
                            verbs.Value[1],
                            verbs.Value[2]);
                WriteLine(Delimiter);
                AskQuestion(verbs.Key, verbsPairs);
            }
        }
    }

    public bool AskQuestion(string[] values, IrregularVerbs verbs) 
    {
        string prompt = CombineWords(values);

        string firstVerb = GetUserResponse($"Write <1ST> form of -> {prompt}: ");
        string secondVerb = GetUserResponse($"Write <2ND> form of -> {prompt}: ");
        string thirdVerb = GetUserResponse($"Write <3RD> form of -> {prompt}: ");

        bool IsCorrect = verbs.ArePairsCorrect(new IrregularVerbs(firstVerb, secondVerb, thirdVerb));

        if(config.DisplayOnPairStats) 
            ShowResponseStatus(verbs, new IrregularVerbs(firstVerb, secondVerb, thirdVerb), IsCorrect);

        return IsCorrect;
    }

    private void ShowResponseStatus(IrregularVerbs verbs, IrregularVerbs input, bool IsPositive)
    {
        if(IsPositive)
        {
            ColorWriteLine("Correct!", ConsoleColor.Green, config.HasColors);
        }
        else
        {
            int correctVerbs = verbs.GetPointsFrom(input);
            Points += correctVerbs;
            ColorWriteLine("Incorrect!", ConsoleColor.Red, config.HasColors);
            WriteLine($"The answer is: {verbs.First} | {verbs.Second} | {verbs.Third}");
            WriteLine($"You got {Points} of {IrregularVerbs.MaxVerbs} pairs");
        }
    }

    public override void DisplayAfterSession() 
    {
        int totalPoints = pairs.Count * IrregularVerbs.MaxVerbs;
        WriteLine($"Wow, you got {Points} of {totalPoints} points!");
    }

    public override void DisplayBeforeSession() { base.DisplayBeforeSession(); }

    public override void DisplayStatusFor(string logs) { throw new NotImplementedException(); }
}
