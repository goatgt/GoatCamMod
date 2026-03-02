using UnityEngine;

namespace GoatCamMod;

public class GuitGameMenuButton : GorillaPressableButton
{
    private GameObject quitGameMenu;

    private void Start()
    {
        gameObject.layer = 18;

        buttonRenderer = GetComponent<MeshRenderer>();

        Material unpressedMat = new(buttonRenderer.material)
        {
                color = Color.white,
        };

        Material pressedMat = new(buttonRenderer.material)
        {
                color = Color.red,
        };

        unpressedMaterial = unpressedMat;
        pressedMaterial   = pressedMat;

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
            if (obj.name == "QuitGameMenu")
            {
                quitGameMenu = obj;

                break;
            }

        if (quitGameMenu != null)
        {
            quitGameMenu.SetActive(false);
            Debug.Log("[GoatCamMod] QuitGameMenu found successfully.");
        }
        else
        {
            Debug.LogError("[GoatCamMod] Could not find QuitGameMenu anywhere in scene!");
        }
    }

    public override void ButtonActivation()
    {
        Debug.Log("[GoatCamMod] Quit Menu Button Pressed");

        isOn = !isOn;
        UpdateColor();

        if (quitGameMenu != null)
            quitGameMenu.SetActive(isOn);
        else
            Debug.LogError("[GoatCamMod] QuitGameMenu is STILL NULL!");
    }
}