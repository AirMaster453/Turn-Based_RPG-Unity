using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PsychesBound
{
    public class UnitRecipe : ScriptableObject
    {
        public const string CharacterFolder = "Character/";

        public string model;
        public List<Role> roles = new List<Role>();
        public Alliances alliance;
    }
}