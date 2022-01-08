public static class EditDistance
{
    public const double MaxAccuracy = 100.0;

    public static double GetAccuracy(string wanted, string got)
    {
        if(wanted == got) return MaxAccuracy;
        char[] wantedArray = FillArray(wanted);
        char[] gotArray = FillArray(got);
        int[,] matrix = new int[gotArray.Length, wantedArray.Length];
        int k = 0;
        foreach(char c in gotArray)
        {
            matrix[k, 0] = k;
            k++;
        }
        k = 0;
        foreach(char c in wantedArray)
        {
            matrix[0, k] = k;
            k++;
        }
        // Calculate edit distance
        double editDistance = ComputeEditDistance(gotArray, wantedArray, matrix);
        if(editDistance == 0) return MaxAccuracy;
        // Get the actual number
        double num = (double)wanted.Length / editDistance;
        double accuracy = MaxAccuracy - ( MaxAccuracy / num );
        return accuracy;
    }

    public static Tuple<string,double> GetAccuracy(string[] wanted, string got)
    {
        double mostAccurate = -1;
        string correctWord = "";
        foreach(var word in wanted)
        {
            double acc = GetAccuracy(word, got);
            if(acc > mostAccurate)
            {
                mostAccurate = acc;
                correctWord = word;
            }
        }
        return new Tuple<string, double>(correctWord, mostAccurate);
    }

    private static char[] FillArray(string word)
    {
        char[] array = new char[word.Length + 1];
        array[0] = ' ';
        int i = 1;
        foreach(char c in word)
        {
            array[i] = c;
            i++;
        }
        return array;
    }

    private static double ComputeEditDistance(char[] wanted, char[] got, int[,] matrix)
    {
        for (int i = 1; i < wanted.Length; i++)
        {
            for (int j = 1; j < got.Length; j++)
            {
                if(wanted[i] == got[j])
                    matrix[i,j] = matrix[i - 1, j - 1];
                else
                    matrix[i,j] = Min(matrix, i, j) + 1;
            }
        }
        return matrix[wanted.Length - 1, got.Length - 1];
    }

    private static int Min(int[,] matrix, int i, int j)
    {
        int insert = matrix[i, j - 1];
        int delete = matrix[i - 1, j];
        int replace = matrix[i - 1, j - 1];
        if(insert < delete && insert < replace)
            return insert;
        else if (delete < insert && delete < replace)
            return delete;
        else 
            return replace;
    }

    private static void DisplayMatrix(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i,j] + "   ");
            }
            Console.WriteLine("\n");
        }
    }
}
