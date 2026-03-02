using UnityEngine;

namespace GoatCamMod;

public class MenuButton : GorillaPressableButton
{
    private GameObject menu;

    private void Start()
    {
        gameObject.layer = 18;

        buttonRenderer = GetComponent<MeshRenderer>();
        Material unpressedMat = new(buttonRenderer.material) { color = Color.white, };
        Material pressedMat   = new(buttonRenderer.material) { color = Color.red, };

        unpressedMaterial = unpressedMat;
        pressedMaterial   = pressedMat;

        menu = transform.root.Find("GoatCamModObjects/Menu")?.gameObject;

        if (menu != null)
            menu.SetActive(false);
        else
            Debug.LogError("[GoatCamMod] Could not find Menu!");
    }

    public override void ButtonActivation()
    {
        Debug.Log("[GoatCamMod] Pressed Button");

        isOn = !isOn;
        UpdateColor();

        if (menu != null)
            menu.SetActive(isOn);
    }
}