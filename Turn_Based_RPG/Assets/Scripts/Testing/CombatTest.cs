using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace PsychesBound.Testing
{
    public class CombatTest : MonoBehaviour
    {
        [SerializeField]
        public Unit player;

        [SerializeField]
        public Unit enemy;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPlayerCharacterTurn(Unit unit)
        {
            var ui = SceneManager.GetSceneByName("UI");
            if(!ui.isLoaded)
            {
                SceneManager.LoadScene("UI", LoadSceneMode.Additive);
            }

            List<GameObject> objects = new List<GameObject>(ui.GetRootGameObjects());
            var canvas = objects.Find((GameObject a) => a.name == "Canvas");

            if(canvas)
            {
                //Get child component of action display for player characters
            }
        }
    }
}