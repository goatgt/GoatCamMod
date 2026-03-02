using System.Collections.Generic;
using UnityEngine;

namespace GoatCamMod
{
    public class timechangebuttonleft : GorillaPressableButton
    {
        private TextMesh timeText;

        private readonly List<int> timeCycle = new List<int>()
        {
            3,
            7,
            0
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

            TextMesh[] allTextMeshes = GameObject.FindObjectsOfType<TextMesh>(true);

            foreach (TextMesh tm in allTextMeshes)
            {
                if (tm.gameObject.name == "Sample Textmesh (28)")
                {
                    timeText = tm;
                    break;
                }
            }
        }

        public override void ButtonActivation()
        {
            base.ButtonActivation();

            isOn = !isOn;
            UpdateColor();

            // Move backward
            timechangebuttonright.currentIndex--;
            if (timechangebuttonright.currentIndex < 0)
                timechangebuttonright.currentIndex = timeCycle.Count - 1;

            BetterDayNightManager.instance.SetTimeOfDay(
                timeCycle[timechangebuttonright.currentIndex]);

            if (timeText != null)
            {
                timeText.text = timeNames[timechangebuttonright.currentIndex];
            }
        }
    }
}
