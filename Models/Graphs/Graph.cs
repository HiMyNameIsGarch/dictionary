public class Graph
{
    private int[] _left;
    private int[] _bottom;
    private int[] _values;
    private int[] _axis;

    public Graph(Bar left, Bar bottom, int[] values)
    {
        _left = left.GetValues();
        _bottom = bottom.GetValues();
        _values = values;
        _axis = new int[_values.Length];
    }

    private bool CanDrawGraph()
    {
        if(Console.WindowWidth < _values.Length)
        {
            Console.WriteLine("Values bigger than screen, abording...");
            return false;
        }
        return true;
    }

    private void DisplayLeftPart(int[] bounds)
    {
    }
    private void DisplayBottomPart(int[] values)
    {
    }

    private void DisplayTable()
    {
        // Display left part
        DisplayLeftPart(_left);
        // Display bottom part
        DisplayBottomPart(_values);
    }

    private void DisplayValues(int left, int top)
    {
    }

    public void Draw()
    {
        if(!CanDrawGraph()) return;

        var beforeTable = Console.GetCursorPosition();
        beforeTable.Left += _left[_left.Length].ToString().Length;

        DisplayTable();

        DisplayValues(beforeTable.Left + 2, beforeTable.Top);
    }
}
