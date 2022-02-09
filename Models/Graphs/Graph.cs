public class Graph
{
    private Bar _yaxis;
    private Bar _xaxis;
    private int[] _coordonates;
    private int[] _axis;

    public Graph(Bar yaxis, Bar xaxis, int[] coordonates)
    {
        _yaxis = yaxis;
        _yaxis.ComputeValues();
        _xaxis = xaxis;
        _xaxis.ComputeValues();
        _coordonates = coordonates;
        _axis = new int[_coordonates.Length];
    }

    private bool CanDrawGraph()
    {

        if(Console.WindowWidth < _xaxis.Values.Length)
        {
            Console.WriteLine("Values are bigger than screen, abording...");
            return false;
        }
        if(_yaxis.Values.Length == 0 || _xaxis.Values.Length == 0)
        {
            Console.WriteLine("Cannot get the values for the outsides");
            return false;
        }
        if(_coordonates.Length != _xaxis.Values.Length)
        {
            Console.WriteLine("The values provided aren't equal to the _xaxis table");
            return false;
        }
        return true;
    }

    private string BuildSpaces(int max, int current)
    {
        string s = "";
        var maxSpaces = (max == current) ? 1 : max - current;
        for(int i = 0; i < maxSpaces; i++)
        {
            s += " ";
        }
        return s;
    }

    private string BuildSpaces(int max)
    {
        string s = "─";
        for(int i = 0; i < max; i++)
        {
            s += s;
        }
        return s;
    }

    private void DisplayLeftPart()
    {
        var numLength = _yaxis.Max.ToString().Length + 1;
        for(int i = _yaxis.Max; i >= _yaxis.Min; i -= _yaxis.Rate)
        {
            Console.WriteLine(i + BuildSpaces(numLength, i.ToString().Length) + "│");
        }
        Console.Write(BuildSpaces(numLength, _yaxis.Min.ToString().Length) + " " + "└");
    }
    private void DisplayBottomPart()
    {
        Console.Write("─");
        var current = Console.GetCursorPosition();
        int j = 0;
        for(int i = _xaxis.Min; i <= _xaxis.Max; i += _xaxis.Rate)
        {
            var aj = Console.GetCursorPosition();
            Console.Write(BuildSpaces(i.ToString().Length));
            Console.SetCursorPosition(aj.Left, aj.Top + 1);
            j++;
            _axis[j - 1] = aj.Left;
            Console.Write(i);
            Console.SetCursorPosition(aj.Left + i.ToString().Length + 1, aj.Top);
        }
        Console.Write("\n");
        Console.SetCursorPosition(current.Left, current.Top + 2);
        Console.Write("\n");
    }

    private void DisplayTable()
    {
        // Display left part
        DisplayLeftPart();
        // Display bottom part
        DisplayBottomPart();
    }

    private void DisplayValues(int left, int top)
    {
    }

    public void Draw()
    {
        if(!CanDrawGraph()) return;

        var beforeTable = Console.GetCursorPosition();
        beforeTable.Left += _yaxis.Max.ToString().Length;

        DisplayTable();

        DisplayValues(beforeTable.Left + 2, beforeTable.Top);
    }
}
