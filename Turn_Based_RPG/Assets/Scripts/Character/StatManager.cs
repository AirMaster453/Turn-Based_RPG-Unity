using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lowscope.Saving;
//#if UNITY_EDITOR
using UnityEditor;
//#endif

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
        private StatFormulaTree tree;

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


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

//#if UNITY_EDITOR
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
            base.OnInspectorGUI();
        }
    }
    
//#endif
}