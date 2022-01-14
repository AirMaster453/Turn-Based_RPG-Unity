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
        UniTask TakeDamage(int damage, IDamageSender source);

        int GetDefense(DamageType type);

        event Action<float, IDamageSender> OnTakeDamage;

        UniTask Die();

        event Action OnDie;

        
        
    }
}