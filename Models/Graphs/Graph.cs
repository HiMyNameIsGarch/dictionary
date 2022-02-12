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

    public void Draw()
    {
        if(!CanDrawGraph()) return;

        var beforeTable = Console.GetCursorPosition();
        beforeTable.Left += _yaxis.Max.Length();

        DisplayTable();

        DisplayValues(beforeTable.Left + 2, beforeTable.Top);
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

    private string BuildChar(int num, char c)
    {
        string s = "";
        for(int i = 0; i < num; i++)
        {
            s += c;
        }
        return s;
    }

    private int GetSpaces(int max, int current) => (max == current) ? 1 : max - current;

    private void DisplayLeftPart()
    {
        var numLength = _yaxis.Max.Length() + 1;
        for(int i = _yaxis.Max; i >= _yaxis.Min; i -= _yaxis.Rate)
        {
            string spaces = BuildChar(GetSpaces(numLength, i.Length()), ' ');
            Console.WriteLine(i + spaces + "│");
        }
        string cornerSpaces = BuildChar(GetSpaces(numLength, _yaxis.Min.Length()), ' ');
        Console.Write(cornerSpaces + " " + "└");
    }
    private void DisplayBottomPart()
    {
        Console.Write("─");
        var beforeDraw = Console.GetCursorPosition();
        int j = 0;
        for(int i = _xaxis.Min; i <= _xaxis.Max; i += _xaxis.Rate)
        {
            var currentCursor = Console.GetCursorPosition();
            string line = BuildChar(i.Length() + 1, '─');
            Console.Write(line);
            Console.SetCursorPosition(currentCursor.Left, currentCursor.Top + 1);
            _axis[j] = currentCursor.Left;
            j++;
            Console.Write(i);
            Console.SetCursorPosition(currentCursor.Left + i.Length() + 1, currentCursor.Top);
        }
        Console.Write("\n");
        Console.SetCursorPosition(beforeDraw.Left, beforeDraw.Top + 2);
        Console.Write("\n");
    }

    private void DisplayTable()
    {
        DisplayLeftPart();

        DisplayBottomPart();
    }

    private void DisplayValues(int left, int top)
    {
        var beforeDraw = Console.GetCursorPosition();
        for(int i = 0; i < _coordonates.Length; i++)
        {
            int valueOnGraph = (_yaxis.Max - _coordonates[i] ) / _yaxis.Rate;
            int startPoint = top + valueOnGraph;
            int downPoints = _yaxis.Values.Length - valueOnGraph;
            for(int j = startPoint; j < startPoint + downPoints; j++)
            {
                for(int k = 0; k < _xaxis.Values[i].Length(); k++)
                {
                    Console.SetCursorPosition(_axis[i] + k, j);
                    DisplayPoint(_yaxis.Values.Length - (j - top), j == startPoint);
                }
            }
        }
        Console.SetCursorPosition(beforeDraw.Left, beforeDraw.Top);
    }

    private void DisplayPoint(int current, bool isFirstPoint)
    {
        int maxBound = _yaxis.Values.Length;
        int minBound = (int)Math.Ceiling(0.3 * maxBound);
        int middleBound = maxBound - minBound;
        // Color 
        Console.ForegroundColor = GetCurrentColorFor(maxBound, minBound, middleBound, current);
        Console.Write(isFirstPoint ? "▄" : "█");
        Console.ForegroundColor = ConsoleColor.White;
    }

    private ConsoleColor GetCurrentColorFor(int max, int min, int middle, int current)
    {
        if(current > middle && current <= max)
            return ConsoleColor.Green;
        else if(current > min && current <= middle)
            return ConsoleColor.Yellow;
        else if(current <= min)
            return ConsoleColor.Red;
        else 
            return ConsoleColor.White;
    }
}
