using UnityEngine;
using GorillaLocomotion;

namespace GoatCamMod
{
    public class followplayerbutton : GorillaPressableButton
    {
        private Transform goatModelTransform;
        private Transform bodyTransform;

        private bool followPlayer = false;
        private float positionSmooth = 0.12f;
        private float rotationSmooth = 0.18f;

        public override void ButtonActivation()
        {
            followPlayer = !followPlayer;

            isOn = followPlayer;
            UpdateColor();
        }

        private void Start()
        {
            gameObject.layer = 18;

            buttonRenderer = GetComponent<MeshRenderer>();
            Material unpressedMat = new Material(buttonRenderer.material) { color = Color.white };
            Material pressedMat = new Material(buttonRenderer.material) { color = Color.red };
            unpressedMaterial = unpressedMat;
            pressedMaterial = pressedMat;

            GameObject goatModel = GameObject.Find("GoatCameraModModelBetter(Clone)");
            if (goatModel != null)
                goatModelTransform = goatModel.transform;

            if (GTPlayer.Instance != null)
                bodyTransform = GTPlayer.Instance.bodyCollider.transform;
        }

        private void LateUpdate()
        {
            if (!followPlayer || goatModelTransform == null || bodyTransform == null)
                return;

            // Keep X/Z position fixed, only follow Y (vertical)
            Vector3 targetPosition = new Vector3(
                goatModelTransform.position.x,       // stay in same X
                bodyTransform.position.y,            // follow player's Y
                goatModelTransform.position.z        // stay in same Z
            );

            goatModelTransform.position = Vector3.Lerp(
                goatModelTransform.position,
                targetPosition,
                positionSmooth
            );

            // Rotate to look at player
            Vector3 directionToPlayer = bodyTransform.position - goatModelTransform.position;
            directionToPlayer.y = 0f; // keep rotation only horizontal (optional, remove if you want full tilt)

            if (directionToPlayer != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer.normalized);
                goatModelTransform.rotation = Quaternion.Slerp(
                    goatModelTransform.rotation,
                    targetRotation,
                    rotationSmooth
                );
            }
        }
    }
}