using Newtonsoft.Json;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace SpaceAce.Gameplay.Levels
{
    public sealed record LevelStatistics : IComparable<LevelStatistics>, IComparer<LevelStatistics>
    {
        public static LevelStatistics Default => new(0, 0, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 0f, 0f, 0f, 0f,
                                                     0f, 0, 0, 0, DateTime.MinValue, TimeSpan.Zero);

        public int ShotsFired { get; }
        public int Hits { get; }

        [JsonIgnore]
        public int Misses => ShotsFired - Hits;

        [JsonIgnore]
        public float Accuracy => ShotsFired == 0 ? 0f : (float)Hits / ShotsFired;

        [JsonIgnore]
        public float AccuracyPercent => Accuracy * 100f;

        public float DamageDealt { get; }
        public float DamageLost { get; }

        [JsonIgnore]
        public float DamageOutput => DamageDealt + DamageLost;

        [JsonIgnore]
        public float DamageMastery =>
            DamageDealt == 0f ? 0f : DamageDealt / DamageOutput;

        [JsonIgnore]
        public float DamageMasteryPercent => DamageMastery * 100f;

        public float DamageReceived { get; }
        public float DamageAvoided { get; }

        [JsonIgnore]
        public float DamageInput => DamageReceived + DamageAvoided;

        [JsonIgnore]
        public float DodgingMastery =>
            DamageReceived == 0f ? 1f : 1f - (DamageReceived - DamageAvoided) / DamageReceived;

        [JsonIgnore]
        public float DodgingMasteryPercent => DodgingMastery * 100f;

        public int MeteorsEncountered { get; }
        public int MeteorsDestroyed { get; }

        [JsonIgnore]
        public int MeteorsMissed =>
            MeteorsEncountered - MeteorsDestroyed;

        [JsonIgnore]
        public float MeteorMastery =>
            MeteorsEncountered == 0 ? 1f : (float)MeteorsDestroyed / MeteorsEncountered;

        [JsonIgnore]
        public float MeteorMasteryPercent => MeteorMastery * 100f;

        public int WrecksEncountered { get; }
        public int WrecksDestroyed { get; }

        [JsonIgnore]
        public int WrecksMissed =>
            WrecksEncountered - WrecksDestroyed;

        [JsonIgnore]
        public float WreckMastery =>
            WrecksEncountered == 0 ? 1f : (float)WrecksDestroyed / WrecksEncountered;

        [JsonIgnore]
        public float WreckMasteryPercent => WreckMastery * 100f;

        public float ExperienceEarned { get; }
        public float ExperienceLost { get; }

        [JsonIgnore]
        public float ExperienceTotal => ExperienceEarned + ExperienceLost;

        [JsonIgnore]
        public float ExperienceMastery =>
            ExperienceEarned == 0f ? 0f : (ExperienceEarned - ExperienceLost) / ExperienceEarned;

        [JsonIgnore]
        public float ExperienceMasteryPercent => ExperienceMastery * 100f;

        [JsonIgnore]
        public float MasteryTotal =>
            Accuracy * DamageMastery * DodgingMastery * MeteorMastery * WreckMastery * ExperienceMastery;

        [JsonIgnore]
        public float MasteryTotalPercent => MasteryTotal * 100f;

        public float CreditsEarned { get; }

        public float DurabilityGained { get; }
        public float DurabilityLost { get; }

        public int EnemiesDefeated { get; }
        public int BossesDefeated { get; }

        public int ItemsUsed { get; }

        public DateTime Date { get; }
        public TimeSpan RunTime { get; }

        public LevelStatistics(int shotsFired,
                               int hits,
                               float damageDealt,
                               float damageLost,
                               float damageReceived,
                               float damageAvoided,
                               int meteorsEncountered,
                               int meteorsDestroyed,
                               int wrecksEncountered,
                               int wrecksDestroyed,
                               float experienceEarned,
                               float experienceLost,
                               float creditsEarned,
                               float durabilityGained,
                               float durabilityLost,
                               int enemiesDefeated,
                               int bossesDefeated,
                               int itemsUsed,
                               DateTime date,
                               TimeSpan runTime)
        {
            ShotsFired = Mathf.Clamp(shotsFired, 0, int.MaxValue);
            Hits = Mathf.Clamp(hits, 0, ShotsFired);

            DamageDealt = Mathf.Clamp(damageDealt, 0f, float.MaxValue);
            DamageLost = Mathf.Clamp(damageLost, 0f, float.MaxValue);

            DamageReceived = Mathf.Clamp(damageReceived, 0f, float.MaxValue);
            DamageAvoided = Mathf.Clamp(damageAvoided, 0f, DamageReceived);

            MeteorsEncountered = Mathf.Clamp(meteorsEncountered, 0, int.MaxValue);
            MeteorsDestroyed = Mathf.Clamp(meteorsDestroyed, 0, MeteorsEncountered);

            WrecksEncountered = Mathf.Clamp(wrecksEncountered, 0, int.MaxValue);
            WrecksDestroyed = Mathf.Clamp(wrecksDestroyed, 0, WrecksEncountered);

            ExperienceEarned = Mathf.Clamp(experienceEarned, 0f, float.MaxValue);
            ExperienceLost = Mathf.Clamp(experienceLost, 0f, float.MaxValue);

            CreditsEarned = Mathf.Clamp(creditsEarned, 0f, float.MaxValue);

            RunTime = runTime;

            DurabilityGained = Mathf.Clamp(durabilityGained, 0f, float.MaxValue);
            DurabilityLost = Mathf.Clamp(durabilityLost, 0f, float.MaxValue);

            EnemiesDefeated = Mathf.Clamp(enemiesDefeated, 0, int.MaxValue);
            BossesDefeated = Mathf.Clamp(bossesDefeated, 0, int.MaxValue);

            ItemsUsed = Mathf.Clamp(itemsUsed, 0, int.MaxValue);

            Date = date;
        }

        public LevelStatistics(LevelStatisticsCache cache, DateTime date, TimeSpan runTime)
        {
            ShotsFired = Mathf.Clamp(cache.ShotsFired, 0, int.MaxValue);
            Hits = Mathf.Clamp(cache.Hits, 0, ShotsFired);

            DamageDealt = Mathf.Clamp(cache.DamageDealt, 0f, float.MaxValue);
            DamageLost = Mathf.Clamp(cache.DamageLost, 0f, float.MaxValue);

            DamageReceived = Mathf.Clamp(cache.DamageReceived, 0f, float.MaxValue);
            DamageAvoided = Mathf.Clamp(cache.DamageAvoided, 0f, DamageReceived);

            MeteorsEncountered = Mathf.Clamp(cache.MeteorsEncountered, 0, int.MaxValue);
            MeteorsDestroyed = Mathf.Clamp(cache.MeteorsDestroyed, 0, MeteorsEncountered);

            WrecksEncountered = Mathf.Clamp(cache.WrecksEncountered, 0, int.MaxValue);
            WrecksDestroyed = Mathf.Clamp(cache.WrecksDestroyed, 0, WrecksEncountered);

            ExperienceEarned = Mathf.Clamp(cache.ExperienceEarned, 0f, float.MaxValue);
            ExperienceLost = Mathf.Clamp(cache.ExperienceLost, 0f, float.MaxValue);

            CreditsEarned = Mathf.Clamp(cache.CreditsEarned, 0f, float.MaxValue);

            DurabilityLost = Mathf.Clamp(cache.DurabilityLost, 0f, float.MaxValue);
            DurabilityGained = Mathf.Clamp(cache.DurabilityGained, 0f, float.MaxValue);

            EnemiesDefeated = Mathf.Clamp(cache.EnemiesDefeated, 0, int.MaxValue);
            ItemsUsed = Mathf.Clamp(cache.ItemsUsed, 0, int.MaxValue);

            Date = date;
            RunTime = runTime;
        }

        #region interfaces

        public int CompareTo(LevelStatistics other)
        {
            if (other is null)
            {
                throw new ArgumentNullException();
            }

            if (MasteryTotal > other.MasteryTotal)
            {
                return 1;
            }

            if (MasteryTotal < other.MasteryTotal)
            {
                return -1;
            }

            return 0;
        }

        public int Compare(LevelStatistics x, LevelStatistics y)
        {
            if (x is null || y is null)
            {
                throw new ArgumentNullException();
            }

            if (x.MasteryTotal > y.MasteryTotal)
            {
                return 1;
            }

            if (x.MasteryTotal < y.MasteryTotal)
            {
                return -1;
            }

            return 0;
        }

        public static bool operator >(LevelStatistics x, LevelStatistics y) =>
            x.MasteryTotal > y.MasteryTotal;

        public static bool operator <(LevelStatistics x, LevelStatistics y) =>
            x.MasteryTotal < y.MasteryTotal;

        public static bool operator >=(LevelStatistics x, LevelStatistics y) =>
            x.MasteryTotal >= y.MasteryTotal;

        public static bool operator <=(LevelStatistics x, LevelStatistics y) =>
            x.MasteryTotal <= y.MasteryTotal;

        #endregion
    }
}