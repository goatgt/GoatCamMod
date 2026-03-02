using UnityEngine;
using TMPro;

namespace GoatCamMod
{
    public class flipbutton : GorillaPressableButton
    {
        private Transform camTransform;
        private bool flipped;

        public override void ButtonActivation()
        {
            base.ButtonActivation();

            Debug.Log("[Monke Mod] Pressed Button");

            isOn = !isOn;
            UpdateColor();

            // Find Camera component anywhere in this prefab
            if (camTransform == null)
            {
                Camera cam = transform.root.GetComponentInChildren<Camera>(true);
                if (cam != null)
                {
                    camTransform = cam.transform;
                    Debug.Log("[Monke Mod] Camera component found");
                }
            }

            if (camTransform == null)
            {
                Debug.LogError("[Monke Mod] NO Camera component found in prefab");
                return;
            }

            // Flip camera 180°
            flipped = !flipped;
            camTransform.localRotation = flipped
                ? Quaternion.Euler(0f, 180f, 0f)
                : Quaternion.identity;
        }

        public void Start()
        {
            gameObject.layer = 18;

            buttonRenderer = GetComponent<MeshRenderer>();
            unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white };
            pressedMaterial = new Material(buttonRenderer.material) { color = Color.red };
        }
    }
}
