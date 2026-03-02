using UnityEngine;

namespace GoatCamMod;

public class SmoothingButtonDown : GorillaPressableButton
{
    private new MeshRenderer buttonRenderer;
    private new Material     pressedMaterial;
    private new Material     unpressedMaterial;

    public void Start()
    {
        gameObject.layer = 18;

        buttonRenderer    = GetComponent<MeshRenderer>();
        unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white, };
        pressedMaterial   = new Material(buttonRenderer.material) { color = Color.red, };
    }

    public override void ButtonActivation()
    {
        isOn = !isOn;
        UpdateColor();

        FirstPersonButton.CycleSmoothingModeBackward();
    }

    private void UpdateColor()
    {
        if (buttonRenderer == null) return;
        buttonRenderer.material = isOn ? pressedMaterial : unpressedMaterial;
    }
}