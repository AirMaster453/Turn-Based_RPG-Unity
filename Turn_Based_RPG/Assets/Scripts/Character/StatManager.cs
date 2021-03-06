using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lowscope.Saving;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PsychesBound
{
    public class StatManager : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private LevelStat[] stats = new LevelStat[Enum.GetNames(typeof(StatType)).Length];

        [SerializeField]
        private IntStat distance = new IntStat(1);

        public IntStat Distance => distance;

        [SerializeField]
        private IntStat jumpHeight = new IntStat(1);

        public IntStat JumpHeight => jumpHeight;

        [SerializeField]
        private FloatStat hitRatePercent = new FloatStat(80);

        public FloatStat HitRatePercent => hitRatePercent;


        [SerializeField]
        private FloatStat critRatePercent = new FloatStat(5);

        public FloatStat CritRatePercent => critRatePercent;

        [SerializeField]
        private FloatStat evasionPercent = new FloatStat(5);

        public FloatStat EvasionPercent => evasionPercent;

        [SerializeField]
        private FloatStat speedPercent = new FloatStat(100);

        public FloatStat SpeedPercent => speedPercent;

        [SerializeField]
        private FloatStat supportPercent = new FloatStat(100);

        public FloatStat SupportPercent => supportPercent;



        [SerializeField]
        private StatFormulaTree tree;

        public StatFormulaTree Tree => tree;


        private RoleManager roleManager;

        public StatManager()
        {
            stats = new LevelStat[Enum.GetNames(typeof(StatType)).Length];

            for(int i = 0; i < stats.Length; i++)
            {
                stats[i] = new LevelStat(1);
            }
        }

        public LevelStat GetStat(StatType i)
        {
            try
            {
                return stats[(int)i];
            }
            catch
            {
                stats = new LevelStat[Enum.GetNames(typeof(StatType)).Length];

                for (int j = 0; j < stats.Length; j++)
                {
                    stats[j] = new LevelStat(1);
                }

                return stats[(int)i];
            }
        }

        public IStat GetStat(SecondaryType i)
        {
            switch(i)
            {
                case SecondaryType.Distance:
                    return Distance;
                case SecondaryType.JumpHeight:
                    return JumpHeight;
                case SecondaryType.HitRate:
                    return HitRatePercent;
                case SecondaryType.CritRate:
                    return CritRatePercent;
                case SecondaryType.Evasion:
                    return EvasionPercent;
                case SecondaryType.Speed:
                    return SpeedPercent;
                case SecondaryType.Support:
                    return SupportPercent;
                default:
                    return null;
            }
        }

        private int _currentHealth = int.MaxValue;

        public int CurrentHealth
        {
            get
            {
                return _currentHealth;
            }

            set
            {
                _currentHealth = Mathf.Clamp(value, 0, GetStat(StatType.HitPoints).Value);
            }
        }

        public float HealthPercent()
        {
            return (float)CurrentHealth / (float)GetStat(StatType.HitPoints).Value;
        }


        private int _currentAether = int.MaxValue;

        public int CurrentAether
        {
            get
            {
                return _currentAether;
            }

            set
            {
                _currentAether = Mathf.Clamp(value, 0, GetStat(StatType.Aether).Value);
            }
        }

        public float AetherPercent()
        {
            return (float)CurrentAether / (float)GetStat(StatType.Aether).Value;
        }

        public float TimeBarCharge()
        {
            return GetStat(StatType.Agility).Value * SpeedPercent.Value;
        }

        public void Equip(IEquip equip)
        {
            for (StatType i = 0; i < (StatType)stats.Length; i++)
            {
                stats[(int)i].AddModifier(equip.GetModifier(i));
            }

            for (SecondaryType i = 0; i < (SecondaryType)Enum.GetNames(typeof(SecondaryType)).Length; i++)
            {
                GetStat(i).AddModifier(equip.GetModifier(i));
            }
        }

        public bool RemoveFromSource(object src)
        {
            bool didRemove = false;
            for(int i = 0; i < stats.Length; i++)
            {
                didRemove = didRemove || stats[i].RemoveFromSource(src);
            }


            for (SecondaryType i = 0; i < (SecondaryType)Enum.GetNames(typeof(SecondaryType)).Length; i++)
            {
                didRemove = didRemove || GetStat(i).RemoveFromSource(src);
            }

            return didRemove;
        }


        public void OnRoleChange(Role oldRole, Role newRole)
        {
            if(oldRole != null)
            {
                RemoveFromSource(oldRole);
            }

            if(newRole != null)
            {
                Equip(newRole);
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            roleManager = GetComponent<RoleManager>();

            
        }



        // Update is called once per frame
        void Update()
        {

        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(StatManager))]
    public class StatManagerEditor :Editor
    {
        bool fold = true;
        int length = Enum.GetNames(typeof(StatType)).Length;

        public override void OnInspectorGUI()
        {
            var stat = target as StatManager;
            fold = EditorGUILayout.Foldout(fold, "Primary Stats");

            if(fold)
            {
                for(int i = 0; i < length; i++)
                {
                    int value = EditorGUILayout.IntField( $"{(StatType)i}", stat.GetStat((StatType)i).InitialValue);

                    stat.GetStat((StatType)i).InitialValue = value;
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            base.OnInspectorGUI();
        }
    }
    
#endif
}