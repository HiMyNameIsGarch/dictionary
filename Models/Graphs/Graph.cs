public class Graph
{
    private Bar _yaxis;
    private Bar _xaxis;
    private int[] _coordonates;
    private int[] _axis;
    private bool _invertColors;

    public Graph(Bar yaxis, Bar xaxis, int[] coordonates, bool invertColors)
    {
        _yaxis = yaxis;
        _yaxis.ComputeValues();
        _xaxis = xaxis;
        _xaxis.ComputeValues();
        _coordonates = coordonates;
        _invertColors = invertColors;
        _axis = new int[_coordonates.Length];
    }

    public void Draw()
    {
        if(!CanDrawGraph()) return;

        int pointBeforeTable = Console.GetCursorPosition().Top;

        DisplayTable();

        DisplayValues(pointBeforeTable);
    }

    private bool CanDrawGraph()
    {
        if(Console.WindowWidth < _xaxis.Values.Length * 2)
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
        string cornerSpaces = BuildChar(_yaxis.Max.Length() + 1, ' ');
        Console.Write(cornerSpaces + "└");
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

    private void DisplayValues(int topPoint)
    {
        var beforeDraw = Console.GetCursorPosition();
        for(int i = 0; i < _coordonates.Length; i++)
        {
            // Make sure the values aren't bigger than max value on yaxis
            _coordonates[i] = _coordonates[i] > _yaxis.Max ? _yaxis.Max : _coordonates[i]; 
            int valueOnGraph = _yaxis.Max - _coordonates[i];
            valueOnGraph = (int)Math.Round((double)valueOnGraph / _yaxis.Rate);
            int startPoint = topPoint + valueOnGraph;
            int downPoints = _yaxis.Values.Length - valueOnGraph;
            for(int j = startPoint; j < startPoint + downPoints; j++)
            {
                for(int k = 0; k < _xaxis.Values[i].Length(); k++)
                {
                    Console.SetCursorPosition(_axis[i] + k, j);
                    DisplayPoint(_yaxis.Values.Length - (j - topPoint), j == startPoint);
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
            return _invertColors ? ConsoleColor.Red : ConsoleColor.Green;
        else if(current > min && current <= middle)
            return ConsoleColor.Yellow;
        else if(current <= min)
            return _invertColors ? ConsoleColor.Green: ConsoleColor.Red;
        else 
            return ConsoleColor.White;
    }
}
