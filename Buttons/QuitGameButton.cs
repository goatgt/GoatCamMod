using UnityEngine;

namespace GoatCamMod;

public class QuitGameButton : GorillaPressableButton
{
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
    }

    public override void ButtonActivation()
    {
        Debug.Log("[GoatCamMod] Quit Game Button Pressed");

        QuitGame();
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
            Debug.Log("[GoatCamMod] Stopping Play Mode (Editor)");
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Debug.Log("[GoatCamMod] Quitting Application");
        Application.Quit();
#endif
    }
}