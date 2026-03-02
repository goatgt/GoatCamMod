using UnityEngine;

namespace GoatCamMod;

public class GreenColorButton : GorillaPressableButton
{
    private const int          MaxDigit = 9;
    private new   MeshRenderer buttonRenderer;

    private     TextMesh colorTextMesh;
    private     int      greenDigit;
    private new Material pressedMaterial;
    private new Material unpressedMaterial;

    private void Start()
    {
        gameObject.layer = 18;

        buttonRenderer = GetComponent<MeshRenderer>();
        Material unpressed = new(buttonRenderer.material) { color = Color.white, };
        Material pressed   = new(buttonRenderer.material) { color = Color.green, };
        unpressedMaterial = unpressed;
        pressedMaterial   = pressed;

        colorTextMesh = null;
        TextMesh[] allTextMeshes = FindObjectsOfType<TextMesh>();
        foreach (TextMesh tm in allTextMeshes)
        {
            if (tm.gameObject.name != "Sample Textmesh (30)")
                continue;

            colorTextMesh = tm;

            break;
        }

        float savedGreen = PlayerPrefs.GetFloat("greenValue", 0f);
        greenDigit = Mathf.RoundToInt(savedGreen * MaxDigit);

        if (colorTextMesh != null)
            colorTextMesh.text = greenDigit.ToString();

        float redValue  = PlayerPrefs.GetFloat("redValue",  0f);
        float blueValue = PlayerPrefs.GetFloat("blueValue", 0f);
        GorillaTagger.Instance.UpdateColor(redValue, savedGreen, blueValue);

        buttonRenderer.material = unpressedMaterial;
    }

    public override void ButtonActivation()
    {
        greenDigit = (greenDigit + 1) % (MaxDigit + 1);

        if (colorTextMesh != null)
            colorTextMesh.text = greenDigit.ToString();

        float greenValue = (float)greenDigit / MaxDigit;

        float redValue  = PlayerPrefs.GetFloat("redValue",  0f);
        float blueValue = PlayerPrefs.GetFloat("blueValue", 0f);

        GorillaTagger.Instance.UpdateColor(redValue, greenValue, blueValue);

        PlayerPrefs.SetFloat("greenValue", greenValue);
    }
}