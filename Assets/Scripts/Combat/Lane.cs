using System.Collections.Generic;

namespace DragonSigil.Combat
{
    /// <summary>
    /// An ordered sequence of tiles connecting one RuinPortal to one
    /// SigilPortal (GDD 4.1). A stage can have multiple Lanes active at
    /// once on higher-difficulty levels.
    /// </summary>
    [System.Serializable]
    public class Lane
    {
        public string LaneId { get; }
        public RuinPortal Source { get; }
        public SigilPortal Destination { get; }
        public IReadOnlyList<Tile> Path { get; }

        public Lane(string laneId, RuinPortal source, SigilPortal destination, IReadOnlyList<Tile> path)
        {
            LaneId = laneId;
            Source = source;
            Destination = destination;
            Path = path;
        }
    }
}
