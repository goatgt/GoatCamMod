using System.Collections.Generic;
using UnityEngine;

namespace GoatCamMod
{
    public class timechangebuttonright : GorillaPressableButton
    {
        public static int currentIndex = 0;

        private TextMesh timeText;

        private readonly List<int> timeCycle = new List<int>()
        {
            3,  // Day
            7,  // Evening
            0   // Night
        };

        private readonly List<string> timeNames = new List<string>()
        {
            "DAY",
            "EVENING",
            "NIGHT"
        };

        public void Start()
        {
            gameObject.layer = 18;

            buttonRenderer = GetComponent<MeshRenderer>();
            unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white };
            pressedMaterial = new Material(buttonRenderer.material) { color = Color.red };

            // Find the TextMesh by name anywhere in the scene
            TextMesh[] allTextMeshes = GameObject.FindObjectsOfType<TextMesh>(true);

            foreach (TextMesh tm in allTextMeshes)
            {
                if (tm.gameObject.name == "Sample Textmesh (28)")
                {
                    timeText = tm;
                    timeText.text = "TIME";
                    Debug.Log("[Monke Mod] Found Sample Textmesh (28)");
                    break;
                }
            }

            if (timeText == null)
                Debug.LogError("[Monke Mod] Could not find Sample Textmesh (28)");
        }

        public override void ButtonActivation()
        {
            base.ButtonActivation();

            isOn = !isOn;
            UpdateColor();

            // Move forward
            currentIndex = (currentIndex + 1) % timeCycle.Count;

            ApplyTime();
        }

        private void ApplyTime()
        {
            BetterDayNightManager.instance.SetTimeOfDay(timeCycle[currentIndex]);

            if (timeText != null)
            {
                timeText.text = timeNames[currentIndex];
                Debug.Log("[Monke Mod] Updated text to: " + timeNames[currentIndex]);
            }
        }
    }
}
