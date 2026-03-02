using UnityEngine;
using GorillaLocomotion;

namespace GoatCamMod
{
    public class greencolorbutton : GorillaPressableButton
    {
        private MeshRenderer buttonRenderer;
        private Material unpressedMaterial;
        private Material pressedMaterial;

        private TextMesh colorTextMesh;
        private int greenDigit = 0;
        private const int MAX_DIGIT = 9;

        public override void ButtonActivation()
        {
            // Increment green digit 0-9
            greenDigit = (greenDigit + 1) % (MAX_DIGIT + 1);

            // Update TextMesh
            if (colorTextMesh != null)
                colorTextMesh.text = greenDigit.ToString();

            // Convert to 0-1 float
            float greenValue = (float)greenDigit / MAX_DIGIT;

            // Get current red and blue from PlayerPrefs
            float redValue = PlayerPrefs.GetFloat("redValue", 0f);
            float blueValue = PlayerPrefs.GetFloat("blueValue", 0f);

            // Apply new color
            GorillaTagger.Instance.UpdateColor(redValue, greenValue, blueValue);

            // Save green value
            PlayerPrefs.SetFloat("greenValue", greenValue);
        }

        private void Start()
        {
            this.gameObject.layer = 18;

            // Button visuals
            buttonRenderer = GetComponent<MeshRenderer>();
            Material unpressed = new Material(buttonRenderer.material) { color = Color.white };
            Material pressed = new Material(buttonRenderer.material) { color = Color.green };
            unpressedMaterial = unpressed;
            pressedMaterial = pressed;

            // Find TextMesh
            colorTextMesh = null;
            TextMesh[] allTextMeshes = GameObject.FindObjectsOfType<TextMesh>();
            foreach (TextMesh tm in allTextMeshes)
            {
                if (tm.gameObject.name == "Sample Textmesh (30)")
                {
                    colorTextMesh = tm;
                    break;
                }
            }

            // Initialize greenDigit from PlayerPrefs
            float savedGreen = PlayerPrefs.GetFloat("greenValue", 0f);
            greenDigit = Mathf.RoundToInt(savedGreen * MAX_DIGIT);

            if (colorTextMesh != null)
                colorTextMesh.text = greenDigit.ToString();

            // Apply initial color with saved red/blue
            float redValue = PlayerPrefs.GetFloat("redValue", 0f);
            float blueValue = PlayerPrefs.GetFloat("blueValue", 0f);
            GorillaTagger.Instance.UpdateColor(redValue, savedGreen, blueValue);

            buttonRenderer.material = unpressedMaterial;
        }
    }
}
