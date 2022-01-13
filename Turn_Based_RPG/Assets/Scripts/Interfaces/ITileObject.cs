using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PsychesBound
{
    public interface ITileObject
    {
        Tile Tile { get; }


        void Place(Tile tile, BattleField field);
    }
}