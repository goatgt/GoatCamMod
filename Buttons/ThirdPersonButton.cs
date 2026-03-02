using UnityEngine;
using GorillaLocomotion;

namespace GoatCamMod
{
    public class thirdpersonbutton : GorillaPressableButton
    {
        private Camera cam;
        private bool isThirdPerson = false;

        private Transform originalParent;
        private Vector3 originalLocalPosition;
        private Quaternion originalLocalRotation;

        public float smoothingSpeed = 10f;
        public Vector3 thirdPersonOffset = new Vector3(0f, 0.2f, -1.5f);

        public static bool ThirdPersonActive = false;

        private GameObject goatCamModObject;
        private firstpersonbutton fpButton;

        private MeshRenderer buttonRenderer;
        private Material unpressedMaterial;
        private Material pressedMaterial;

        public void Start()
        {
            gameObject.layer = 18;

            buttonRenderer = GetComponent<MeshRenderer>();
            unpressedMaterial = new Material(buttonRenderer.material) { color = Color.white };
            pressedMaterial = new Material(buttonRenderer.material) { color = Color.red };

            goatCamModObject = GameObject.Find("GoatCameraModModelBetter(Clone)");
            fpButton = FindObjectOfType<firstpersonbutton>();
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

                originalParent = cam.transform.parent;
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
                // Disable first-person if active
                if (fpButton != null)
                    fpButton.DisableFirstPerson();

                // Switch to third-person
                cam.transform.SetParent(head);
                cam.transform.localPosition = thirdPersonOffset;
                cam.transform.localRotation = Quaternion.identity;

                // Move GoatCamModObject to 0,0,0 like first-person button does
                if (goatCamModObject != null)
                    goatCamModObject.transform.position = Vector3.zero;

                isThirdPerson = true;
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

            isThirdPerson = false;
            ThirdPersonActive = false;
            UpdateColor();
            Debug.Log("[Monke Mod] Third-person disabled");
        }

        private void UpdateColor()
        {
            if (buttonRenderer == null) return;
            buttonRenderer.material = isOn ? pressedMaterial : unpressedMaterial;
        }
    }
}