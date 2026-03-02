using UnityEngine;

namespace GoatCamMod
{
    public class fovbuttonup : GorillaPressableButton
    {
        private Camera cam;
        private TextMesh fovText;

        public override void ButtonActivation()
        {
            base.ButtonActivation();

            Debug.Log("[Monke Mod] Pressed FOV Up Button");

            isOn = !isOn;
            UpdateColor();

            // Find Camera component anywhere in this prefab (once)
            if (cam == null)
            {
                cam = transform.root.GetComponentInChildren<Camera>(true);
                if (cam != null)
                {
                    Debug.Log("[Monke Mod] Camera component found");
                }
            }

            if (cam == null)
            {
                Debug.LogError("[Monke Mod] NO Camera component found in prefab");
                return;
            }

            // Increase FOV by 5, but cap at 120
            cam.fieldOfView = Mathf.Min(cam.fieldOfView + 5f, 120f);
            Debug.Log("[Monke Mod] FOV set to: " + cam.fieldOfView);

            // Update the TextMesh text to show only the number
            if (fovText != null)
            {
                fovText.text = Mathf.RoundToInt(cam.fieldOfView).ToString();
            }
        }

        public void Start()
        {
            gameObject.layer = 18;

            buttonRenderer = GetComponent<MeshRenderer>();
            unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white };
            pressedMaterial = new Material(buttonRenderer.material) { color = Color.red };

            // Find the TextMesh component on the object named "Sample Textmesh (9)"
            GameObject textObj = GameObject.Find("Sample Textmesh (9)");
            if (textObj != null)
            {
                fovText = textObj.GetComponent<TextMesh>();
                if (fovText == null)
                {
                    Debug.LogError("[Monke Mod] Sample Textmesh (9) does not have a TextMesh component!");
                }
            }
            else
            {
                Debug.LogError("[Monke Mod] Could not find GameObject named 'Sample Textmesh (9)'");
            }
        }
    }
}
