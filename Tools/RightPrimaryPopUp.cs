using System.Collections.Generic;
using System.Linq;
using GorillaLocomotion;
using UnityEngine;
using Valve.VR;

namespace GoatCamMod.Tools;

public class RightPrimaryPopUp : MonoBehaviour
{
    private const float DoubleClickWindow = 2f;
    private const float RightOffset       = 0.3f;

    private const float SpawnDistance = 1.5f;
    private       bool  cachedPrimaryDown;
    private       int   clickCount;
    private       bool  currentPrimaryDown;

    private float             firstClickTime;
    private FirstPersonButton fpButton;

    private GameObject goatCamModObject;

    public  Dictionary<Transform, Vector3> StoredPositions;
    private ThirdPersonButton              tpButton;

    private void Start()
    {
        goatCamModObject = GameObject.Find("GoatCameraModModelBetter(Clone)");
        fpButton         = FindObjectOfType<FirstPersonButton>();
        tpButton         = FindObjectOfType<ThirdPersonButton>();
    }

    private void Update()
    {
        currentPrimaryDown = SteamVR_Actions.gorillaTag_RightPrimaryClick
                                            .GetState(SteamVR_Input_Sources.RightHand);

        if (PrimaryTriggered())
            switch (clickCount)
            {
                case 0:
                    clickCount     = 1;
                    firstClickTime = Time.time;

                    break;

                case 1 when Time.time - firstClickTime <= DoubleClickWindow:
                {
                    clickCount = 0;

                    MoveGoatCamModObjectInFrontOfPlayer();
                    ResetCameraMode();

                    if (fpButton != null && GTPlayer.Instance != null)
                        fpButton.DisableFirstPerson();

                    if (tpButton != null && ThirdPersonButton.ThirdPersonActive)
                        tpButton.DisableThirdPerson();

                    break;
                }
            }

        if (clickCount == 1 && Time.time - firstClickTime > DoubleClickWindow)
            clickCount = 0;
    }

    private bool PrimaryTriggered()
    {
        bool result = cachedPrimaryDown != currentPrimaryDown && currentPrimaryDown;
        cachedPrimaryDown = currentPrimaryDown;

        return result;
    }

    private void MoveGoatCamModObjectInFrontOfPlayer()
    {
        if (goatCamModObject == null || Camera.main == null)
            return;

        Transform head = Camera.main.transform;

        Vector3 spawnPos =
                head.position                +
                head.forward * SpawnDistance +
                head.right   * RightOffset;

        Quaternion spawnRot =
                Quaternion.Euler(0f, head.eulerAngles.y + 180f, 0f);

        goatCamModObject.transform.position = spawnPos;
        goatCamModObject.transform.rotation = spawnRot;
    }

    private void ResetCameraMode()
    {
        Camera cam = transform.root.GetComponentInChildren<Camera>(true);

        if (cam == null)
            return;

        Transform prefabRoot = cam.transform.root;
        prefabRoot.rotation = Quaternion.identity;
    }

    public void RestorePositions()
    {
        if (StoredPositions == null)
            return;

        foreach (KeyValuePair<Transform, Vector3> pair in StoredPositions.Where(pair => pair.Key != null))
            pair.Key.position = pair.Value;
    }
}