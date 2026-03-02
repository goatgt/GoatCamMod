using UnityEngine;
using GorillaLocomotion;

namespace GoatCamMod
{
    public class bluecolorbutton : GorillaPressableButton
    {
        private MeshRenderer buttonRenderer;
        private Material unpressedMaterial;
        private Material pressedMaterial;

        private TextMesh colorTextMesh;
        private int blueDigit = 0;
        private const int MAX_DIGIT = 9;

        public override void ButtonActivation()
        {
            // Increment blue digit 0-9
            blueDigit = (blueDigit + 1) % (MAX_DIGIT + 1);

            // Update TextMesh
            if (colorTextMesh != null)
                colorTextMesh.text = blueDigit.ToString();

            // Convert to 0-1 float
            float blueValue = (float)blueDigit / MAX_DIGIT;

            // Get current red and green from PlayerPrefs
            float redValue = PlayerPrefs.GetFloat("redValue", 0f);
            float greenValue = PlayerPrefs.GetFloat("greenValue", 0f);

            // Apply new color
            GorillaTagger.Instance.UpdateColor(redValue, greenValue, blueValue);

            // Save blue value
            PlayerPrefs.SetFloat("blueValue", blueValue);
        }

        private void Start()
        {
            this.gameObject.layer = 18;

            // Button visuals
            buttonRenderer = GetComponent<MeshRenderer>();
            Material unpressed = new Material(buttonRenderer.material) { color = Color.white };
            Material pressed = new Material(buttonRenderer.material) { color = Color.blue };
            unpressedMaterial = unpressed;
            pressedMaterial = pressed;

            // Find TextMesh
            colorTextMesh = null;
            TextMesh[] allTextMeshes = GameObject.FindObjectsOfType<TextMesh>();
            foreach (TextMesh tm in allTextMeshes)
            {
                if (tm.gameObject.name == "Sample Textmesh (31)")
                {
                    colorTextMesh = tm;
                    break;
                }
            }

            // Initialize blueDigit from PlayerPrefs
            float savedBlue = PlayerPrefs.GetFloat("blueValue", 0f);
            blueDigit = Mathf.RoundToInt(savedBlue * MAX_DIGIT);

            if (colorTextMesh != null)
                colorTextMesh.text = blueDigit.ToString();

            // Apply initial color with saved red/green
            float redValue = PlayerPrefs.GetFloat("redValue", 0f);
            float greenValue = PlayerPrefs.GetFloat("greenValue", 0f);
            GorillaTagger.Instance.UpdateColor(redValue, greenValue, savedBlue);

            buttonRenderer.material = unpressedMaterial;
        }
    }
}
