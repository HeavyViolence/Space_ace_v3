using System;

using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace SpaceAce.Main.Localization
{
    [DisplayName("SI formatter")]
    public sealed class SIFormatter : FormatterBase
    {
        public override string[] DefaultNames => new string[] { "SI", "Si", "si" };

        public override bool TryEvaluateFormat(IFormattingInfo info)
        {
            if (info is null)
            {
                throw new ArgumentNullException();
            }

            if (info.CurrentValue is float f)
            {
                if (f == float.PositiveInfinity)
                {
                    info.Write("Inf");
                    return true;
                }

                if (f >= 0f)
                {
                    if (f < 1f)
                    {
                        if (f > 1e-3f)
                        {
                            info.Write($"{f: n2}");
                            return true;
                        }

                        if (f > 1e-6f)
                        {
                            info.Write($"{f / 1e-3f: n2}m");
                            return true;
                        }

                        if (f > 1e-9f)
                        {
                            info.Write($"{f / 1e-6f: n2}\u03bc");
                            return true;
                        }

                        if (f > 1e-12f)
                        {
                            info.Write($"{f / 1e-9f: n2}n");
                            return true;
                        }

                        if (f > 1e-15f)
                        {
                            info.Write($"{f / 1e-12f: n2}p");
                            return true;
                        }

                        if (f > 1e-18f)
                        {
                            info.Write($"{f / 1e-15f: n2}f");
                            return true;
                        }

                        if (f > 1e-21f)
                        {
                            info.Write($"{f / 1e-18f: n2}a");
                            return true;
                        }

                        if (f > 1e-24f)
                        {
                            info.Write($"{f / 1e-21f: n2}z");
                            return true;
                        }

                        if (f > 1e-27f)
                        {
                            info.Write($"{f / 1e-24f: n2}y");
                            return true;
                        }

                        if (f > 1e-30f)
                        {
                            info.Write($"{f / 1e-27f: n2}r");
                            return true;
                        }

                        info.Write($"{f / 1e-30f: n2}q");
                        return true;
                    }

                    if (f >= 1f)
                    {
                        if (f < 1e+3f)
                        {
                            info.Write($"{f: n2}");
                            return true;
                        }

                        if (f < 1e+6f)
                        {
                            info.Write($"{f / 1e+3f: n2}k");
                            return true;
                        }

                        if (f < 1e9f)
                        {
                            info.Write($"{f / 1e+6f: n2}M");
                            return true;
                        }

                        if (f < 1e+12f)
                        {
                            info.Write($"{f / 1e+9f: n2}G");
                            return true;
                        }

                        if (f < 1e+15f)
                        {
                            info.Write($"{f / 1e+12f: n2}T");
                            return true;
                        }

                        if (f < 1e+18f)
                        {
                            info.Write($"{f / 1e+15f: n2}P");
                            return true;
                        }

                        if (f < 1e+21f)
                        {
                            info.Write($"{f / 1e+18f: n2}E");
                            return true;
                        }

                        if (f < 1e+24f)
                        {
                            info.Write($"{f / 1e+21f: n2}Z");
                            return true;
                        }

                        if (f < 1e+27f)
                        {
                            info.Write($"{f / 1e+24f: n2}Y");
                            return true;
                        }

                        if (f < 1e+30f)
                        {
                            info.Write($"{f / 1e+27f: n2}R");
                            return true;
                        }

                        info.Write($"{f / 1e+30f: n2}Q");
                        return true;
                    }
                }

                return false;
            }

            if (info.CurrentValue is int i)
            {
                if (i == -1)
                {
                    info.Write("Inf");
                    return true;
                }

                if (i >= 0)
                {
                    if (i < 1_000)
                    {
                        info.Write($"{i}");
                        return true;
                    }

                    if (i < 1_000_000)
                    {
                        info.Write($"{i / 1e+3f: n2}k");
                        return true;
                    }

                    if (i < 1_000_000_000)
                    {
                        info.Write($"{i / 1e+6f: n2}M");
                        return true;
                    }

                    info.Write($"{i / 1e+9f: n2}G");
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}