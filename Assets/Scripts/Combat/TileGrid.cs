using System.Collections.Generic;
using UnityEngine;

namespace DragonSigil.Combat
{
    /// <summary>
    /// Holds the tile grid for a single stage and resolves adjacency,
    /// including which Platform tiles count as Connecting Tiles (GDD 4.1).
    /// </summary>
    public class TileGrid : MonoBehaviour
    {
        [SerializeField] private int width;
        [SerializeField] private int height;

        private Tile[,] _tiles;

        public int Width => width;
        public int Height => height;

        public void Initialize(int newWidth, int newHeight, System.Func<Vector2Int, TileType> typeProvider)
        {
            width = newWidth;
            height = newHeight;
            _tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var coord = new Vector2Int(x, y);
                    _tiles[x, y] = new Tile(coord, typeProvider(coord));
                }
            }
        }

        public Tile GetTile(Vector2Int coordinate)
        {
            if (coordinate.x < 0 || coordinate.x >= width || coordinate.y < 0 || coordinate.y >= height)
            {
                return null;
            }
            return _tiles[coordinate.x, coordinate.y];
        }

        /// <summary>
        /// Returns the four orthogonal neighbors of a tile (used for Connecting
        /// Tile checks and for enemy melee-reach resolution).
        /// </summary>
        public IEnumerable<Tile> GetOrthogonalNeighbors(Tile origin)
        {
            var offsets = new[]
            {
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1)
            };

            foreach (var offset in offsets)
            {
                var neighbor = GetTile(origin.Coordinate + offset);
                if (neighbor != null)
                {
                    yield return neighbor;
                }
            }
        }

        /// <summary>
        /// True if a Ground tile with an enemy on it is adjacent to the given
        /// Platform tile — i.e. the tile is eligible to be authored as
        /// Connecting. Level design calls this at stage-authoring time via
        /// Tile.MarkAsConnecting(), not per-frame at runtime.
        /// </summary>
        public bool HasAdjacentGroundTile(Tile platformTile)
        {
            foreach (var neighbor in GetOrthogonalNeighbors(platformTile))
            {
                if (neighbor.IsGround)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
