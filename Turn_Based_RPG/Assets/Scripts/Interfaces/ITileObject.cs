using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PsychesBound
{
    public interface ITileObject
    {
        Tile Tile { get; }


        void Place(Tile tile, BattleField field);

        /// <summary>
        /// To match the placement of the current tile
        /// </summary>
        void Match();

        Direction Direction { get; set; }
    }
}