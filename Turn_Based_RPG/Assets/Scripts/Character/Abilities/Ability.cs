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
        public abstract List<Unit> GetTargets(Unit unit, BattleField field);

        public abstract List<Unit> SearchUnits(Unit unit, BattleField field);

        public abstract UniTask OnBattleStart(Unit unit, BattleField field  );
        public abstract UniTask OnBattleEnd(Unit unit, BattleField field);
    }
}