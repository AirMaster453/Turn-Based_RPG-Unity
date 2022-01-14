using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PsychesBound
{
    public class Tile : MonoBehaviour
    {
        public const float stepHeight = 0.25f;
        /// <summary>
        /// The unit currently on the tile
        /// </summary>
        [HideInInspector]
        public ITileObject content;

        [SerializeField]
        private float heightScale = 0.25f;

        /// <summary>
        /// The tile before the this tile
        /// </summary>
        [HideInInspector]
        public Tile prev;

        [HideInInspector]
        public int distance = int.MaxValue;

        public Point position;

        [Min(0)]
        public int height = 1;

        [SerializeField]
        private Vector3 _center;

        public Vector3 center => transform.position - _center;

        public BattleField field;




        public ITrap Trap;

        private void OnValidate()
        {
            var scale = transform.localScale;
            scale.y = heightScale * height;
            transform.localScale = scale;
            var pos = transform.localPosition;
            pos.y = scale.y /2;

            transform.localPosition = pos;

        }

        public void OnStep(Unit unit)
        {
            this.content = unit;
            Trap?.OnStep(unit, field);
        }
    }
}