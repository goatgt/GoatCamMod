using System;
using GorillaLocomotion;
using UnityEngine;

namespace GoatCamMod;

public class FirstPersonButton : GorillaPressableButton
{
    public enum SmoothingMode
    {
        Low,
        Medium,
        High,
    }

    public static  SmoothingMode currentMode = SmoothingMode.Low;
    private static TextMesh      smoothingText;

    private MeshRenderer buttonRenderer;
    private Camera       cam;
    private GameObject   goatCamModObject;

    private Transform head;
    private bool      isFirstPerson;

    public  Action     OnFirstPersonPressed;
    private Vector3    originalLocalPosition;
    private Quaternion originalLocalRotation;

    private     Transform  originalParent;
    private new Material   pressedMaterial;
    private     Quaternion targetRotation;
    private new Material   unpressedMaterial;

    public void Start()
    {
        gameObject.layer = 18;

        goatCamModObject = GameObject.Find("GoatCameraModModelBetter(Clone)");

        GameObject textObj = GameObject.Find("Sample Textmesh (29)");
        if (textObj != null)
        {
            smoothingText = textObj.GetComponent<TextMesh>();
            UpdateSmoothingText();
        }

        buttonRenderer    = GetComponent<MeshRenderer>();
        unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white, };
        pressedMaterial   = new Material(buttonRenderer.material) { color = Color.red, };
    }

    private void LateUpdate()
    {
        if (!isFirstPerson || cam == null || head == null)
            return;

        cam.transform.position = head.position;
        targetRotation         = head.rotation;

        float smoothing = GetSmoothingValue();
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, targetRotation, smoothing * Time.deltaTime);
    }

    public override void ButtonActivation()
    {
        base.ButtonActivation();

        isOn = !isOn;
        UpdateColor();

        if (goatCamModObject != null)
            goatCamModObject.transform.position = Vector3.zero;

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

        head = GTPlayer.Instance.headCollider.transform;
        if (head == null)
        {
            Debug.LogError("[Monke Mod] Could not find the local player's head");

            return;
        }

        if (ThirdPersonButton.ThirdPersonActive && !isFirstPerson)
        {
            Debug.Log("[Monke Mod] Cannot enter first-person while third-person is active");

            return;
        }

        if (!isFirstPerson)
        {
            cam.transform.SetParent(null);
            cam.transform.position = head.position;

            isFirstPerson = true;
            OnFirstPersonPressed?.Invoke();
        }
        else
        {
            DisableFirstPerson();
        }
    }

    public void DisableFirstPerson()
    {
        if (!isFirstPerson || cam == null) return;

        cam.transform.SetParent(originalParent);
        cam.transform.localPosition = originalLocalPosition;
        cam.transform.localRotation = originalLocalRotation;

        isFirstPerson = false;
        UpdateColor();
    }

    public static void CycleSmoothingModeForward()
    {
        currentMode = currentMode switch
                      {
                              SmoothingMode.Low    => SmoothingMode.Medium,
                              SmoothingMode.Medium => SmoothingMode.High,
                              SmoothingMode.High   => SmoothingMode.Low,
                              var _                => currentMode,
                      };

        UpdateSmoothingText();
    }

    public static void CycleSmoothingModeBackward()
    {
        currentMode = currentMode switch
                      {
                              SmoothingMode.Low    => SmoothingMode.High,
                              SmoothingMode.Medium => SmoothingMode.Low,
                              SmoothingMode.High   => SmoothingMode.Medium,
                              var _                => currentMode,
                      };

        UpdateSmoothingText();
    }

    private static void UpdateSmoothingText()
    {
        if (smoothingText != null)
            smoothingText.text = currentMode.ToString().ToUpper();
    }

    private void UpdateColor()
    {
        if (buttonRenderer == null)
            return;

        buttonRenderer.material = isOn ? pressedMaterial : unpressedMaterial;
    }

    private static float GetSmoothingValue() =>
            currentMode switch
            {
                    SmoothingMode.Low    => 1000f,
                    SmoothingMode.Medium => 10f,
                    SmoothingMode.High   => 5f,
                    var _                => 10f,
            };
}