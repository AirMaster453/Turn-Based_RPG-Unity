using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychesBound
{
    public interface IEquip
    {
        IModifier GetModifier(StatType stat);

        IModifier Distance { get; }
        IModifier JumpHeight { get; }
        IModifier HitRate { get; }
        IModifier CritRate { get; }
        IModifier Evasion { get; }
        IModifier Speed { get; }
    }
}