using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PsychesBound
{
    public class UnitMeterDisplay : MonoBehaviour
    {
        [SerializeField]
        private Unit unit;

        [SerializeField]
        private Camera uiCamera;

        [SerializeField]
        private Image healthBar;

        [SerializeField]
        private Image aetherBar;

        [SerializeField]
        private Image activeTimeBar;


        private IEnumerator Start()
        {
            yield return new WaitUntil(WaitForStatInit);

            StartCoroutine(UpdateHealth());
            StartCoroutine(UpdateAether());
            StartCoroutine(UpdateTimeBar());
        }

        private void Update()
        {
            transform.LookAt(uiCamera.transform);
        }

        bool WaitForStatInit()
        {
            return unit?.Stats;
        }

        private IEnumerator UpdateHealth()
        {
            while(true)
            {
                healthBar.fillAmount = unit.Stats.HealthPercent();
                yield return null;
            }
        }

        private IEnumerator UpdateAether()
        {
            while (true)
            {
                aetherBar.fillAmount = unit.Stats.AetherPercent();
                yield return null;
            }
        }


        private IEnumerator UpdateTimeBar()
        {
            while (true)
            {
                activeTimeBar.fillAmount = unit.TimeBarPercent();
                yield return null;
            }
        }

    }
}