public interface IParser<T>
{
    string FilePath { get; }

    string FileText { get; }

    public T ParseFile();

}

