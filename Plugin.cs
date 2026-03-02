using System;
using System.Threading.Tasks;
using BepInEx;
using GoatCamMod.Tools;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.XR;

namespace GoatCamMod;

[BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
public class Plugin : BaseUnityPlugin
{
    private Camera     goatCam;
    private GameObject gsdPrefab;
    private Camera     shoulderCam;

    private void Awake()
    {
        GorillaTagger.OnPlayerSpawned(async void () =>
                                      {
                                          try
                                          {
                                              await SetupCam();
                                          }
                                          catch (Exception e)
                                          {
                                              // Ignored
                                          }
                                      });
    }

    private void LateUpdate()
    {
        if (goatCam == null || shoulderCam == null)
            return;

        shoulderCam.fieldOfView      = goatCam.fieldOfView;
        shoulderCam.nearClipPlane    = goatCam.nearClipPlane;
        shoulderCam.farClipPlane     = goatCam.farClipPlane;
        shoulderCam.cullingMask      = goatCam.cullingMask;
        shoulderCam.clearFlags       = goatCam.clearFlags;
        shoulderCam.backgroundColor  = goatCam.backgroundColor;
        shoulderCam.allowHDR         = goatCam.allowHDR;
        shoulderCam.allowMSAA        = goatCam.allowMSAA;
        shoulderCam.orthographic     = goatCam.orthographic;
        shoulderCam.orthographicSize = goatCam.orthographicSize;
    }

    private static void AddComponent<T>(Transform parent, string name) where T : Component
    {
        Transform button = parent.Find(name);

        if (button != null)
            button.gameObject.AddComponent<T>();
        else
            Debug.LogWarning($"[GoatCam]: Could not find {name}");
    }

    private async Task SetupCam()
    {
        try
        {
            gsdPrefab = await AssetLoader.LoadAsset<GameObject>("GoatCameraModModelBetter");
            if (gsdPrefab == null)
            {
                Debug.LogError("[GoatCam]: Failed to load prefab.");

                return;
            }

            GameObject instance = Instantiate(gsdPrefab);
            instance.SetActive(true);

            instance.transform.position   = new Vector3(-66.8047f, 11.9368f, -82.5664f);
            instance.transform.rotation   = Quaternion.Euler(0f, 229.9595f, 0f);
            instance.transform.localScale = Vector3.one * 0.345887f;

            instance.AddComponent<DevHoldable>();
            instance.AddComponent<RightPrimaryPopUp>();

            Transform root     = instance.transform.Find("GoatCamModObjects");
            Transform menu     = root.Find("Menu");
            Transform quitMenu = menu.Find("QuitGameMenu");

            AddComponent<ThirdPersonButton>(root, "ThirdPersonButton");
            AddComponent<FollowPlayerButton>(root, "FollowPlayerButton");
            AddComponent<FlipButton>(root, "FlipCameraButton");
            AddComponent<FirstPersonButton>(root, "FirstPersonButton");
            AddComponent<NearClipButtonUp>(root, "NearClipButtonRight");
            AddComponent<NearClipButtonDown>(root, "NearClipButtonLeft");
            AddComponent<FovButtonDown>(root, "FOVButtonLeft");
            AddComponent<FovButtonUp>(root, "FOVButtonRight");
            AddComponent<MenuButton>(root, "Idk Yet Button");

            AddComponent<HideNameButton>(menu, "Idk Yet Button (1)");
            AddComponent<SmoothingButtonDown>(menu, "Idk Yet Button (2)");
            AddComponent<SmoothingButtonUp>(menu, "Idk Yet Button (3)");
            AddComponent<TimeChangeButtonLeft>(menu, "Idk Yet Button (4)");
            AddComponent<TimeChangeButtonRight>(menu, "Idk Yet Button (5)");
            AddComponent<HideHeadButton>(menu, "Idk Yet Button (6)");
            AddComponent<DisconnectButton>(menu, "Idk Yet Button (7)");
            AddComponent<GuitGameMenuButton>(menu, "Idk Yet Button (8)");
            AddComponent<RedColorButton>(menu, "Idk Yet Button (9)");
            AddComponent<GreenColorButton>(menu, "Idk Yet Button (13)");
            AddComponent<BlueColorButton>(menu, "Idk Yet Button (14)");
            AddComponent<CaptureButton>(menu, "Idk Yet Button (16)");

            AddComponent<CloseQuitMenuButton>(quitMenu, "Idk Yet Button (16)");
            AddComponent<QuitGameButton>(quitMenu, "Idk Yet Button (17)");

            goatCam = instance.GetComponentInChildren<Camera>();
            if (goatCam == null)
            {
                Debug.LogError("[GoatCam]: Camera not found in prefab.");

                return;
            }

            GameObject shoulderCamObj = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera");
            if (shoulderCamObj == null)
            {
                Debug.LogError("[GoatCam]: Shoulder Camera not found.");

                return;
            }

            shoulderCam = shoulderCamObj.GetComponent<Camera>();

            CinemachineBrain camBrain = shoulderCamObj.GetComponent<CinemachineBrain>();
            if (camBrain != null)
                camBrain.enabled = false;

            Transform vcam = shoulderCamObj.transform.Find("CM vcam1");
            if (vcam != null)
                vcam.gameObject.SetActive(false);

            shoulderCamObj.transform.SetParent(goatCam.transform);
            shoulderCamObj.transform.localPosition = Vector3.zero;
            shoulderCamObj.transform.localRotation = Quaternion.identity;

            GameObject lckWallCameraSpawner = GameObject.Find("LCKWallCameraSpawner");
            if (lckWallCameraSpawner != null)
            {
                lckWallCameraSpawner.SetActive(false);
                Debug.Log("[GoatCam]: LCKWallCameraSpawner hidden.");
            }
            else
            {
                Debug.LogWarning("[GoatCam]: LCKWallCameraSpawner not found.");
            }

            Debug.Log("[GoatCam]: Desktop camera fully synced to GoatCam.");

            //Starts in first person when not on vr so you can play on pc
            if (!XRSettings.isDeviceActive)
                FirstPersonButton.Instance.ToggleFirstPerson();
        }
        catch (Exception e)
        {
            Debug.LogError("[GoatCam]: Setup failed - " + e.Message);
        }
    }
}