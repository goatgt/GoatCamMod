using UnityEngine;

namespace GoatCamMod;

public class RedColorButton : GorillaPressableButton
{
    private const int          MaxDigit = 9;
    private new   MeshRenderer buttonRenderer;

    private     TextMesh colorTextMesh;
    private new Material pressedMaterial;
    private     int      redDigit;
    private new Material unpressedMaterial;

    private void Start()
    {
        gameObject.layer = 18;

        buttonRenderer = GetComponent<MeshRenderer>();
        Material unpressed = new(buttonRenderer.material) { color = Color.white, };
        Material pressed   = new(buttonRenderer.material) { color = Color.red, };
        unpressedMaterial = unpressed;
        pressedMaterial   = pressed;

        colorTextMesh = null;
        TextMesh[] allTextMeshes = FindObjectsOfType<TextMesh>();
        foreach (TextMesh tm in allTextMeshes)
        {
            if (tm.gameObject.name != "Sample Textmesh (25)")
                continue;

            colorTextMesh = tm;

            break;
        }

        float savedRed = PlayerPrefs.GetFloat("redValue", 0f);
        redDigit = Mathf.RoundToInt(savedRed * MaxDigit);

        if (colorTextMesh != null)
            colorTextMesh.text = redDigit.ToString();

        float greenValue = PlayerPrefs.GetFloat("greenValue", 0f);
        float blueValue  = PlayerPrefs.GetFloat("blueValue",  0f);
        GorillaTagger.Instance.UpdateColor(savedRed, greenValue, blueValue);

        buttonRenderer.material = unpressedMaterial;
    }

    public override void ButtonActivation()
    {
        redDigit = (redDigit + 1) % (MaxDigit + 1);

        if (colorTextMesh != null)
            colorTextMesh.text = redDigit.ToString();

        float redValue = (float)redDigit / MaxDigit;

        float greenValue = PlayerPrefs.GetFloat("greenValue", 0f);
        float blueValue  = PlayerPrefs.GetFloat("blueValue",  0f);

        GorillaTagger.Instance.UpdateColor(redValue, greenValue, blueValue);

        PlayerPrefs.SetFloat("redValue", redValue);
    }
}