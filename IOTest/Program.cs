using System;
using NoName.AdofaiLevelIO;
using NoName.AdofaiLevelIO.Model;
using NoName.AdofaiLevelIO.Model.Actions;

AdofaiLevel adofaiLevel = new AdofaiLevel("First Town Of This Journey (Easy).adofai");

const double radian = 360 / (3.1415926535897 * 2);
Console.WriteLine(1.5707963705062866 * radian);

Console.WriteLine($"Artist: {adofaiLevel.LevelInfo.Artist}");
Console.WriteLine($"Author: {adofaiLevel.LevelInfo.Author}");
Console.WriteLine($"Song: {adofaiLevel.LevelInfo.Song}");
Console.WriteLine($"Bpm: {adofaiLevel.LevelInfo.Bpm}");
Console.WriteLine($"Floors.Count: {adofaiLevel.Floors.Count}");

Console.WriteLine("--------------Floors--------------");
foreach (var item in adofaiLevel.Floors)
{
    Console.Write(item.Index);
    Console.Write(" Actions:" + item.Actions.Count);
    Console.Write(" { ");
    foreach (var action in item.Actions)
    {
        var eventType = action.EventType;
        if (eventType == EventType.NotAvailable)
            continue;
        Console.Write(eventType.ToString());
        if (eventType == EventType.SetSpeed)
        {
            var speedInfo = action as SetSpeed;
            Console.Write(" ");
            Console.Write(speedInfo?.SpeedType);
            Console.Write(" ");
            Console.Write(speedInfo?.Value);
        }
    }
    Console.Write(" }");
    Console.WriteLine();
}
Console.WriteLine("--------------Floors--------------");

Console.WriteLine(adofaiLevel.RawData["pathData"]?.ToString());