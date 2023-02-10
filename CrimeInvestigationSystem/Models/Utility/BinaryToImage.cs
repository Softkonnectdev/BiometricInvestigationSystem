using System.Data.Linq;
using System.Drawing;
using System.IO;

namespace CrimeInvestigationSystem.Models.Utility
{
    public class BinaryToImage
    {

        public static Image GetBinaryImage(Binary binaryData)
        {
            if (binaryData == null) return null;

            byte[] buffer = binaryData.ToArray();
            MemoryStream memStream = new MemoryStream();
            memStream.Write(buffer, 0, buffer.Length);
            return Image.FromStream(memStream);
        }



    }
}