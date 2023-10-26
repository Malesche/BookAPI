using System.Text;

namespace DataCollectionPrototype.SourceGathering.OpenLibrary.BulkReader
{
    internal class FileReader
    {
        private readonly FileStream _fileStream;
        private StreamReader _file;

        public FileReader(string filePath)
        {
            _fileStream = new FileStream(filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite);
            _file = new StreamReader(_fileStream, Encoding.UTF8, true, 128);
        }

        public string ReadNextLine()
        {
            return _file.ReadLine();
        }
    }
}
