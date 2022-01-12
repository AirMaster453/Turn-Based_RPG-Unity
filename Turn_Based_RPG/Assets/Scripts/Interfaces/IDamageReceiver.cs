using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


namespace PsychesBound
{
    public interface IDamageReceiver 
    {
        UniTask TakeDamage(float damage);

        float GetDefense(DamageType type);
    }
}