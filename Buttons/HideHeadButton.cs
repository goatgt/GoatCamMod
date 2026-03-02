using System;
using UnityEngine;

namespace GoatCamMod
{
    public class hideheadbutton : GorillaPressableButton
    {
        private GameObject headCosmetics;

        public override void ButtonActivation()
        {
            Debug.Log("[Monke Mod] Pressed Button");

            isOn = !isOn;
            UpdateColor();

            if (headCosmetics == null)
            {
                headCosmetics = GameObject.Find("HeadCosmetics");

                if (headCosmetics == null)
                {
                    Debug.LogError("[Monke Mod] Could not find HeadCosmetics");
                    return;
                }
            }

            // Hide when ON, show when OFF
            headCosmetics.SetActive(!isOn);
        }

        public void Start()
        {
            gameObject.layer = 18;

            buttonRenderer = GetComponent<MeshRenderer>();

            unpressedMaterial = new Material(buttonRenderer.material)
            {
                color = Color.white
            };

            pressedMaterial = new Material(buttonRenderer.material)
            {
                color = Color.red
            };
        }
    }
}