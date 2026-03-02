using GorillaLocomotion;
using UnityEngine;

namespace GoatCamMod;

public class ThirdPersonButton : GorillaPressableButton
{
    public static bool ThirdPersonActive;

    public float   SmoothingSpeed    = 10f;
    public Vector3 ThirdPersonOffset = new(0f, 0.2f, -1.5f);

    private new MeshRenderer      buttonRenderer;
    private     Camera            cam;
    private     FirstPersonButton fpButton;

    private GameObject goatCamModObject;
    private bool       isThirdPerson;
    private Vector3    originalLocalPosition;
    private Quaternion originalLocalRotation;

    private     Transform originalParent;
    private new Material  pressedMaterial;
    private new Material  unpressedMaterial;

    public void Start()
    {
        gameObject.layer = 18;

        buttonRenderer    = GetComponent<MeshRenderer>();
        unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white, };
        pressedMaterial   = new Material(buttonRenderer.material) { color = Color.red, };

        goatCamModObject = GameObject.Find("GoatCameraModModelBetter(Clone)");
        fpButton         = FindObjectOfType<FirstPersonButton>();
    }

    public override void ButtonActivation()
    {
        base.ButtonActivation();

        isOn = !isOn;
        UpdateColor();

        if (cam == null)
        {
            cam = transform.root.GetComponentInChildren<Camera>(true);
            if (cam == null)
            {
                Debug.LogError("[Monke Mod] No Camera found in prefab");

                return;
            }

            originalParent        = cam.transform.parent;
            originalLocalPosition = cam.transform.localPosition;
            originalLocalRotation = cam.transform.localRotation;
        }

        Transform head = GTPlayer.Instance.headCollider.transform;
        if (head == null)
        {
            Debug.LogError("[Monke Mod] Could not find the local player's head");

            return;
        }

        if (!isThirdPerson)
        {
            if (fpButton != null)
                fpButton.DisableFirstPerson();

            cam.transform.SetParent(head);
            cam.transform.localPosition = ThirdPersonOffset;
            cam.transform.localRotation = Quaternion.identity;

            if (goatCamModObject != null)
                goatCamModObject.transform.position = Vector3.zero;

            isThirdPerson     = true;
            ThirdPersonActive = true;
            Debug.Log("[Monke Mod] Camera switched to third-person and moved object to (0,0,0)");
        }
        else
        {
            DisableThirdPerson();
        }
    }

    public void DisableThirdPerson()
    {
        if (!isThirdPerson || cam == null) return;

        cam.transform.SetParent(originalParent);
        cam.transform.localPosition = originalLocalPosition;
        cam.transform.localRotation = originalLocalRotation;

        isThirdPerson     = false;
        ThirdPersonActive = false;
        UpdateColor();
        Debug.Log("[Monke Mod] Third-person disabled");
    }

    private void UpdateColor()
    {
        if (buttonRenderer == null)
            return;

        buttonRenderer.material = isOn ? pressedMaterial : unpressedMaterial;
    }
}