using UnityEngine;
using GorillaLocomotion;
using System.Collections.Generic;
using Valve.VR;

namespace GoatCamMod
{
    public class RightPrimaryPopUp : MonoBehaviour
    {
        private bool CachedPrimaryDown;
        private bool CurrentPrimaryDown;

        private float spawnDistance = 1.5f;
        private float rightOffset = 0.3f;

        private float firstClickTime = 0f;
        private int clickCount = 0;
        private float doubleClickWindow = 2f; // 2 second window

        public Dictionary<Transform, Vector3> storedPositions;

        private GameObject goatCamModObject;
        private firstpersonbutton fpButton;
        private thirdpersonbutton tpButton;

        public bool PrimaryTriggered
        {
            get
            {
                bool result = CachedPrimaryDown != CurrentPrimaryDown && CurrentPrimaryDown;
                CachedPrimaryDown = CurrentPrimaryDown;
                return result;
            }
        }

        void Start()
        {
            goatCamModObject = GameObject.Find("GoatCameraModModelBetter(Clone)");
            fpButton = FindObjectOfType<firstpersonbutton>();
            tpButton = FindObjectOfType<thirdpersonbutton>();
        }

        void Update()
        {
            // Right Primary Button instead of joystick
            CurrentPrimaryDown = SteamVR_Actions.gorillaTag_RightPrimaryClick
                .GetState(SteamVR_Input_Sources.RightHand);

            if (PrimaryTriggered)
            {
                if (clickCount == 0)
                {
                    clickCount = 1;
                    firstClickTime = Time.time;
                }
                else if (clickCount == 1 && Time.time - firstClickTime <= doubleClickWindow)
                {
                    clickCount = 0;

                    // DOUBLE CLICK SUCCESS
                    MoveGoatCamModObjectInFrontOfPlayer();
                    ResetCameraMode();

                    if (fpButton != null && GTPlayer.Instance != null)
                        fpButton.DisableFirstPerson();

                    if (tpButton != null && thirdpersonbutton.ThirdPersonActive)
                        tpButton.DisableThirdPerson();
                }
            }

            // Reset if too slow
            if (clickCount == 1 && Time.time - firstClickTime > doubleClickWindow)
            {
                clickCount = 0;
            }
        }

        private void MoveGoatCamModObjectInFrontOfPlayer()
        {
            if (goatCamModObject == null || Camera.main == null)
                return;

            Transform head = Camera.main.transform;

            Vector3 spawnPos =
                head.position +
                head.forward * spawnDistance +
                head.right * rightOffset;

            Quaternion spawnRot =
                Quaternion.Euler(0f, head.eulerAngles.y + 180f, 0f);

            goatCamModObject.transform.position = spawnPos;
            goatCamModObject.transform.rotation = spawnRot;
        }

        private void ResetCameraMode()
        {
            Camera cam = transform.root.GetComponentInChildren<Camera>(true);

            if (cam != null)
            {
                Transform prefabRoot = cam.transform.root;
                prefabRoot.rotation = Quaternion.identity;
            }
        }

        public void RestorePositions()
        {
            if (storedPositions == null)
                return;

            foreach (var pair in storedPositions)
            {
                if (pair.Key != null)
                    pair.Key.position = pair.Value;
            }
        }
    }
}