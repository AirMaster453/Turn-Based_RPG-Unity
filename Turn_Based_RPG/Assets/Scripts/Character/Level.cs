using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace PsychesBound
{
    public delegate void LevelChange(int level, int delta);

    [Serializable]
    public class Level
    {
        public const int MaxLevel = 100;

        public static long GetExpFromLevel(int Level)
        {
            //long exp = 0L;

            var firstpass = 0L;
            var secondpass = 0L;

            for (int levelCycle = 1; levelCycle < Level; levelCycle++)
            {
                firstpass += LevelScaling(levelCycle);
                secondpass = firstpass / 4;
            }

            return secondpass;
        }

        public static long LevelScaling(int Level)
        {
            return Level < MaxLevel && Level > 0 ? ((long)Mathf.Pow(Level, 4) + 10L * (long)Mathf.Pow(Level, 3) + 37L * (long)Mathf.Pow(Level, 2) + 56L * Level - 96L) / 16L : 0;
        }

        public static int GetLevelFromExp(long exp)
        {
            var firstpass = 0L;
            var secondpass = 0L;

            for (var levelCycle = 1; levelCycle < MaxLevel; levelCycle++)
            {
                firstpass += LevelScaling(levelCycle);
                secondpass = firstpass / 4;

                if (secondpass > exp)
                {
                    return levelCycle;
                }
            }

            if (secondpass <= exp)
                return MaxLevel;
            return 1;
        }

        [SerializeField, Range(1, MaxLevel)]
        private int _level = 1;

        private long _exp;


        private long _totalExp;

        private bool isDirty = true;

        public event Action<long> OnExperienceGain;

        public event LevelChange OnLevelChanged;


        public int CurrentLevel => _level;

        public void SetLevel(int level)
        {
            isDirty = true;
            _level = Mathf.Clamp(level, 1 , MaxLevel);
        }
        public long GetTotalExp()
        {
            if(isDirty)
            {
                _totalExp = GetExpFromLevel(_level) + _exp;
                isDirty = false;
            }

            return _totalExp;
        }

        public long ToNextLevel()
        {
            return LevelScaling(_level + 1) - _exp;
        }

        public async UniTask GainExperienceAsync(long exp)
        {
            if (_level >= MaxLevel)
                return;
            var newExp = _exp + exp;
            OnExperienceGain?.Invoke(exp);

            var oldLvl = _level;
            var toNext = ToNextLevel();
            bool changed = false;
            isDirty = true;
            while(toNext <= newExp)
            {
                _level++;
                newExp -= toNext;
               
                changed = true;

                if(_level >= MaxLevel)
                {
                    _exp = 0;
                    OnLevelChanged?.Invoke(_level, _level - oldLvl);
                    return;
                }

                //CancellationTokenSource src = new CancellationTokenSource();
                //var task = Task.Run(()=> Task.Yield(), src.Token);
                //GameManager.AddToken(src);
                //await task;
                //GameManager.RemoveToken(src);

                await UniTask.Yield();
            }

            OnExperienceGain?.Invoke(exp);

            if (changed)
                OnLevelChanged?.Invoke(_level, _level - oldLvl);

            _exp = exp;
        }

        public Level() : this(1)
        {
            
        }

        public Level(int level, long exp = 0)
        {
            _level = level;
            _exp = 0;
            _totalExp = GetExpFromLevel(level - 1) + _exp;
            isDirty = true;
        }
    }
}