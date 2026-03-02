using UnityEngine;

namespace GoatCamMod;

public class DisconnectButton : GorillaPressableButton
{
    private void Start()
    {
        gameObject.layer = 18;

        buttonRenderer = GetComponent<MeshRenderer>();

        unpressedMaterial = new Material(buttonRenderer.material)
        {
                color = Color.white,
        };

        pressedMaterial = new Material(buttonRenderer.material)
        {
                color = Color.red,
        };
    }

    public override void ButtonActivation()
    {
        Debug.Log("[GoatCamMod] Disconnect Button Pressed");

        if (NetworkSystem.Instance.InRoom)
            NetworkSystem.Instance.ReturnToSinglePlayer();
    }
}