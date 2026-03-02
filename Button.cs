using UnityEngine;

namespace GoatCamMod;

public class Button : GorillaPressableButton
{
    public void Start()
    {
        gameObject.layer = 18;

        buttonRenderer = GetComponent<MeshRenderer>();
        Material unpressedMat = new(buttonRenderer.material) { color = Color.white, };
        Material pressedMat   = new(buttonRenderer.material) { color = Color.red, };
        unpressedMaterial = unpressedMat;
        pressedMaterial   = pressedMat;
    }

    public override void ButtonActivation()
    {
        Debug.Log("[Monke Mod] Pressed Button");
        isOn = !isOn;
        UpdateColor();
    }
}