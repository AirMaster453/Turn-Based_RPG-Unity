using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychesBound
{
    /// <summary>
    /// Stat progress formula for all the stats that deal with damage calculation
    /// </summary>
    [CreateAssetMenu(fileName = "Damage Stat Formula", menuName = menuName + "Damage")]
    public class StandardFormula : StatFormula
    {
        public override float Calculate(ILevelHandler stat, int lvl)
        {
            return ((float)stat.InitialValue * lvl * 2) + (5 * (1 + (lvl / 6))); 
        }
    }
}