using System;
using NoName.AdofaiLevelIO;
using NoName.AdofaiLevelIO.Model.Actions;

AdofaiLevel adofaiLevel = new("level.adofai");
#pragma warning disable CA1416 // Validate platform compatibility
Console.WindowWidth = 200;
#pragma warning restore CA1416 // Validate platform compatibility
Console.WindowHeight = 50;
Console.WriteLine($"Artist: {adofaiLevel.LevelInfo.Artist}");
Console.WriteLine($"Author: {adofaiLevel.LevelInfo.Author}");
Console.WriteLine($"Song: {adofaiLevel.LevelInfo.Song}");
Console.WriteLine($"Bpm: {adofaiLevel.LevelInfo.Bpm}");
Console.WriteLine($"Floors.Count: {adofaiLevel.Floors.Count}");
Console.WriteLine("--------------Floors--------------");
foreach (var item in adofaiLevel.Floors)
{
    Console.Write($"FloorIndex: {item.Index:D3}");
    Console.Write($" Direction: {item.Direction}");
    Console.Write($" EntryAngle: {item.EntryAngle * 360.0 / (Math.PI * 2.0):000.0}");
    Console.Write($" ExitAngle: {item.ExitAngle * 360.0 / (Math.PI * 2.0):000.0}");
    Console.Write($" RelativeAngle: {item.RelativeAngle * 360.0 / (Math.PI * 2.0):000.0}");
    Console.Write($" BPM: {item.Bpm:0000}");
    Console.Write($" RealBPM: {item.Bpm / item.RelativeAngle * 360.0 / (Math.PI * 2.0) / 180:0000}");
    Console.Write($" IsClockWise: {item.IsClockWise}");
    Console.Write($" Actions:{item.Actions.Count:D3}");
    Console.Write(" { ");
    foreach (var action in item.Actions)
    {
        var eventType = action.EventType;
        if (eventType == EventType.NotAvailable)
            continue;
        Console.Write($"{eventType} ");
        if (eventType == EventType.SetSpeed)
        {
            var speedInfo = action as SetSpeed;
            Console.Write(" ");
            Console.Write(speedInfo?.SpeedType);
            Console.Write(" ");
            Console.Write(speedInfo?.Value);
        }
    }
    Console.Write("}");
    Console.WriteLine();
}
Console.WriteLine("--------------Floors--------------");
Console.WriteLine(adofaiLevel.RawData["pathData"]?.ToString());

for (var j = adofaiLevel.Floors.Count - 1; j >= 0 ; j--)
{
    Console.WriteLine(adofaiLevel.Floors[j].Value.Index);
}

const float bpm = 400f;
var i = 0;
foreach (var floor in adofaiLevel.Floors)
{
    if (floor.IsMidSpin)
        i -= 1;
    if (i % 4 == 1 || i % 4 == 2)
        floor.Bpm = (float)(floor.RelativeAngle * 360.0 / (Math.PI * 2.0) / 180 * (bpm / 2));
    else
        floor.Bpm = (float)(floor.RelativeAngle * 360.0 / (Math.PI * 2.0) / 180 * bpm);
    i += 1;
}

adofaiLevel.SaveLevel();
