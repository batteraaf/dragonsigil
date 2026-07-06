using System.Collections.Generic;
using UnityEngine;

namespace DragonSigil.Combat
{
    /// <summary>
    /// Data-driven skill range, defined in tiles relative to the caster
    /// rather than a raw radius (GDD 4.4). Example: a Warden's heal might be
    /// forward = 3, side = 1, behind = 1 — a forward-weighted cross.
    /// Authored per skill (not per role) so kits can feel distinct.
    /// </summary>
    [CreateAssetMenu(fileName = "NewSkillRangePattern", menuName = "DragonSigil/Skill Range Pattern")]
    public class SkillRangePattern : ScriptableObject
    {
        [Tooltip("How many tiles ahead of the caster (toward the enemy path) the skill affects.")]
        [SerializeField] private int forwardTiles;

        [Tooltip("How many tiles to the left and right of the caster's tile the skill affects.")]
        [SerializeField] private int sideTiles;

        [Tooltip("How many tiles behind the caster the skill affects.")]
        [SerializeField] private int behindTiles;

        public int ForwardTiles => forwardTiles;
        public int SideTiles => sideTiles;
        public int BehindTiles => behindTiles;

        /// <summary>
        /// Resolves the set of tiles this pattern affects for a caster on
        /// <paramref name="origin"/> facing <paramref name="forwardDirection"/>
        /// (a unit vector, e.g. (0,1) if the lane runs along +Y).
        /// </summary>
        public List<Tile> ResolveAffectedTiles(TileGrid grid, Tile origin, Vector2Int forwardDirection)
        {
            var results = new List<Tile>();
            if (grid == null || origin == null)
            {
                return results;
            }

            var sideDirection = new Vector2Int(-forwardDirection.y, forwardDirection.x);

            for (int f = -behindTiles; f <= forwardTiles; f++)
            {
                // Skip the forward/behind axis itself when f == 0 so side
                // offsets aren't double counted at the caster's own row.
                int sideRange = (f == 0) ? sideTiles : 0;

                for (int s = -sideRange; s <= sideRange; s++)
                {
                    if (f == 0 && s == 0)
                    {
                        continue; // the caster's own tile is not a "target" tile
                    }

                    var offset = forwardDirection * f + sideDirection * s;
                    var tile = grid.GetTile(origin.Coordinate + offset);
                    if (tile != null)
                    {
                        results.Add(tile);
                    }
                }
            }

            return results;
        }
    }
}
