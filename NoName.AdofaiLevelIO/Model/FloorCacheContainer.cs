using System;
using System.Collections.Generic;

namespace NoName.AdofaiLevelIO.Model
{
    internal class FloorCacheContainer : List<FloorCache>
    {
        public void Caching(int index, FloorCache value)
        {
            if (index < Count)
            {
                var floorCache = this[index];

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
            else if (Count == index) 
                Add(value);
            else
            {
                for (var i = Count; i < index; i++)
                    Add(new FloorCache());
                Add(value);
            }
        }

        public bool TryGetValue(int index, out FloorCache floorCache)
        {
            if (index < Count)
            {
                floorCache = this[index];
                return true;
            }

            floorCache = null;
            return false;
        }

        public void AddListener(Floor floor) => floor.OnFloorChanged += Floor_OnFloorChanged;

        public void RemoveListener(Floor floor) => floor.OnFloorChanged -= Floor_OnFloorChanged;

        private void Floor_OnFloorChanged(int floorIndex, CacheValue cacheValue)
        {
            var count = Count;
            switch (cacheValue)
            {
                case CacheValue.Direction:
                    for (var i = floorIndex; i < count; i++)
                        this[i].Direction = null;
                    break;
                case CacheValue.EntryAngle:
                    for (var i = floorIndex; i < count; i++)
                        this[i].EntryAngle = null;
                    break;
                case CacheValue.ExitAngle:
                    for (var i = floorIndex; i < count; i++)
                        this[i].ExitAngle = null;
                    break;
                case CacheValue.Bpm:
                    for (var i = floorIndex; i < count; i++)
                        this[i].Bpm = null;
                    break;
                case CacheValue.IsClockWise:
                    for (var i = floorIndex; i < count; i++)
                        this[i].IsClockWise = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cacheValue), cacheValue, null);
            }
        }
    }
}