using UnityEngine;

namespace GoatCamMod;

public class NearClipButtonDown : GorillaPressableButton
{
    private Camera   cam;
    private TextMesh nearText;

    public void Start()
    {
        gameObject.layer = 18;

        buttonRenderer    = GetComponent<MeshRenderer>();
        unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white, };
        pressedMaterial   = new Material(buttonRenderer.material) { color = Color.red, };

        GameObject textObj = GameObject.Find("Sample Textmesh (7)");
        if (textObj != null)
        {
            nearText = textObj.GetComponent<TextMesh>();
            if (nearText == null)
                Debug.LogError("[Monke Mod] Sample Textmesh (7) does not have a TextMesh component!");
        }
        else
        {
            Debug.LogError("[Monke Mod] Could not find GameObject named 'Sample Textmesh (7)'");
        }
    }

    public override void ButtonActivation()
    {
        base.ButtonActivation();

        Debug.Log("[Monke Mod] Pressed Near Down Button");

        isOn = !isOn;
        UpdateColor();

        if (cam == null)
        {
            cam = transform.root.GetComponentInChildren<Camera>(true);
            if (cam != null)
                Debug.Log("[Monke Mod] Camera component found");
        }

        if (cam == null)
        {
            Debug.LogError("[Monke Mod] NO Camera component found in prefab");

            return;
        }

        cam.nearClipPlane = Mathf.Clamp(cam.nearClipPlane - 0.01f, 0.01f, 0.1f);
        Debug.Log("[Monke Mod] Near Clip Plane set to: " + cam.nearClipPlane);

        if (nearText != null)
            nearText.text = cam.nearClipPlane.ToString("F2");
    }
}