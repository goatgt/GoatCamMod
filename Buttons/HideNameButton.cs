using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

namespace GoatCamMod
{
    public class hidenamebutton : GorillaPressableButton
    {
        private GameObject nameTagObject;

        public override void ButtonActivation()
        {
            Debug.Log("[Monke Mod] Pressed Button");

            isOn = !isOn;
            UpdateColor();

            if (nameTagObject == null)
            {
                nameTagObject = GameObject.Find("body_AnchorFrontRight_NameTag");
            }

            if (nameTagObject != null)
            {
                nameTagObject.SetActive(!isOn);
            }
            else
            {
                Debug.LogWarning("[Monke Mod] Could not find body_AnchorFrontRight_NameTag");
            }
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