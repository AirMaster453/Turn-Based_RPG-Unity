using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PsychesBound
{
    public interface IDamageSender 
    {
        float GetAttack(DamageType type);
    }
}