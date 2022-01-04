using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PsychesBound
{
    /// <summary>
    /// Core component for all characters
    /// </summary>
    [RequireComponent(typeof(RoleManager))]
    public class Unit : MonoBehaviour
    {

        public const int AtbBarSize = 5000;
        // Add level. (If jobs/roles don't offer their own level system)

        // Add Stats

        /// <summary>
        /// Temperary agility stat
        /// </summary>
        public int agility = 10;

        public int movement = 2;

        public int jumpHeight;

        private float ActiveTimeBar = 0;

        // Add Role/job/class

        // add behavior and events for combat/turn handling

        [SerializeField]
        private Transform jumper;

        public Transform Jumper => jumper;

        public Tile tile { get; protected set; }

        [HideInInspector]
        public Direction dir;

        private RoleManager roleManager;

        public void Place(Tile target, BattleField field)
        {
            // Make sure old tile location is not still pointing to this unit
            if (tile != null && tile.unit == gameObject)
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
            transform.localPosition = tile.transform.position;
            transform.localEulerAngles = dir.ToEuler();
        }
    

        public virtual event Action<Unit> OnTurnStartCallback;
        public virtual event Action<Unit> OnTurnEndCallback;

        // add simple state machine

        // add some form of atb handling

        // Start is called before the first frame update
        void Start()
        {
            roleManager = GetComponent<RoleManager>();
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
            while(true)
            {
                ActiveTimeBar += agility * Time.deltaTime;
                yield return new WaitWhile(() => state.TurnIsActive);
                if (ActiveTimeBar >= AtbBarSize)
                {
                    
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