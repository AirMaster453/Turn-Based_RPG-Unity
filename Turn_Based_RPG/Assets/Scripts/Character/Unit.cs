using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace PsychesBound
{
    /// <summary>
    /// Core component for all characters
    /// </summary>
    [RequireComponent(typeof(StatManager))]
    [RequireComponent(typeof(RoleManager))]
    public class Unit : MonoBehaviour
    {

        public const int AtbBarSize = 5000;
        public const float OverallSpeed = 7.25f;
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

        public Tile tile { get => _tile; protected set => _tile = value; }

        private Tile _tile;

        [HideInInspector]
        public Direction dir;

        private RoleManager roleManager;

        public RoleManager RoleManager => roleManager;

        [HideInInspector]
        public Actions Actions = default;

        public void Place(Tile target, BattleField field)
        {
            // Make sure old tile location is not still pointing to this unit
            if (tile != null && tile.unit == this)
                tile.unit = null;

            // Link unit and tile references
            tile = target;

            if (target != null)
            {
                target.unit = this;
                target.Trap?.OnStep(this, field);
            }
        }
        public void Match()
        {
            transform.position = tile.transform.position /** 2*/;
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

        private IEnumerator OnBattle(GameBattleState state)
        {
            var routine = new WaitWhile(() => state.TurnIsActive);
            while (true)
            {
                activeTimeBar += stats.TimeBarCharge() * OverallSpeed * Time.deltaTime;

                activeTimeBar = Mathf.Clamp(activeTimeBar, 0, AtbBarSize);

                Debug.Log($"{name}: atb = {activeTimeBar}");

                //var routine = new WaitWhile(() => state.TurnIsActive);
                yield return routine;
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
            battleCoroutine = StartCoroutine(OnBattle(state));
        }

        public void EndBattle()
        {
            StopCoroutine(battleCoroutine);
        }
    }
}