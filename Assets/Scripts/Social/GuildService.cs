using System;
using System.Collections.Generic;

namespace DragonSigil.Social
{
    /// <summary>
    /// Player Guild — the social layer independent of Dragon Orders (GDD 6.3).
    /// Enforces the confirmed 50-member cap.
    /// </summary>
    public class GuildService
    {
        public const int MaxMembers = 50;

        private readonly List<string> _memberIds = new List<string>();

        public IReadOnlyList<string> Members => _memberIds;
        public bool IsFull => _memberIds.Count >= MaxMembers;

        public bool TryAddMember(string playerId)
        {
            if (IsFull)
            {
                return false;
            }

            if (_memberIds.Contains(playerId))
            {
                return false;
            }

            _memberIds.Add(playerId);
            return true;
        }

        public bool RemoveMember(string playerId)
        {
            return _memberIds.Remove(playerId);
        }
    }
}
