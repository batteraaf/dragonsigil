using System;
using System.Collections.Generic;
using DragonSigil.Characters;

namespace DragonSigil.Progression
{
    /// <summary>Banner categories (GDD 7.2).</summary>
    public enum BannerType
    {
        Standard,
        FeaturedOrder,
        LimitedEdition
    }

    [Serializable]
    public class Banner
    {
        public string BannerId;
        public BannerType Type;
        public Currency Cost;
        public int PityThreshold; // guaranteed Legendary+ within this many pulls
    }

    /// <summary>
    /// Server-authoritative-in-spirit gacha roller (GDD 7.2/10.3). Actual
    /// randomness and pity state MUST be validated server-side in production
    /// to avoid client-side manipulation — this class models the logic the
    /// server would run.
    /// </summary>
    public class GachaService
    {
        private readonly Dictionary<string, int> _pityCounters = new Dictionary<string, int>();

        /// <summary>
        /// Standard and Featured/Order banners share Dragon Crystal pity;
        /// Limited-Edition banners track Prismatic Scale pity separately so
        /// LE pulls never cannibalize core progress (GDD 7.1/7.2).
        /// </summary>
        public Rarity Roll(Banner banner, Func<double> randomSource)
        {
            var pityKey = PityKeyFor(banner);
            int pulls = _pityCounters.TryGetValue(pityKey, out var count) ? count : 0;
            pulls++;

            Rarity result;
            if (pulls >= banner.PityThreshold)
            {
                result = Rarity.Legendary;
                pulls = 0;
            }
            else
            {
                result = RollWeightedRarity(randomSource());
                if (result >= Rarity.Legendary)
                {
                    pulls = 0;
                }
            }

            _pityCounters[pityKey] = pulls;
            return result;
        }

        private string PityKeyFor(Banner banner)
        {
            // Limited-Edition banners get their own pity track regardless of
            // banner id, per GDD 7.1.
            return banner.Type == BannerType.LimitedEdition ? "LE" : "STANDARD";
        }

        private Rarity RollWeightedRarity(double roll)
        {
            // Placeholder weights — final odds are a live-ops/business
            // decision (GDD 7.3/11), not hardcoded here.
            if (roll < 0.60) return Rarity.Common;
            if (roll < 0.85) return Rarity.Rare;
            if (roll < 0.96) return Rarity.Epic;
            if (roll < 0.995) return Rarity.Legendary;
            return Rarity.Mythic;
        }
    }
}
