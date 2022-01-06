using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychesBound
{
    /// <summary>
    /// stat progress formula for stats like aether and hit points.
    /// </summary>
    [CreateAssetMenu(fileName = "Meter Formula", menuName = menuName + "Meter")]
    public class MeterFormula : StatFormula
    {
        public override float Calculate(ILevelHandler stat, int lvl)
        {
            return ((float)stat.InitialValue * lvl * 10) + (50 * (10 + ((lvl * 10) / 6)));
        }
    }
}