public class Graph
{
    private Axis _yaxis;
    private Axis _xaxis;
    private int[] _values;
    private int[] _axis;
    private bool _invertColors;

    public Graph(Axis yaxis, Axis xaxis, int[] values, bool invertColors)
    {
        _yaxis = yaxis;
        _yaxis.ComputeValues();
        _xaxis = xaxis;
        _xaxis.ComputeValues();
        _values = values;
        _invertColors = invertColors;
        _axis = new int[_values.Length];
    }

    public void Draw()
    {
        MinimizeValues();

        if(!CanDrawGraph()) return;

        int pointBeforeTable = Console.GetCursorPosition().Top;

        DisplayTable();

        DisplayValues(pointBeforeTable);
    }

    private int GetxaxisLength()
    {
        int length = 0;
        for(int i = _xaxis.Max; i >= _xaxis.Min; i -= _xaxis.Rate)
        {
            length += i.Length();
        }
        length += _xaxis.Values.Length + _yaxis.Max.Length() + 3; // 2 spaces and the line
        return length;
    }

    private void MinimizeValues()
    {
        while(Console.WindowWidth < GetxaxisLength())
        {
            // Increase the rate
            _xaxis.Rate += _xaxis.Rate;
            _xaxis.ComputeValues();

            int[] shortValues = new int[_xaxis.Values.Length];
            _axis = new int[_xaxis.Values.Length];
            int i = 0;
            foreach(var v in _xaxis.Values)
            {
                shortValues[i] = GetValuesFrom(v, _xaxis.Rate);
                i++;
            }
            _values = shortValues;
        }
    }
    private int GetValuesFrom(int position, int num)
    {
        double total = 0;
        for(int i = position; i < position + num; i++)
        {
            if(_values.Length > i - 1) total += _values[i - 1];
        }
        return (int)Math.Round(total / num);
    }

    private bool CanDrawGraph()
    {
        if(_yaxis.Values.Length == 0 || _xaxis.Values.Length == 0)
        {
            Console.WriteLine("One of the axis is empty");
            return false;
        }
        if(_values.Length != _xaxis.Values.Length)
        {
            Console.WriteLine("The values provided aren't equal to the x axis");
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
        for(int i = 0; i < _values.Length; i++)
        {
            // Make sure the values aren't bigger than max value on yaxis
            _values[i] = _values[i] > _yaxis.Max ? _yaxis.Max : _values[i]; 
            int valueOnGraph = _yaxis.Max - _values[i];
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
