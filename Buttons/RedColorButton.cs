using UnityEngine;
using GorillaLocomotion;

namespace GoatCamMod
{
    public class redcolorbutton : GorillaPressableButton
    {
        private MeshRenderer buttonRenderer;
        private Material unpressedMaterial;
        private Material pressedMaterial;

        private TextMesh colorTextMesh;
        private int redDigit = 0;
        private const int MAX_DIGIT = 9;

        public override void ButtonActivation()
        {
            // Increment red digit 0-9
            redDigit = (redDigit + 1) % (MAX_DIGIT + 1);

            // Update TextMesh
            if (colorTextMesh != null)
                colorTextMesh.text = redDigit.ToString();

            // Convert to 0-1 float
            float redValue = (float)redDigit / MAX_DIGIT;

            // Get current green and blue from PlayerPrefs
            float greenValue = PlayerPrefs.GetFloat("greenValue", 0f);
            float blueValue = PlayerPrefs.GetFloat("blueValue", 0f);

            // Apply new color
            GorillaTagger.Instance.UpdateColor(redValue, greenValue, blueValue);

            // Save red value
            PlayerPrefs.SetFloat("redValue", redValue);
        }

        private void Start()
        {
            this.gameObject.layer = 18;

            // Button visuals
            buttonRenderer = GetComponent<MeshRenderer>();
            Material unpressed = new Material(buttonRenderer.material) { color = Color.white };
            Material pressed = new Material(buttonRenderer.material) { color = Color.red };
            unpressedMaterial = unpressed;
            pressedMaterial = pressed;

            // Find TextMesh
            colorTextMesh = null;
            TextMesh[] allTextMeshes = GameObject.FindObjectsOfType<TextMesh>();
            foreach (TextMesh tm in allTextMeshes)
            {
                if (tm.gameObject.name == "Sample Textmesh (25)")
                {
                    colorTextMesh = tm;
                    break;
                }
            }

            // Initialize redDigit from PlayerPrefs
            float savedRed = PlayerPrefs.GetFloat("redValue", 0f);
            redDigit = Mathf.RoundToInt(savedRed * MAX_DIGIT);

            if (colorTextMesh != null)
                colorTextMesh.text = redDigit.ToString();

            // Apply initial color with saved green/blue
            float greenValue = PlayerPrefs.GetFloat("greenValue", 0f);
            float blueValue = PlayerPrefs.GetFloat("blueValue", 0f);
            GorillaTagger.Instance.UpdateColor(savedRed, greenValue, blueValue);

            buttonRenderer.material = unpressedMaterial;
        }
    }
}
