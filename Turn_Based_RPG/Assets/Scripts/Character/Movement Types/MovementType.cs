using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace PsychesBound
{
    public abstract class MovementType : ScriptableObject
    {
        public const string menuName = "Movement Type/";
        public virtual List<Tile> GetTilesInRange(BattleField field, Unit unit)
        {
            List<Tile> retValue = field.Search(unit, unit.Tile, ExpandSearch);
            Filter(retValue);
            return retValue;
        }
        protected virtual bool ExpandSearch(Tile from, Tile to, Unit unit)
        {
            return (from.distance + 1) <= unit.Stats.Distance.Value;
        }

        protected virtual void Filter(List<Tile> tiles)
        {
            for (int i = tiles.Count - 1; i >= 0; --i)
                if (tiles[i].unit != null)
                    tiles.RemoveAt(i);
        }

        /// <summary>
        /// Make sure method is async when overriding
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="unit"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public abstract UniTask Traverse(Tile tile, Unit unit, BattleField field);
    }
}