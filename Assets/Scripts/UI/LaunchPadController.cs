using UnityEngine;
using DragonSigil.Characters;
using DragonSigil.Combat;

namespace DragonSigil.UI
{
    /// <summary>
    /// Resolves a drag-and-drop screen point to a tile and deploys a
    /// champion prefab there, shared by every ChampionLaunchPadCard so the
    /// screen-to-tile math lives in exactly one place.
    /// </summary>
    public class LaunchPadController : MonoBehaviour
    {
        [SerializeField] private TileGrid tileGrid;

        /// <summary>
        /// Attempts to deploy championPrefab at whatever tile screenPoint
        /// lands on. No-ops (returns false) if the point is off-grid or the
        /// tile is already occupied — callers don't need to pre-validate.
        /// </summary>
        public bool TryPlaceChampion(GameObject championPrefab, Vector2 screenPoint)
        {
            var camera = Camera.main;
            if (camera == null || tileGrid == null || championPrefab == null)
            {
                return false;
            }

            var groundPlane = new Plane(Vector3.up, tileGrid.transform.position);
            var ray = camera.ScreenPointToRay(screenPoint);
            if (!groundPlane.Raycast(ray, out float distance))
            {
                return false;
            }

            var worldPoint = ray.GetPoint(distance);
            var coordinate = tileGrid.WorldToNearestCoordinate(worldPoint);
            var tile = tileGrid.GetTile(coordinate);
            if (tile == null || tile.IsOccupied)
            {
                return false;
            }

            var instance = Instantiate(championPrefab, tileGrid.GetWorldPosition(coordinate), Quaternion.identity);
            var champion = instance.GetComponent<Champion>();
            champion.Deploy(tile);
            return true;
        }
    }
}
