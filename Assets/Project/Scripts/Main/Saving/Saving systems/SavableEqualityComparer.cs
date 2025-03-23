using System;
using System.Collections.Generic;

namespace SpaceAce.Main.Saving
{
    public sealed class SavableEqualityComparer : IEqualityComparer<ISavable>
    {
        public bool Equals(ISavable x, ISavable y)
        {
            if (x is null || y is null)
            {
                return false;
            }

            return x.StateName == y.StateName;
        }

        public int GetHashCode(ISavable obj) =>
            HashCode.Combine(obj.StateName);
    }
}