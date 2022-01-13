using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;


namespace PsychesBound
{
    public interface IDamageReceiver : ITileObject
    {
        int CurrentHealth { get; set;  }
        UniTask TakeDamage(float damage, IDamageSender source);

        float GetDefense(DamageType type);

        event Action<float, IDamageSender> OnTakeDamage;

        UniTask Die();

        event Action OnDie;

        
        
    }
}