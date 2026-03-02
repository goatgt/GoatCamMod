using UnityEngine;

namespace GoatCamMod;

public class BlueColorButton : GorillaPressableButton
{
    private const int          MaxDigit = 9;
    private       int          blueDigit;
    private new   MeshRenderer buttonRenderer;

    private     TextMesh colorTextMesh;
    private new Material pressedMaterial;
    private new Material unpressedMaterial;

    private void Start()
    {
        gameObject.layer = 18;

        buttonRenderer = GetComponent<MeshRenderer>();
        Material unpressed = new(buttonRenderer.material) { color = Color.white, };
        Material pressed   = new(buttonRenderer.material) { color = Color.blue, };
        unpressedMaterial = unpressed;
        pressedMaterial   = pressed;

        colorTextMesh = null;
        TextMesh[] allTextMeshes = FindObjectsOfType<TextMesh>();
        foreach (TextMesh tm in allTextMeshes)
            if (tm.gameObject.name == "Sample Textmesh (31)")
            {
                colorTextMesh = tm;

                break;
            }

        float savedBlue = PlayerPrefs.GetFloat("blueValue", 0f);
        blueDigit = Mathf.RoundToInt(savedBlue * MaxDigit);

        if (colorTextMesh != null)
            colorTextMesh.text = blueDigit.ToString();

        float redValue   = PlayerPrefs.GetFloat("redValue",   0f);
        float greenValue = PlayerPrefs.GetFloat("greenValue", 0f);
        GorillaTagger.Instance.UpdateColor(redValue, greenValue, savedBlue);

        buttonRenderer.material = unpressedMaterial;
    }

    public override void ButtonActivation()
    {
        blueDigit = (blueDigit + 1) % (MaxDigit + 1);

        if (colorTextMesh != null)
            colorTextMesh.text = blueDigit.ToString();

        float blueValue = (float)blueDigit / MaxDigit;

        float redValue   = PlayerPrefs.GetFloat("redValue",   0f);
        float greenValue = PlayerPrefs.GetFloat("greenValue", 0f);

        GorillaTagger.Instance.UpdateColor(redValue, greenValue, blueValue);

        PlayerPrefs.SetFloat("blueValue", blueValue);
    }
}