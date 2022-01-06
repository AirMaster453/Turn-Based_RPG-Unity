using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychesBound
{
    /// <summary>
    /// Stat progress formula for stats like agility and luck
    /// </summary>
    [CreateAssetMenu(fileName = "Agility Formula", menuName = menuName + "Other")]
    public class OtherFormula : StatFormula
    {
        
        public override float Calculate(ILevelHandler stat, int lvl)
        {
            return ((float)stat.InitialValue * lvl) + (5 * (1 + (lvl / 6)));
        }
    }
}