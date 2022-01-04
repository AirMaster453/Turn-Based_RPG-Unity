using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace PsychesBound
{
    [Serializable]
    public class Role : ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private RoleBonus[] roleBonus = new RoleBonus[Enum.GetNames(typeof(StatType)).Length];


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

        public void InitialBaseValues(StatFormulaTree tree, int lvl)
        {
            StatType t = 0;
            for(int i = 0; i < roleBonus.Length; i++, t++)
            {
                GetBonus(t).BaseValue = (int)tree.GetFormula(t).Calculate(GetBonus(t), lvl);
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
        }

        


        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            try
            {
                for (int i = 0; i < roleBonus.Length; i++)
                {
                    roleBonus[i] = new RoleBonus(roleType.GetStat((StatType)i), this);

                }
            }
            catch
            {

            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            
        }
    }
}