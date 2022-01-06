using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PsychesBound
{
    public enum Alliances
    {
        None = 0,
        Neutral = 1 << 0,
        Player = 1 << 1,
        Enemy = 1 << 2
    }
}