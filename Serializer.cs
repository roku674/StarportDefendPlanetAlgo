//Created by Alexander Fields http://alexanderfields.me
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace StarportDefendPlanetAlgo
{
    public class Serializer
    {
        public static void Serialize(object t, string path)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, t);
            }
        }

        public static int[,] Deserialize(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                return (int[,])bformatter.Deserialize(stream);
            }
        }
    }
}