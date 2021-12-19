public class IrregularVerbs
{
    public static readonly int MaxVerbs = 3;

    public string First { get; }
    public string Second { get; }
    public string Third { get; }

    public IrregularVerbs(string one ,string two, string three)
    {
        First = one;
        Second = two;
        Third = three;
    }

    public static bool CanBuildVerbPairs(string[] pairs)
    {
        return pairs.Length == MaxVerbs;
    }

    public bool ArePairsCorrect(IrregularVerbs userVerbs)
    {
        return First == userVerbs.First && Second == userVerbs.Second && Third == userVerbs.Third;
    }

    public int GetPointsFrom(IrregularVerbs verbs)
    {
        int points = 0;
        if(First == verbs.First) points++;
        if(Second == verbs.Second) points++;
        if(Third == verbs.Third) points++;
        return points;
    }
}
