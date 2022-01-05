using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PsychesBound
{
    public enum Actions
    {
        None = 0,
        Move = 1 << 0,
        Attack = 1 << 1,
        EndTurn = 1 << 2
    }
}
