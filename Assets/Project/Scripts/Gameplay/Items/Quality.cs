using System;
using System.Collections.Generic;

namespace SpaceAce.Gameplay.Items
{
    public sealed record Quality : IComparable<Quality>, IComparer<Quality>
    {
        public Tier Tier { get; }
        public float Grade { get; }

        public Quality(Tier tier, float grade)
        {
            Tier = tier;
            Grade = grade;
        }

        #region interfaces

        public int CompareTo(Quality other)
        {
            if (other is null)
            {
                return 1;
            }

            if (Tier == other.Tier)
            {
                if (Grade < other.Grade)
                {
                    return -1;
                }

                if (Grade > other.Grade)
                {
                    return 1;
                }

                return 0;
            }
            else
            {
                if (Tier < other.Tier)
                {
                    return -1;
                }

                if (Tier > other.Tier)
                {
                    return 1;
                }

                return 0;
            }
        }

        public int Compare(Quality x, Quality y)
        {
            if (x is null || y is null)
            {
                throw new ArgumentNullException();
            }

            if (x.Tier == y.Tier)
            {
                if (x.Grade < y.Grade)
                {
                    return -1;
                }

                if (x.Grade > y.Grade)
                {
                    return 1;
                }

                return 0;
            }
            else
            {
                if (x.Tier < y.Tier)
                {
                    return -1;
                }

                if (x.Tier > y.Tier)
                {
                    return 1;
                }

                return 0;
            }
        }

        public static bool operator >(Quality x, Quality y)
        {
            if (x is null || y is null)
            {
                throw new ArgumentNullException();
            }

            if (x.Tier == y.Tier)
            {
                if (x.Grade < y.Grade)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (x.Tier < y.Tier)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static bool operator <(Quality x, Quality y)
        {
            if (x is null || y is null)
            {
                throw new ArgumentNullException();
            }

            if (x.Tier == y.Tier)
            {
                if (x.Grade < y.Grade)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (x.Tier < y.Tier)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion
    }
}