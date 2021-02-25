using System;
using NoName.AdofaiLevelIO;

Console.Write("file path: ");
AdofaiLevel adofaiLevel = new(Console.ReadLine());
Console.WriteLine($"Artist: {adofaiLevel.LevelInfo.Artist}");
Console.WriteLine($"Author: {adofaiLevel.LevelInfo.Author}");
Console.WriteLine($"Song: {adofaiLevel.LevelInfo.Song}");
Console.WriteLine($"Bpm: {adofaiLevel.LevelInfo.Bpm}");
Console.WriteLine($"Floors.Count: {adofaiLevel.Floors.Count}");

Console.WriteLine("");
Console.Write("start floor index: ");
var startindex = int.Parse(Console.ReadLine() ?? string.Empty);
Console.Write("end floor index: ");
var endindex = int.Parse(Console.ReadLine() ?? string.Empty);
Console.Write("bpm: ");
var bpm = (float)int.Parse(Console.ReadLine() ?? string.Empty);

for (var i = startindex; i < endindex; i++)
{
    var floor = adofaiLevel.Floors[i];

    if (i % 4 == 1 || i % 4 == 2)
        floor.Bpm = (float)(floor.RelativeAngle * 360.0 / (Math.PI * 2.0) / 180 * (bpm / 2));
    else
        floor.Bpm = (float)(floor.RelativeAngle * 360.0 / (Math.PI * 2.0) / 180 * bpm);
}

Console.WriteLine("OK.");
adofaiLevel.SaveLevel();
Console.WriteLine("LevelSaved.");
