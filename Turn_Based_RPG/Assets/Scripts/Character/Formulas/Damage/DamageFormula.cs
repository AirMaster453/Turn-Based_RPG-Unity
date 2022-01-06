using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PsychesBound
{
    public class DamageFormula : ScriptableObject
    {
        public virtual float CalculateDamage(Unit user, Unit target, DamageType type)
        {
            var damage = Mathf.Pow(user.Stats.GetStat(type.ToAttackType()).Value, 2) / target.Stats.GetStat(type.ToDefenseType()).Value;
            return damage;
        }
    }
}
