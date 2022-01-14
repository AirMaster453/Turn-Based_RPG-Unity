using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace PsychesBound
{
    public class GameBattleState : IGameState
    {
        
        public GameManager GameManager { get; set; }

        /// <summary>
        /// If a unit's turn is in play
        /// </summary>
        public static bool TurnIsActive { get; private set; } = false;

        public int TurnCounter;

        public static float CombatTime { get; private set; } = 0;

        public BattleField field;

        internal Unit turnHolder;

        public async static UniTask WaitInCombatSeconds(float seconds, CancellationToken token = default)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds), () => !TurnIsActive, PlayerLoopTiming.Update, token);
        }

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

        public async UniTask OnEnter()
        {
            //GameManager.field = Object.FindObjectOfType<BattleField>();
            await UniTask.WaitUntil(GameManager.field.TileSet);

            GameManager.field.PlacePlayers(GameManager.CombatTest.player);
            GameManager.field.PlaceEnemies(GameManager.CombatTest.enemy);

            GameManager.CombatTest.player.OnTurnStartCallback += MovePlayer;
            GameManager.CombatTest.enemy.OnTurnStartCallback += MovePlayer;

            CombatTime = 0;

            this.GameManager.CombatTest.player.BeginBattle(this);
            this.GameManager.CombatTest.enemy.BeginBattle(this);
            
        }

        public async UniTask OnExit()
        {
            TurnIsActive = false;
            await UniTask.Yield();
        }

        public async void OnUpdate()
        {
            while(true)
            {
                if(!TurnIsActive)
                {
                    CombatTime += Time.deltaTime;
                }
                await UniTask.Yield();
            }
        }

        public  void MovePlayer(Unit unit)
        {
            DoAction(unit);
        }


        private async UniTask DoAction(Unit unit)
        {
            var nextTiles = unit.RoleManager.MainRole.RoleType.MovementType.GetTilesInRange(GameManager.field, unit);

            int tile = UnityEngine.Random.Range(0, nextTiles.Count);
            Debug.Log($"{unit} is moving");

            await unit.RoleManager.MainRole.RoleType.MovementType.Traverse(nextTiles[tile], unit, GameManager.field);

            nextTiles = null;

            //GC.Collect();
            EndTurn();
        }


    }
}