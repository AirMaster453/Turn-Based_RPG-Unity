using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;


namespace PsychesBound
{
    /// <summary>
    /// Core component for all characters
    /// </summary>
    [RequireComponent(typeof(StatManager))]
    [RequireComponent(typeof(RoleManager))]
    public class Unit : MonoBehaviour, IBattler
    {

        public const int AtbBarSize = 5000;
        public const float OverallSpeed = 19.45f;
        // Add level. (If jobs/roles don't offer their own level system)

        // Add Stats
        private StatManager stats;

        public StatManager Stats => stats;
        
        //public int agility = 10;

        //public int movement = 2;

        //public int jumpHeight;

        private float activeTimeBar = 0;

        
        public float TimeBarPercent()
        {
            return activeTimeBar / AtbBarSize;
        }

        // Add Role/job/class

        // add behavior and events for combat/turn handling

        [SerializeField]
        private Transform jumper;

        public Transform Jumper => jumper;

        public Tile Tile { get => _tile; protected set => _tile = value; }

        private Tile _tile;
         
        public Direction Direction { get => dir;  set => dir = value; }
        private Direction dir;

        private RoleManager roleManager;

        public RoleManager RoleManager => roleManager;

        [HideInInspector]
        public Actions Actions = default;

        public void Place(Tile target, BattleField field)
        {
            // Make sure old tile location is not still pointing to this unit
            if (Tile != null && Tile.content == this)
                Tile.content = null;

            // Link unit and tile references
            Tile = target;

            if (target != null)
            {
                target.content = this;
                target.Trap?.OnStep(this, field);
            }
        }
        public void Match()
        {
            transform.position = Tile.transform.position /** 2*/;
            transform.localEulerAngles = dir.ToEuler();
        }

        private Dictionary<string, object> _extraData = new Dictionary<string, object>();

        public void AddExtraData(string key, object value)
        {
            if (_extraData.ContainsKey(key))
                return;


            _extraData.Add(key, value);
        }

        public object GetExtraData(string key)
        {
            object value;

            _extraData.TryGetValue(key, out value);
            return value;
        }


        public void SetExtraData(string key, object value)
        {
            try
            {
                _extraData[key] = value;
            }
            catch
            {
                
            }
        }
        public bool RemoveExtraData(string key)
        {
            return _extraData.Remove(key);
        }
    
        
        public virtual event Action<Unit> OnTurnStartCallback;
        public virtual event Action<Unit> OnTurnEndCallback;
        public event Action<float, IDamageSender> OnTakeDamage;
        public event Action OnDie;

        // add simple state machine

        // add some form of atb handling

        // Start is called before the first frame update
        void Start()
        {
            roleManager = GetComponent<RoleManager>();
            stats = GetComponent<StatManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTurnStart()
        {
            OnTurnStartCallback?.Invoke(this);
        }

        public void OnTurnEnd()
        {
            OnTurnEndCallback?.Invoke(this);
        }

        private Coroutine battleCoroutine;

        //private IEnumerator OnBattle(GameBattleState state)
        //{
        //    var routine = new WaitWhile(() => state.TurnIsActive);
        //    while (true)
        //    {
        //        activeTimeBar += stats.TimeBarCharge() * OverallSpeed * Time.deltaTime;

        //        activeTimeBar = Mathf.Clamp(activeTimeBar, 0, AtbBarSize);

        //        Debug.Log($"{name}: atb = {activeTimeBar}");

        //        //var routine = new WaitWhile(() => state.TurnIsActive);
        //        yield return routine;
        //        if (activeTimeBar >= AtbBarSize && state.turnHolder == null)
        //        {
        //            state.StartTurn(this);
        //            activeTimeBar = 0;

        //            Debug.Log($"{name} turn is active");

        //        }

        //    }
        //}


        CancellationTokenSource battleTask = new CancellationTokenSource();

        public bool IsBattling { get; private set; }
        public int CurrentHealth { get => Stats.CurrentHealth; set => Stats.CurrentHealth = value; }

        public int CurrentAether { get => Stats.CurrentAether; set => Stats.CurrentAether = value;}
        private async UniTask OnBattle(GameBattleState state)
        {
            var routine = new WaitWhile(() => GameBattleState.TurnIsActive);
            while (IsBattling)
            {
                activeTimeBar += stats.TimeBarCharge() * OverallSpeed * Time.deltaTime;

                activeTimeBar = Mathf.Clamp(activeTimeBar, 0, AtbBarSize);

                Debug.Log($"{name}: atb = {activeTimeBar}");

                //var routine = new WaitWhile(() => state.TurnIsActive);
                await UniTask.WaitWhile(() => GameBattleState.TurnIsActive, PlayerLoopTiming.Update,battleTask.Token);
                if (activeTimeBar >= AtbBarSize && state.turnHolder == null)
                {
                    state.StartTurn(this);
                    activeTimeBar = 0;

                    Debug.Log($"{name} turn is active");

                }

            }
        }



        public void BeginBattle(GameBattleState state)
        {
            //battleCoroutine = StartCoroutine(OnBattle(state));

            IsBattling = true;

            OnBattle(state);
        }

        public void EndBattle()
        {
            //StopCoroutine(battleCoroutine);

            IsBattling = false;

            battleTask.Cancel();
        }


        protected void OnApplicationQuit()
        {
            battleTask.Cancel();
        }

        public void AddModifier(StatType type, IModifier modifier)
        {
            Stats.GetStat(type).AddModifier(modifier);
        }

        public void AddModifier(SecondaryType type, IModifier modifier)
        {
            Stats.GetStat(type).AddModifier(modifier);
        }

        public bool RemoveFromSource(object source)
        {
            return Stats.RemoveFromSource(source);
        }

        public int GetStatValue(StatType type)
        {
            return Stats.GetStat(type).Value;
        }



        public float GetStatValue(SecondaryType type)
        {
            return Stats.GetStat(type).Value;
        }

        public async UniTask TakeDamage(int damage, IDamageSender source)
        {
            OnTakeDamage?.Invoke(damage, source);

            CurrentHealth -= damage;

            //Add some getting hit animation

            if(CurrentHealth <= 0)
            {
                await Die();
            }
        }

        public int GetDefense(DamageType type)
        {
            return Stats.GetStat(type.ToDefenseType()).Value;
        }

        public async UniTask Die()
        {
            //Will be replaced witha delay for the death animation
            await UniTask.Yield();


            OnDie?.Invoke();
        }

        public int GetAttack(DamageType type)
        {
            return Stats.GetStat(type.ToAttackType()).Value;
        }
    }
}