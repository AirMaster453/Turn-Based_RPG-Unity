using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychesBound
{
    public interface IEquip
    {
        IModifier GetModifier(StatType stat);

        IModifier GetModifier(SecondaryType type);
    }
}