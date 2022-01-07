using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychesBound
{
    /// <summary>
    /// For waiting in combat when active time bars are charging
    /// </summary>
    public class WaitInCombatSeconds : CustomYieldInstruction
    {
        private float waitingTime;
        public override bool keepWaiting => GameBattleState.CombatTime < waitingTime;

        public WaitInCombatSeconds(float time)
        {
            waitingTime = GameBattleState.CombatTime + time;
        }
    }
}