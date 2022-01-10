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

        public void EndTurn()
        {
            turnHolder.OnTurnEnd();
            TurnIsActive = false;
            turnHolder = null;
        }

        public IEnumerator OnEnter()
        {
            //GameManager.field = Object.FindObjectOfType<BattleField>();
            yield return new WaitUntil(GameManager.field.TileSet);

            GameManager.field.PlacePlayers(GameManager.CombatTest.player);
            GameManager.field.PlaceEnemies(GameManager.CombatTest.enemy);

            GameManager.CombatTest.player.OnTurnStartCallback += MovePlayer;

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

        public void MovePlayer(Unit unit)
        {
            var nextTiles = unit.RoleManager.MainRole.RoleType.MovementType.GetTilesInRange(GameManager.field, unit);

            int tile = Random.Range(0, nextTiles.Count);
            Debug.Log($"{unit} is moving");

            GameManager.StartCoroutine(unit.RoleManager.MainRole.RoleType.MovementType.Traverse(nextTiles[tile], unit, GameManager.field));
        }
    }
}