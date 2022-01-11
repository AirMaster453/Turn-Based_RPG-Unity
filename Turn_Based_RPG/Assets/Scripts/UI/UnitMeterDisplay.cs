using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;


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


        private async void Start()
        {
            await UniTask.WaitUntil(WaitForStatInit);

            UpdateHealth();
            UpdateAether();
            UpdateTimeBar();
        }

        private void Update()
        {
            transform.LookAt(uiCamera.transform);
        }

        bool WaitForStatInit()
        {
            return unit?.Stats;
        }

        private async UniTask UpdateHealth()
        {
            while(true)
            {
                healthBar.fillAmount = unit.Stats.HealthPercent();
                await UniTask.Yield();
            }
        }

        private async UniTask UpdateAether()
        {
            while (true)
            {
                aetherBar.fillAmount = unit.Stats.AetherPercent();
                await UniTask.Yield();
            }
        }


        private async UniTask UpdateTimeBar()
        {
            while (true)
            {
                activeTimeBar.fillAmount = unit.TimeBarPercent();
                await UniTask.Yield();
            }
        }

    }
}