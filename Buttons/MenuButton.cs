using UnityEngine;

namespace GoatCamMod
{
    public class menubutton : GorillaPressableButton
    {
        private GameObject menu;

        public override void ButtonActivation()
        {
            Debug.Log("[GoatCamMod] Pressed Button");

            isOn = !isOn;
            UpdateColor();

            if (menu != null)
            {
                menu.SetActive(isOn); // Toggle visibility
            }
        }

        void Start()
        {
            this.gameObject.layer = 18;

            buttonRenderer = GetComponent<MeshRenderer>();
            Material unpressedMat = new Material(buttonRenderer.material) { color = Color.white };
            Material pressedMat = new Material(buttonRenderer.material) { color = Color.red };

            unpressedMaterial = unpressedMat;
            pressedMaterial = pressedMat;

            // 🔥 Find GoatCameraModModelBetter/Menu
            menu = transform.root.Find("GoatCamModObjects/Menu")?.gameObject;

            if (menu != null)
            {
                menu.SetActive(false); // Hidden by default
            }
            else
            {
                Debug.LogError("[GoatCamMod] Could not find Menu!");
            }
        }
    }
}
