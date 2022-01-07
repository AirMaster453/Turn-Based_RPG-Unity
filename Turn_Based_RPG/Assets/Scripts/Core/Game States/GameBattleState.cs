using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychesBound
{
    public class GameBattleState : IGameState
    {
        
        public GameManager GameManager { get; set; }

        /// <summary>
        /// If a unit's turn is in play
        /// </summary>
        public bool TurnIsActive { get; private set; } = false;

        public int TurnCounter;

        public static float CombatTime { get; private set; } = 0;

        public BattleField field;

        internal Unit turnHolder;

        public GameBattleState(GameManager manager)
        {
            GameManager = manager;
        }

        public void StartTurn(Unit unit)
        {
            turnHolder = unit;
            TurnIsActive = true;
            turnHolder.OnTurnStart();
        }

        public IEnumerator OnEnter()
        {
            //GameManager.field = Object.FindObjectOfType<BattleField>();
            yield return new WaitUntil(GameManager.field.TileSet);

            GameManager.field.PlacePlayers(GameManager.CombatTest.player);
            GameManager.field.PlaceEnemies(GameManager.CombatTest.enemy);

            CombatTime = 0;

            this.GameManager.CombatTest.player.BeginBattle(this);
            this.GameManager.CombatTest.enemy.BeginBattle(this);
            yield break;
        }

        public IEnumerator OnExit()
        {
            TurnIsActive = false;
            yield break;
        }

        public IEnumerator OnUpdate()
        {
            while(true)
            {
                if(!TurnIsActive)
                {
                    CombatTime += Time.deltaTime;
                }
                yield return null;
            }
        }
    }
}