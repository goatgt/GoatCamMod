using UnityEngine;

namespace GoatCamMod
{
    public class smoothingbuttonup : GorillaPressableButton
    {
        private MeshRenderer buttonRenderer;
        private Material unpressedMaterial;
        private Material pressedMaterial;

        public override void ButtonActivation()
        {
            isOn = !isOn;
            UpdateColor();

            firstpersonbutton.CycleSmoothingModeForward();
        }

        public void Start()
        {
            gameObject.layer = 18;

            buttonRenderer = GetComponent<MeshRenderer>();
            unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white };
            pressedMaterial = new Material(buttonRenderer.material) { color = Color.red };
        }

        private void UpdateColor()
        {
            if (buttonRenderer == null) return;
            buttonRenderer.material = isOn ? pressedMaterial : unpressedMaterial;
        }
    }
}