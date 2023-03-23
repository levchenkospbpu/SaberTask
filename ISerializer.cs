using System.IO;

namespace SaberTask
{
    internal interface ISerializer
    {
        public void SerializeHandler(FileStream fileStream);
    }
}
