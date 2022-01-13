using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PsychesBound
{
    // Base class for all role/job scripts
    [CreateAssetMenu(menuName = "Character/Role Type")]
    public class RoleType : ScriptableObject
    {

        // Add stat bonuses
        [SerializeField, HideInInspector]
        private int[] statBonuses = new int[Enum.GetNames(typeof(StatType)).Length];

        [SerializeField, Min(0)]
        private int distance = 0;

        public int Distance => distance;

        [SerializeField, Min(0)]
        private int jumpHeight = 0;

        public int JumpHeight => jumpHeight;

        [SerializeField, Min(0)]
        private float hitRate = 0;

        public float HitRate => hitRate;

        [SerializeField, Min(0)]
        private float critRate = 0;

        public float CritRate => critRate;

        [SerializeField, Min(0)]
        private float evasion = 0;

        public float Evasion => evasion;


        [SerializeField, Min(0)]
        private float speed = 0;

        public float Speed => speed;


        [SerializeField, Min(0)]
        private float support = 0;

        public float Support => support;



        // Add abilities
        [SerializeField]
        protected List<Ability> abilities = new List<Ability>();

        public RoleType()
        {
            statBonuses = new int[Enum.GetNames(typeof(StatType)).Length];

            for(int i = 0; i < statBonuses.Length; i++)
            {
                statBonuses[i] = 1;
            }


        }



        [SerializeField]
        private MovementType movementType;




        public int GetStat(StatType type)
        {
            try
            {
                return statBonuses[(int)type];
            }
            catch (IndexOutOfRangeException m)
            {
                Debug.LogError(m);
                statBonuses = new int[Enum.GetNames(typeof(StatType)).Length];
                return statBonuses[(int)type];
            }
        }

        public float GetStat(SecondaryType type)
        {
            switch(type)
            {
                case SecondaryType.Distance:
                    return distance;
                case SecondaryType.JumpHeight:
                    return jumpHeight;
                case SecondaryType.HitRate:
                    return hitRate;
                case SecondaryType.CritRate:
                    return critRate;
                case SecondaryType.Evasion:
                    return evasion;
                case SecondaryType.Speed:
                    return speed;
                case SecondaryType.Support:
                    return support;
                default:
                    return 0;
            }
        }


        internal void SetStat(StatType type, int value)
        {
            try
            {
                statBonuses[(int)type] = value;
            }
            catch (IndexOutOfRangeException m)
            {
                statBonuses = new int[Enum.GetNames(typeof(StatType)).Length];
                statBonuses[(int)type] = value;
                Debug.LogError(m);
            }
        }

        private ReadOnlyCollection<Ability> asReadOnly;
        public ReadOnlyCollection<Ability> Abilities
        {
            get
            {
                if (asReadOnly == null)
                    asReadOnly = abilities.AsReadOnly();
                return asReadOnly;
            }
        }

        public MovementType MovementType => movementType;

        // Add list of equippable armors and weapons

        // Add leveling

        //public abstract void Ability(Unit unit);
    }

#if UNITY_EDITOR
    [CustomEditor((typeof(RoleType)))]
    public class RoleTypeEditor : Editor
    {
        int length = Enum.GetNames(typeof(StatType)).Length;
        bool fold = false;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(30f);
            fold = EditorGUILayout.Foldout(fold, "Initial Stat Bonuses");

            var role = target as RoleType;

            if(fold)
            {
                
                for(int i = 0; i < length; i++)
                {
                    var value = Mathf.Clamp(EditorGUILayout.IntField($"{(StatType)i}", role.GetStat((StatType)i)), 0, int.MaxValue);
                    role.SetStat((StatType)i, value);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }

    }
#endif
}