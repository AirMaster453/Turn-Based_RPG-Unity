using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PsychesBound
{
    [Serializable]
    public class Level
    {
        public const int MaxLevel = 100;

        public static long GetExpFromLevel(int Level)
        {
            long exp = 0L;

            for(int i = 1; i < Level; i++)
            {
                exp += ToNextLevel(Level);
            }

            return exp;
        }

        public static long ToNextLevel(int Level)
        {
            return Level <= MaxLevel ? ((long)Mathf.Pow(Level, 4) + 10L * (long)Mathf.Pow(Level, 3) + 37L * (long)Mathf.Pow(Level, 2) + 56L * Level - 96L) / 16L : 0;
        }

        public static int GetLevelFromExp(long exp)
        {
            for(int i = 1; i < MaxLevel; i++)
            {
                var nextLvl = ToNextLevel(i);
                if (exp < nextLvl)
                {
                    return i;
                }

                exp -= nextLvl;
            }

            return MaxLevel;
        }

        [SerializeField, Range(1, MaxLevel)]
        private int _level = 1;

        private long _exp;
    }
}