using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

namespace PsychesBound
{
    [Serializable]
    public class FloatStat : IStat, ISerializationCallbackReceiver
    {
        [SerializeField, Min(1)]
        protected float _baseValue;


        protected float lastBaseValue = float.MinValue;

        protected float _value = 0;

        private readonly List<IModifier> modifiers = new List<IModifier>();

        [NonSerialized]
        private readonly ReadOnlyCollection<IModifier> readonlyModifiers;

        public ReadOnlyCollection<IModifier> Modifiers => readonlyModifiers;

        private bool isDirty = true;

        public float BaseValue
        {
            get => _baseValue;
        }

        public float Value
        {
            get
            {
                if (isDirty || _baseValue != lastBaseValue)
                {
                    _value = CalculateFinalValue();
                    lastBaseValue = _baseValue;
                    isDirty = false;
                }
                return _value;
            }
        }

        

        public FloatStat() : this(1)
        {

        }

        public FloatStat(int baseVal)
        {
            modifiers = new List<IModifier>();
            readonlyModifiers = modifiers.AsReadOnly();
            _baseValue = baseVal;
            isDirty = true;
        }

        public void AddModifier(IModifier mod)
        {
            modifiers.Add(mod);
            modifiers.Sort(CompareModifiers);
            isDirty = true;
        }

        public bool RemoveFromSource(object src)
        {
            bool didRemove = false;
            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                var mod = modifiers[i];

                if (mod.Source == src)
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

        private float CalculateFinalValue()
        {
            float finalValue = BaseValue;

            for (int i = 0; i < modifiers.Count; i++)
            {
                var mod = modifiers[i];


                finalValue = mod.Modify(finalValue);
            }

            return (float)Math.Round(finalValue, 1);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _baseValue = (float)Math.Round(_baseValue, 1);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            
        }
    }
}