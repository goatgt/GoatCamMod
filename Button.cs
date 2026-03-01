using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

namespace GoatCamMod
{
    public class button : GorillaPressableButton
    {
        public override void ButtonActivation()
        {
            Debug.Log("[Monke Mod] Pressed Button");
            isOn = !isOn;
            UpdateColor();

        }
        public void Start()
        {
            this.gameObject.layer = 18;

            buttonRenderer = GetComponent<MeshRenderer>();
            Material unpressedMat = new Material(buttonRenderer.material) { color = Color.white };
            Material pressedMat = new Material(buttonRenderer.material) { color = Color.red };
            unpressedMaterial = unpressedMat;
            pressedMaterial = pressedMat;

        }
    }
}