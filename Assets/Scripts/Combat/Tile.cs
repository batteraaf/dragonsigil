using UnityEngine;
using DragonSigil.Characters;

namespace DragonSigil.Combat
{
    /// <summary>
    /// A single cell in a stage's tile grid (GDD 4.1). Stages are laid out on
    /// a grid; enemies advance tile-by-tile along Ground tiles, and Platform
    /// tiles sit elevated alongside the path.
    /// </summary>
    [System.Serializable]
    public class Tile
    {
        public Vector2Int Coordinate { get; }
        public TileType Type { get; private set; }

        /// <summary>The champion currently deployed on this tile, if any.</summary>
        public Champion Occupant { get; private set; }

        public Tile(Vector2Int coordinate, TileType type)
        {
            Coordinate = coordinate;
            Type = type;
        }

        public bool IsConnecting => Type == TileType.PlatformConnecting;
        public bool IsPlatform => Type == TileType.Platform || Type == TileType.PlatformConnecting;
        public bool IsGround => Type == TileType.Ground;
        public bool IsOccupied => Occupant != null;

        public void SetOccupant(Champion champion)
        {
            Occupant = champion;
        }

        public void ClearOccupant()
        {
            Occupant = null;
        }

        /// <summary>
        /// Level design marks specific Platform tiles as Connecting per stage
        /// (GDD 4.1). Call this from stage setup/authoring tools rather than
        /// at runtime during combat.
        /// </summary>
        public void MarkAsConnecting()
        {
            if (Type == TileType.Platform)
            {
                Type = TileType.PlatformConnecting;
            }
        }
    }
}
