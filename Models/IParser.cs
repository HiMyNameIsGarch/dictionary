public interface IParser<T>
{
    string FilePath { get; }

    string FileText { get; }

    string BaseDirectory { get; }

    public T ParseFile();

}

