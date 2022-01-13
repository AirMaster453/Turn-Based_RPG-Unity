using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PsychesBound
{
    public abstract class Art : ScriptableObject
    {
        protected const string ArtMenu = "Arts/";

        /// <summary>
        /// The points at where the art hits
        /// </summary>
        protected List<Point> range = new List<Point>();
        public abstract UniTask Execute(Unit unit, BattleField field);
    }
}