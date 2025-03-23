using System;

namespace SpaceAce.Gameplay.Battle
{
    public struct BattleStatisticsCache
    {
        public int ShotsFired { get; set; }
        public int Hits { get; set; }

        public float DamageDealt { get; set; }
        public float DamageLost { get; set; }

        public float DamageReceived { get; set; }
        public float DamageAvoided { get; set; }

        public int MeteorsEncountered { get; set; }
        public int MeteorsDestroyed { get; set; }

        public int WrecksEncountered { get; set; }
        public int WrecksDestroyed { get; set; }

        public float ExperienceEarned { get; set; }
        public float ExperienceLost { get; set; }

        public float CreditsEarned { get; set; }

        public float DurabilityGained { get; set; }
        public float DurabilityLost { get; set; }

        public int EnemiesDefeated { get; set; }
        public int EliteEnemiesDefeated { get; set; }
        public int BossesDefeated { get; set; }

        public int ItemsUsed { get; set; }

        public readonly BattleStatistics GetSnapshot(TimeSpan runTime) =>
            new(this, DateTime.UtcNow, runTime);

        public void Reset()
        {
            ShotsFired = 0;
            Hits = 0;

            DamageDealt = 0f;
            DamageLost = 0f;

            DamageReceived = 0f;
            DamageAvoided = 0f;

            MeteorsEncountered = 0;
            MeteorsDestroyed = 0;

            WrecksEncountered = 0;
            WrecksDestroyed = 0;

            ExperienceEarned = 0f;
            ExperienceLost = 0f;

            CreditsEarned = 0f;

            DurabilityGained = 0f;
            DurabilityLost = 0f;

            EnemiesDefeated = 0;
            EliteEnemiesDefeated = 0;
            BossesDefeated = 0;

            ItemsUsed = 0;
        }
    }
}