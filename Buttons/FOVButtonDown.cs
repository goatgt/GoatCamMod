using UnityEngine;

namespace GoatCamMod;

public class FovButtonDown : GorillaPressableButton
{
    private Camera   cam;
    private TextMesh fovText;

    public void Start()
    {
        gameObject.layer = 18;

        buttonRenderer    = GetComponent<MeshRenderer>();
        unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white, };
        pressedMaterial   = new Material(buttonRenderer.material) { color = Color.red, };

        GameObject textObj = GameObject.Find("Sample Textmesh (9)");
        if (textObj != null)
        {
            fovText = textObj.GetComponent<TextMesh>();
            if (fovText == null)
                Debug.LogError("[Monke Mod] Sample Textmesh (9) does not have a TextMesh component!");
        }
        else
        {
            Debug.LogError("[Monke Mod] Could not find GameObject named 'Sample Textmesh (9)'");
        }
    }

    public override void ButtonActivation()
    {
        base.ButtonActivation();

        Debug.Log("[Monke Mod] Pressed FOV Down Button");

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

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - 5f, 30f, 120f);
        Debug.Log("[Monke Mod] FOV set to: " + cam.fieldOfView);

        if (fovText != null)
            fovText.text = Mathf.RoundToInt(cam.fieldOfView).ToString();
    }
}