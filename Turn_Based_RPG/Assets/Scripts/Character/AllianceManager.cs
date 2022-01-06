using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PsychesBound
{
    public class AllianceManager : MonoBehaviour
    {
        [SerializeField]
        private Alliances type;

        private Alliances currentAlliance;

        public Alliances CurrentAlliance => currentAlliance;
        
        public void ResetAlliance()
        {
            currentAlliance = type;
        }

        public Alliances GetAlliances() => type;

        public void SetBaseAlliance(Alliances alliances)
        {
            type = alliances;
        }

        // Start is called before the first frame update
        void Start()
        {
            ResetAlliance();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
