public interface IParser<T>
{
    string FilePath { get; }

    public T ParseFile();

    public bool HasErrors();

}

