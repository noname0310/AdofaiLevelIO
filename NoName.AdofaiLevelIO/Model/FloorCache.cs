namespace NoName.AdofaiLevelIO.Model
{
    internal class FloorCache
    {
        public char? Direction { get; set; } = null;
        public double? EntryAngle { get; set; } = null;
        public double? ExitAngle { get; set; } = null;
        public float? Bpm { get; set; } = null;
        public bool? IsClockWise { get; set; } = null;
    }

    public enum CacheValue
    {
        Direction,
        EntryAngle,
        ExitAngle,
        Bpm,
        IsClockWise
    }
}