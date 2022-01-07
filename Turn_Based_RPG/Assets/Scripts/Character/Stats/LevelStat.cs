using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

namespace PsychesBound
{
    public class LevelStat : IStat, ILevelHandler
    {

        /// <summary>
        /// The initial value starting at level `
        /// </summary>
        [SerializeField, Min(1)]
        protected int initialValue;

        public int InitialValue { get => initialValue; set => initialValue = value; }


        protected int lastInitValue = int.MinValue;
        /// <summary>
        /// The base value of the stat at current level
        /// </summary>
        //[SerializeField, Min(1)]
        protected int _baseValue;

        protected int lastBaseValue = int.MinValue;


        protected int _value = 0;


        private readonly List<IModifier> modifiers = new List<IModifier>();

        [NonSerialized]
        private readonly ReadOnlyCollection<IModifier> readonlyModifiers;

        public ReadOnlyCollection<IModifier> Modifiers => readonlyModifiers;

        private bool isDirty = true;

        public int BaseValue { get => _baseValue; set => _baseValue = Mathf.Clamp(value, 1, int.MaxValue); }

        public int Value
        {
            get
            {
                if(isDirty || _baseValue != lastBaseValue || initialValue != lastInitValue)
                {
                    _value = CalculateFinalValue();
                    lastBaseValue = _baseValue;
                    lastInitValue = initialValue;
                    isDirty = false;
                }
                return _value;
            }
        }

        float IStat.BaseValue => BaseValue;
        float IStat.Value => Value;

        public LevelStat() : this(1)
        {

        }

        public LevelStat(int initVal)
        {
            modifiers = new List<IModifier>();
            readonlyModifiers = modifiers.AsReadOnly();
            initialValue = initVal;
            _baseValue = initVal;
            isDirty = true;
        }

        public void AddModifier(IModifier mod)
        {
            if (mod == null)
                return;
            modifiers.Add(mod);
            modifiers.Sort(CompareModifiers);
            isDirty = true;
        }

        public bool RemoveFromSource(object src)
        {
            bool didRemove = false;
            for(int i = modifiers.Count - 1; i >= 0; i--)
            {
                var mod = modifiers[i];

                if(mod.Source == src)
                {
                    modifiers.RemoveAt(i);
                    isDirty = true;
                    didRemove = true;
                }    
            }

            return didRemove;
        }

        public bool RemoveModifier(IModifier mod)
        {
            var removed = modifiers.Remove(mod);
            isDirty = isDirty || removed;
            return removed;
        }

        private int CompareModifiers(IModifier a, IModifier b)
        {
            if (a.Order > b.Order)
                return -1;
            else if (a.Order < b.Order)
                return 1;
            else
                return 0;
        }

        private int CalculateFinalValue()
        {
            int finalValue = BaseValue;

            for(int i = 0; i < modifiers.Count; i++)
            {
                var mod = modifiers[i];


                finalValue = (int)mod.Modify(finalValue);
            }

            return finalValue;
        }
    }
}