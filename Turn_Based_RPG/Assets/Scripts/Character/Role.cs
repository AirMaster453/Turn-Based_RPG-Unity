using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace PsychesBound
{
    [Serializable]
    public class Role : IEquip, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private RoleBonus[] roleBonus = new RoleBonus[Enum.GetNames(typeof(StatType)).Length];

        [SerializeField]
        private Level _level = new Level(1);

        public Level Level => _level;


        private StatFormulaTree formulaTree;

        public RoleBonus GetBonus(StatType type)
        {
            try
            {
                var bonus = roleBonus[(int)type];

                return bonus;
            }
            catch
            {
                roleBonus = new RoleBonus[Enum.GetNames(typeof(StatType)).Length];
                if (roleType)
                {
                    roleBonus[(int)type] = new RoleBonus(roleType.GetStat(type), this);
                }
                else
                {
                    roleBonus[(int)type] = new RoleBonus(this);

                    
                }
                var bonus = roleBonus[(int)type];

                return bonus;
            }
        }

        public IModifier GetModifier(StatType type)
        {
            return GetBonus(type);
        }

        public IModifier GetModifier(SecondaryType type)
        {
            return new FlatModifier(roleType.GetStat(type), this);
        }

        //public IModifier Distance => new FlatModifier(roleType.Distance, this);
        //public IModifier JumpHeight => new FlatModifier(roleType.JumpHeight, this);
        //public IModifier HitRate => new FlatModifier(roleType.HitRate, this);
        //public IModifier CritRate => new FlatModifier(roleType.CritRate, this);
        //public IModifier Evasion => new FlatModifier(roleType.Evasion, this);
        //public IModifier Speed => new FlatModifier(roleType.Speed, this);

        public void InitialBaseValues(StatFormulaTree tree)
        {
            formulaTree = tree;
            StatType t = 0;
            for(int i = 0; i < roleBonus.Length; i++, t++)
            {
                //GetBonus(t).InitialValue = roleType.GetStat(t);
                roleBonus[i].BaseValue = (int)tree.GetFormula(t).Calculate(GetBonus(t), _level.CurrentLevel);
            }
        }


        [SerializeField]
        private RoleType roleType;

        public RoleType RoleType => roleType;


        public Role() : this(null)
        {
            
        }

        public Role(RoleType role)
        {
            roleBonus = new RoleBonus[Enum.GetNames(typeof(StatType)).Length];

            roleType = role;

            for (int i = 0; i < roleBonus.Length; i++)
            {
                if (roleType)
                {
                    roleBonus[i] = new RoleBonus(roleType.GetStat((StatType)i), this);
                }
                else
                {
                    roleBonus[i] = new RoleBonus(this);
                }
            }

            _level.OnLevelChanged += OnLevelChange;
        }

        


        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            try
            {
                for (int i = 0; i < roleBonus.Length; i++)
                {
                    roleBonus[i].InitialValue = roleType.GetStat((StatType)i);
                    roleBonus[i].Source = this;

                }
            }
            catch
            {
                //roleBonus = new RoleBonus[Enum.GetNames(typeof(StatType)).Length];
            }
        }

        private void OnLevelChange(int level, int delta)
        {
            if (formulaTree)
                return;
            StatType t = 0;
            for (int i = 0; i < roleBonus.Length; i++, t++)
            {
                roleBonus[i].BaseValue = (int)formulaTree.GetFormula(t).Calculate(GetBonus(t), level);
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            
        }
    }
}