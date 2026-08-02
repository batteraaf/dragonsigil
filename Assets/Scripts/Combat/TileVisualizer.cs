using UnityEngine;

namespace DragonSigil.Combat
{
    /// <summary>
    /// Spawns one flat tile mesh per TileGrid cell, keeping the visual grid
    /// always in sync with the logical grid (GDD 4.1) instead of requiring
    /// hand-placed environment pieces per tile.
    /// </summary>
    public class TileVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject tileMeshPrefab;

        [Tooltip("Alternating tile colors so individual tiles read clearly as a grid, checkerboard-style by (x + y) parity.")]
        [SerializeField] private Material lightTileMaterial;
        [SerializeField] private Material darkTileMaterial;

        public void BuildVisuals(TileGrid grid)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    var coordinate = new Vector2Int(x, y);
                    var instance = Instantiate(tileMeshPrefab, grid.GetWorldPosition(coordinate), tileMeshPrefab.transform.rotation, transform);
                    instance.name = $"Tile_{x}_{y}";

                    var scale = instance.transform.localScale;
                    scale.x = grid.TileSize;
                    scale.y = grid.TileSize;
                    instance.transform.localScale = scale;

                    bool isLight = (x + y) % 2 == 0;
                    instance.GetComponent<MeshRenderer>().sharedMaterial = isLight ? lightTileMaterial : darkTileMaterial;
                }
            }
        }
    }
}
