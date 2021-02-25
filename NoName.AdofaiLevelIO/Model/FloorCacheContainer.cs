using System.Collections.Generic;

namespace NoName.AdofaiLevelIO.Model
{
    public class FloorCacheContainer : Dictionary<int, FloorCache>
    {
        public void Caching(int key, FloorCache value)
        {
            if (TryGetValue(key, out var floorCache))
            {
                if (value.Direction != null)
                    floorCache.Direction = value.Direction;
                if (value.Bpm != null)
                    floorCache.Bpm = value.Bpm;
                if (value.EntryAngle != null)
                    floorCache.EntryAngle = value.EntryAngle;
                if (value.ExitAngle != null)
                    floorCache.ExitAngle = value.ExitAngle;
                if (value.IsClockWise != null)
                    floorCache.IsClockWise = value.IsClockWise;
            }
            else
                Add(key, value);
        }
    }
}