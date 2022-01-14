using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PsychesBound
{
    public abstract class Ability : ScriptableObject
    {
        [SerializeField]
        protected string abilityName;

        public string AbilityName => abilityName;

        [SerializeField]
        protected string description;

        public string Description => description;

        //this is subject to change
        public abstract HashSet<ITileObject> GetTargets(IBattler user, BattleField field);

        //public abstract List<Unit> SearchUnits(IBattler user, BattleField field);

        public abstract UniTask OnBattleStart(IBattler unit, BattleField field  );
        public abstract UniTask OnBattleEnd(IBattler unit, BattleField field);
    }
}