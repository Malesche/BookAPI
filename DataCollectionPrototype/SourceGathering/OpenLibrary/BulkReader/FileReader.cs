using System.Text;
using GC = System.GC;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary.BulkReader;

internal class FileReader : IDisposable
{
    private StreamReader _file;

    public FileReader(string filePath)
    {
        var  fileStream = new FileStream(filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite);
        _file = new StreamReader(fileStream, Encoding.UTF8, true, 128);
    }

    ~FileReader()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool isDisposing)
    {
        _file.Dispose();
        _file = null;

        if (isDisposing)
        {
            GC.SuppressFinalize(this);
        }
    }

    public string ReadNextLine()
    {
        return _file.ReadLine();
    }
}