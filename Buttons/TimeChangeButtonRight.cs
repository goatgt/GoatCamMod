using System.Collections.Generic;
using UnityEngine;

namespace GoatCamMod;

public class TimeChangeButtonRight : GorillaPressableButton
{
    public static int CurrentIndex;

    private readonly List<int> timeCycle =
    [
            3,
            7,
            0,
    ];

    private readonly List<string> timeNames =
    [
            "DAY",
            "EVENING",
            "NIGHT",
    ];

    private TextMesh timeText;

    public void Start()
    {
        gameObject.layer = 18;

        buttonRenderer    = GetComponent<MeshRenderer>();
        unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white, };
        pressedMaterial   = new Material(buttonRenderer.material) { color = Color.red, };

        TextMesh[] allTextMeshes = FindObjectsOfType<TextMesh>(true);

        foreach (TextMesh tm in allTextMeshes)
            if (tm.gameObject.name == "Sample Textmesh (28)")
            {
                timeText      = tm;
                timeText.text = "TIME";
                Debug.Log("[Monke Mod] Found Sample Textmesh (28)");

                break;
            }

        if (timeText == null)
            Debug.LogError("[Monke Mod] Could not find Sample Textmesh (28)");
    }

    public override void ButtonActivation()
    {
        base.ButtonActivation();

        isOn = !isOn;
        UpdateColor();

        CurrentIndex = (CurrentIndex + 1) % timeCycle.Count;

        ApplyTime();
    }

    private void ApplyTime()
    {
        BetterDayNightManager.instance.SetTimeOfDay(timeCycle[CurrentIndex]);

        if (timeText == null)
            return;

        timeText.text = timeNames[CurrentIndex];
        Debug.Log("[Monke Mod] Updated text to: " + timeNames[CurrentIndex]);
    }
}