using UnityEngine;

namespace GoatCamMod;

public class HideNameButton : GorillaPressableButton
{
    private GameObject nameTagObject;

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

        if (nameTagObject == null)
            nameTagObject = GameObject.Find("body_AnchorFrontRight_NameTag");

        if (nameTagObject != null)
            nameTagObject.SetActive(!isOn);
        else
            Debug.LogWarning("[Monke Mod] Could not find body_AnchorFrontRight_NameTag");
    }
}