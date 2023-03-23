using System.IO;

namespace SaberTask
{
    internal interface IDeserializer
    {
        public void DeserializeHandler(FileStream fileStream);
    }
}
