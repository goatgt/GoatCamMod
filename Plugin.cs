using BepInEx;
using Unity.Cinemachine;
using GoatCamMod;
using GoatCamMod.Tools;
using System;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace GoatCamMod
{
    [BepInPlugin(Constants.GUID, Constants.NAME, Constants.VERS)]
    public class Plugin : BaseUnityPlugin
    {
        private GameObject _GSDPrefab;
        private Camera _goatCam;
        private Camera _shoulderCam;

        void Awake()
        {
            GorillaTagger.OnPlayerSpawned(async () => await SetupCam());
        }

        async Task SetupCam()
        {
            try
            {
                _GSDPrefab = await AssetLoader.LoadAsset<GameObject>("GoatCameraModModelBetter");
                if (_GSDPrefab == null)
                {
                    Debug.LogError("[GoatCam]: Failed to load prefab.");
                    return;
                }

                var instance = Instantiate(_GSDPrefab);
                instance.SetActive(true);

                instance.transform.position = new Vector3(-66.8047f, 11.9368f, -82.5664f);
                instance.transform.rotation = Quaternion.Euler(0f, 229.9595f, 0f);
                instance.transform.localScale = Vector3.one * 0.345887f;

                // Attach hold + popup
                instance.AddComponent<DevHoldable>();
                instance.AddComponent<RightPrimaryPopUp>();

                // Attach all buttons
                instance.transform.Find("GoatCamModObjects/ThirdPersonButton").gameObject.AddComponent<thirdpersonbutton>();
                instance.transform.Find("GoatCamModObjects/FollowPlayerButton").gameObject.AddComponent<followplayerbutton>();
                instance.transform.Find("GoatCamModObjects/FlipCameraButton").gameObject.AddComponent<flipbutton>();
                instance.transform.Find("GoatCamModObjects/FirstPersonButton").gameObject.AddComponent<firstpersonbutton>();
                instance.transform.Find("GoatCamModObjects/NearClipButtonRight").gameObject.AddComponent<nearclipbuttonup>();
                instance.transform.Find("GoatCamModObjects/NearClipButtonLeft").gameObject.AddComponent<nearclipbuttondown>();
                instance.transform.Find("GoatCamModObjects/FOVButtonLeft").gameObject.AddComponent<fovbuttondown>();
                instance.transform.Find("GoatCamModObjects/FOVButtonRight").gameObject.AddComponent<fovbuttonup>();
                instance.transform.Find("GoatCamModObjects/Idk Yet Button").gameObject.AddComponent<menubutton>();
                instance.transform.Find("GoatCamModObjects/Menu/Idk Yet Button (1)").gameObject.AddComponent<hidenamebutton>();
                instance.transform.Find("GoatCamModObjects/Menu/Idk Yet Button (2)").gameObject.AddComponent<smoothingbuttondown>();
                instance.transform.Find("GoatCamModObjects/Menu/Idk Yet Button (3)").gameObject.AddComponent<smoothingbuttonup>();
                instance.transform.Find("GoatCamModObjects/Menu/Idk Yet Button (4)").gameObject.AddComponent<timechangebuttonleft>();
                instance.transform.Find("GoatCamModObjects/Menu/Idk Yet Button (5)").gameObject.AddComponent<timechangebuttonright>();
                instance.transform.Find("GoatCamModObjects/Menu/Idk Yet Button (6)").gameObject.AddComponent<hideheadbutton>();
                instance.transform.Find("GoatCamModObjects/Menu/Idk Yet Button (7)").gameObject.AddComponent<disconnectbutton>();
                instance.transform.Find("GoatCamModObjects/Menu/Idk Yet Button (8)").gameObject.AddComponent<quitgamemenubutton>();
                instance.transform.Find("GoatCamModObjects/Menu/Idk Yet Button (9)").gameObject.AddComponent<redcolorbutton>();
                instance.transform.Find("GoatCamModObjects/Menu/Idk Yet Button (13)").gameObject.AddComponent<greencolorbutton>();
                instance.transform.Find("GoatCamModObjects/Menu/Idk Yet Button (14)").gameObject.AddComponent<bluecolorbutton>();
                instance.transform.Find("GoatCamModObjects/Menu/Idk Yet Button (16)").gameObject.AddComponent<capturebutton>();
                instance.transform.Find("GoatCamModObjects/Menu/QuitGameMenu/Idk Yet Button (16)").gameObject.AddComponent<closequitmenubutton>();
                instance.transform.Find("GoatCamModObjects/Menu/QuitGameMenu/Idk Yet Button (17)").gameObject.AddComponent<quitgamebutton>();

                // Get GoatCam internal camera
                _goatCam = instance.GetComponentInChildren<Camera>();
                if (_goatCam == null)
                {
                    Debug.LogError("[GoatCam]: Camera not found in prefab.");
                    return;
                }

                // Get Gorilla Tag Shoulder Camera (desktop output)
                var shoulderCamObj = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera");
                if (shoulderCamObj == null)
                {
                    Debug.LogError("[GoatCam]: Shoulder Camera not found.");
                    return;
                }

                _shoulderCam = shoulderCamObj.GetComponent<Camera>();

                // Disable Cinemachine
                var camBrain = shoulderCamObj.GetComponent<CinemachineBrain>();
                if (camBrain != null)
                    camBrain.enabled = false;

                // Disable virtual camera
                var vcam = shoulderCamObj.transform.Find("CM vcam1");
                if (vcam != null)
                    vcam.gameObject.SetActive(false);

                // Parent Shoulder Camera to GoatCam camera
                shoulderCamObj.transform.SetParent(_goatCam.transform);
                shoulderCamObj.transform.localPosition = Vector3.zero;
                shoulderCamObj.transform.localRotation = Quaternion.identity;

                // Find and hide LCKWallCameraSpawner
                var lckWallCameraSpawner = GameObject.Find("LCKWallCameraSpawner");
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
            }
            catch (Exception e)
            {
                Debug.LogError("[GoatCam]: Setup failed - " + e.Message);
            }
        }

        void LateUpdate()
        {
            if (_goatCam == null || _shoulderCam == null)
                return;

            _shoulderCam.fieldOfView = _goatCam.fieldOfView;
            _shoulderCam.nearClipPlane = _goatCam.nearClipPlane;
            _shoulderCam.farClipPlane = _goatCam.farClipPlane;
            _shoulderCam.cullingMask = _goatCam.cullingMask;
            _shoulderCam.clearFlags = _goatCam.clearFlags;
            _shoulderCam.backgroundColor = _goatCam.backgroundColor;
            _shoulderCam.allowHDR = _goatCam.allowHDR;
            _shoulderCam.allowMSAA = _goatCam.allowMSAA;
            _shoulderCam.orthographic = _goatCam.orthographic;
            _shoulderCam.orthographicSize = _goatCam.orthographicSize;
        }
    }

    public class Constants
    {
        public const string GUID = "goat.goatcammod";
        public const string NAME = "GoatCamMod";
        public const string VERS = "1.0.0";
    }
}