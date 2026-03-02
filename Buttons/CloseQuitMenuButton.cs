using UnityEngine;

namespace GoatCamMod;

public class CloseQuitMenuButton : GorillaPressableButton
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

        if (quitGameMenu == null)
            Debug.LogError("[GoatCamMod] Could not find QuitGameMenu in scene!");
    }

    public override void ButtonActivation()
    {
        Debug.Log("[GoatCamMod] Close Quit Menu Button Pressed");

        if (quitGameMenu != null)
            quitGameMenu.SetActive(false);
        else
            Debug.LogError("[GoatCamMod] QuitGameMenu is NULL!");
    }
}