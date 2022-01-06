using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PsychesBound
{
    public abstract class StatFormula : ScriptableObject
    {
        protected const string menuName = "Formulas/";
        public abstract float Calculate(ILevelHandler stat, int lvl);
    }
}