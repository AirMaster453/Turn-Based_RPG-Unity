using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading;

using Object = UnityEngine.Object;

namespace PsychesBound
{
    /// <summary>
    /// For handling core game elements. Such as game overs, starting, loading, etc.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager _main;

        public static GameManager Main => _main;

        #endregion

        private void OnValidate()
        {
            _main = this;
        }

        //private static List<CancellationTokenSource> tokens = new List<CancellationTokenSource>();

        //public static void AddToken(CancellationTokenSource token)
        //{
        //    tokens.Add(token);
        //}

        //public static void RemoveToken(CancellationTokenSource token)
        //{
        //    tokens.Remove(token);
        //}

        CancellationTokenSource stateToken = new CancellationTokenSource();

        public void ChangeToBattleState()
        {
            ChangeState(battleState);
        }

        public void ChangeState(IGameState state)
        {
            if(currentState != null && stateCoroutine != null)
            {
                currentState.OnExit();
                stateToken.Cancel();
            }

            if(currentState?.GetType() == state?.GetType())
            {
                Debug.Log("Same state type");
                return;
            }

            currentState = state;

            currentState.OnEnter();
            UniTask.Run(currentState.OnUpdate, true,stateToken.Token);
            //UniTask.Run

        }

        public Testing.CombatTest CombatTest;

        public BattleField field;

        private GameBattleState battleState;

        private IGameState currentState;

        private Coroutine stateCoroutine;
        private void TestCode()
        {
            ChangeToBattleState();
        }
        // Start is called before the first frame update
        void Start()
        {
            battleState = new GameBattleState(this);
            TestCode();
        }

        // Update is called once per frame
        void Update()
        {
            Object m = this;


        }

        private void OnApplicationQuit()
        {
            //foreach(var token in tokens)
            //{
            //    token.Cancel();
            //}

            //tokens.Clear();

            stateToken.Cancel();
        }

        #region Events
        public static event Action<Unit> OnCharacterLevelUp;
        public static event Action<Unit> OnCharacterDie;
        
        #endregion
    }
}