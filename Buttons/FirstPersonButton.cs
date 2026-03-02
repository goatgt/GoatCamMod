using UnityEngine;
using GorillaLocomotion;

namespace GoatCamMod
{
    public class firstpersonbutton : GorillaPressableButton
    {
        private Camera cam;
        private bool isFirstPerson = false;

        private GameObject gorillaFace;
        private GameObject goatCamModObject;

        public enum SmoothingMode
        {
            Low,
            Medium,
            High
        }

        public static SmoothingMode currentMode = SmoothingMode.Low;
        private static TextMesh smoothingText;

        private Transform originalParent;
        private Vector3 originalLocalPosition;
        private Quaternion originalLocalRotation;

        private Transform head;
        private Quaternion targetRotation;

        public System.Action OnFirstPersonPressed;

        private MeshRenderer buttonRenderer;
        private Material unpressedMaterial;
        private Material pressedMaterial;

        public void Start()
        {
            gameObject.layer = 18;

            gorillaFace = GameObject.Find("gorillaface");
            goatCamModObject = GameObject.Find("GoatCameraModModelBetter(Clone)");

            GameObject textObj = GameObject.Find("Sample Textmesh (29)");
            if (textObj != null)
            {
                smoothingText = textObj.GetComponent<TextMesh>();
                UpdateSmoothingText();
            }

            buttonRenderer = GetComponent<MeshRenderer>();
            unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white };
            pressedMaterial = new Material(buttonRenderer.material) { color = Color.red };
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

                originalParent = cam.transform.parent;
                originalLocalPosition = cam.transform.localPosition;
                originalLocalRotation = cam.transform.localRotation;
            }

            head = GTPlayer.Instance.headCollider.transform;
            if (head == null)
            {
                Debug.LogError("[Monke Mod] Could not find the local player's head");
                return;
            }

            if (thirdpersonbutton.ThirdPersonActive && !isFirstPerson)
            {
                Debug.Log("[Monke Mod] Cannot enter first-person while third-person is active");
                return;
            }

            if (!isFirstPerson)
            {
                cam.transform.SetParent(null);
                cam.transform.position = head.position;

                if (gorillaFace != null)
                    gorillaFace.SetActive(false);

                isFirstPerson = true;
                OnFirstPersonPressed?.Invoke();
            }
            else
            {
                DisableFirstPerson();
            }
        }

        void LateUpdate()
        {
            if (!isFirstPerson || cam == null || head == null)
                return;

            cam.transform.position = head.position;
            targetRotation = head.rotation;

            float smoothing = GetSmoothingValue();
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, targetRotation, smoothing * Time.deltaTime);
        }

        public void DisableFirstPerson()
        {
            if (!isFirstPerson || cam == null) return;

            cam.transform.SetParent(originalParent);
            cam.transform.localPosition = originalLocalPosition;
            cam.transform.localRotation = originalLocalRotation;

            if (gorillaFace != null)
                gorillaFace.SetActive(true);

            isFirstPerson = false;
            UpdateColor();
        }

        public static void CycleSmoothingModeForward()
        {
            switch (currentMode)
            {
                case SmoothingMode.Low: currentMode = SmoothingMode.Medium; break;
                case SmoothingMode.Medium: currentMode = SmoothingMode.High; break;
                case SmoothingMode.High: currentMode = SmoothingMode.Low; break;
            }
            UpdateSmoothingText();
        }

        public static void CycleSmoothingModeBackward()
        {
            switch (currentMode)
            {
                case SmoothingMode.Low: currentMode = SmoothingMode.High; break;
                case SmoothingMode.Medium: currentMode = SmoothingMode.Low; break;
                case SmoothingMode.High: currentMode = SmoothingMode.Medium; break;
            }
            UpdateSmoothingText();
        }

        private static void UpdateSmoothingText()
        {
            if (smoothingText != null)
                smoothingText.text = currentMode.ToString().ToUpper();
        }

        private void UpdateColor()
        {
            if (buttonRenderer == null) return;
            buttonRenderer.material = isOn ? pressedMaterial : unpressedMaterial;
        }

        private static float GetSmoothingValue()
        {
            switch (currentMode)
            {
                case SmoothingMode.Low: return 1000f;
                case SmoothingMode.Medium: return 10f;
                case SmoothingMode.High: return 5f;
                default: return 10f;
            }
        }
    }
}