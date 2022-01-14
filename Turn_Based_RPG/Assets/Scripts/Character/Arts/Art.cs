using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PsychesBound
{
    public abstract class Art : ScriptableObject
    {
        protected const string ArtMenu = "Arts/";

        [SerializeField]
        protected string _name;

        public string ArtName => _name;

        [SerializeField]
        protected string _description;

        public string Description => _description;

        [SerializeField]
        protected int _cost;

        public int Cost => _cost;


        [SerializeField]
        /// <summary>
        /// The points at where the art hits
        /// </summary>
        protected List<Point> range = new List<Point>();
        public abstract UniTask Execute(IBattler unit, BattleField field);

        public virtual bool Condition(IBattler unit, BattleField field)
        {
            return unit.CurrentAether >= _cost;
        }


        public virtual HashSet<Tile> GetInRange(IBattler unit, BattleField field)
        {
            HashSet<Tile> tiles = new HashSet<Tile>();

            for(int i = 0; i < range.Count; i++)
            {
                Tile tile = field.GetTileAt(range[i] + unit.Tile.position);

                if(tile)
                    tiles.Add(tile);
            }

            Filter(tiles);

            return tiles;
        }
        protected abstract void Filter(HashSet<Tile> tiles);
    }
}