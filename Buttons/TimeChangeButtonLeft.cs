using System.Collections.Generic;
using UnityEngine;

namespace GoatCamMod;

public class TimeChangeButtonLeft : GorillaPressableButton
{
    private readonly List<int> timeCycle = new()
    {
            3,
            7,
            0,
    };

    private readonly List<string> timeNames = new()
    {
            "DAY",
            "EVENING",
            "NIGHT",
    };

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
                timeText = tm;

                break;
            }
    }

    public override void ButtonActivation()
    {
        base.ButtonActivation();

        isOn = !isOn;
        UpdateColor();

        TimeChangeButtonRight.CurrentIndex--;
        if (TimeChangeButtonRight.CurrentIndex < 0)
            TimeChangeButtonRight.CurrentIndex = timeCycle.Count - 1;

        BetterDayNightManager.instance.SetTimeOfDay(
                timeCycle[TimeChangeButtonRight.CurrentIndex]);

        if (timeText != null)
            timeText.text = timeNames[TimeChangeButtonRight.CurrentIndex];
    }
}