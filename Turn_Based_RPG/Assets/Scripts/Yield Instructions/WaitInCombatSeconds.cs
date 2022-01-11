using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;


namespace PsychesBound
{
    /// <summary>
    /// For waiting in combat when active time bars are charging
    /// </summary>
    public struct WaitInCombatSeconds : IEnumerator
    {
        private float waitingTime;
        public  bool MoveNext() => GameBattleState.CombatTime < waitingTime;

        public void Reset()
        {
            waitingTime = 0f;
        }

        public object Current => null;

        public WaitInCombatSeconds(float time)
        {
            waitingTime = GameBattleState.CombatTime + time;
        }
    }

    
}