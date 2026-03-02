using GorillaLocomotion;
using UnityEngine;

namespace GoatCamMod;

public class FollowPlayerButton : GorillaPressableButton
{
    private const float     PositionSmooth = 0.12f;
    private const float     RotationSmooth = 0.18f;
    private       Transform bodyTransform;

    private bool      followPlayer;
    private Transform goatModelTransform;

    private void Start()
    {
        gameObject.layer = 18;

        buttonRenderer = GetComponent<MeshRenderer>();
        Material unpressedMat = new(buttonRenderer.material) { color = Color.white, };
        Material pressedMat   = new(buttonRenderer.material) { color = Color.red, };
        unpressedMaterial = unpressedMat;
        pressedMaterial   = pressedMat;

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

        //Sorry if this isn't what you intended, but I've had people tell me they wanted it to follow their position and not just make the camera look at it
        Vector3 targetPosition = new(
                bodyTransform.position.x,
                bodyTransform.position.y,
                bodyTransform.position.z
        );

        goatModelTransform.position = Vector3.Lerp(
                goatModelTransform.position,
                targetPosition,
                PositionSmooth
        );

        Vector3 directionToPlayer = bodyTransform.position - goatModelTransform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer.normalized);
        goatModelTransform.rotation = Quaternion.Slerp(
                goatModelTransform.rotation,
                targetRotation,
                RotationSmooth
        );
    }

    public override void ButtonActivation()
    {
        followPlayer = !followPlayer;

        isOn = followPlayer;
        UpdateColor();
    }
}