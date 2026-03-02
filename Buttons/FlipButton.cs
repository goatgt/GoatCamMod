using UnityEngine;

namespace GoatCamMod;

public class FlipButton : GorillaPressableButton
{
    private Transform camTransform;
    private bool      flipped;

    public void Start()
    {
        gameObject.layer = 18;

        buttonRenderer    = GetComponent<MeshRenderer>();
        unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white, };
        pressedMaterial   = new Material(buttonRenderer.material) { color = Color.red, };
    }

    public override void ButtonActivation()
    {
        base.ButtonActivation();

        Debug.Log("[Monke Mod] Pressed Button");

        isOn = !isOn;
        UpdateColor();

        if (camTransform == null)
        {
            Camera cam = transform.root.GetComponentInChildren<Camera>(true);
            if (cam != null)
            {
                camTransform = cam.transform;
                Debug.Log("[Monke Mod] Camera component found");
            }
        }

        if (camTransform == null)
        {
            Debug.LogError("[Monke Mod] NO Camera component found in prefab");

            return;
        }

        flipped = !flipped;
        camTransform.localRotation = flipped
                                             ? Quaternion.Euler(0f, 180f, 0f)
                                             : Quaternion.identity;
    }
}