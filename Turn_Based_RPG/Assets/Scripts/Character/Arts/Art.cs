using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PsychesBound
{
    public abstract class Art : ScriptableObject
    {
        protected const string ArtMenu = "Arts/";


        public abstract UniTask Execute(Unit unit, BattleField field);
    }
}