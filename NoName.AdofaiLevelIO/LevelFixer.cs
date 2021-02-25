using System;
using System.IO;

namespace NoName.AdofaiLevelIO
{
    internal class LevelFixer
    {
        internal static string Fix(string json)
        {
            var stringReader = new StringReader(json);
            Console.WriteLine(stringReader.ReadLine());

            throw new NotImplementedException();
        }
    }
}