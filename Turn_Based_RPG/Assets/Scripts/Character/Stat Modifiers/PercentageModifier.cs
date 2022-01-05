using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PsychesBound
{
    [System.Serializable]
    public class PercentageModifier : IModifier
    {
        [SerializeField]
        protected float _value;

        [SerializeField]
        protected int _order;

        protected object _source;


        public float Value => _value;

        public int Order => _order;

        public object Source => _source;

        public float Modify(float from)
        {
            return from * (1 + _value / 100);
        }

        public PercentageModifier(float val, int ord, object src)
        {
            _value = val;
            _order = ord;
            _source = src;
        }

        public PercentageModifier(float val, object src) : this(val, 1, src) { }
    }
}