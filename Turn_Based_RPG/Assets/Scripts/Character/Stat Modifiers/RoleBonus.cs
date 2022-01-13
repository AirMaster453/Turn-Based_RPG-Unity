using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace PsychesBound
{
    [Serializable]
    public struct RoleBonus : IModifier, ILevelHandler
    {
        private int _init;

        public int InitialValue { get => _init; set => _init = value; }

        private int _baseValue;
        public int BaseValue { get => _baseValue; set => _baseValue = value; }

        public int Value => BaseValue;

        float IModifier.Value => Value;

        int IModifier.Order => -1;

        private object _source;
        public object Source { get => _source; set => _source = value; }

        float IModifier.Modify(float from)
        {
            return from + Value;
        }

        //public RoleBonus()
        //{

        //}

        public RoleBonus(int initVal, object src)
        {
            _init = initVal;
            _baseValue = initVal;
            _source = src;
        }

        public RoleBonus(object src)
        {
            _source = src;
            _init = 0;
            _baseValue = 0;
        }
    }
}