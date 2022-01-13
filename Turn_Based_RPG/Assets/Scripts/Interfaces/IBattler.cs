using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychesBound
{
    public interface IBattler : IDamageReceiver, IDamageSender, IHoldExtraData
    {
        void AddModifier(StatType type, IModifier modifier);

        void AddModifier(SecondaryType type, IModifier modifier);

        bool RemoveFromSource(object source);

        int GetStatValue(StatType type);

        float GetStatValue(SecondaryType type);
    }
}