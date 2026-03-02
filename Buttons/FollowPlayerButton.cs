using GorillaLocomotion;
using UnityEngine;

namespace GoatCamMod;

public class FollowPlayerButton : GorillaPressableButton
{
    private const float MaxLeashDistance = 3.8f;
    private const float MinDistance      = 1.5f;
    private const float PositionSmooth   = 0.55f;
    private const float RotationSmooth   = 0.24f;
    private const float RotBoostMax      = 2.4f;

    private Transform bodyTransform;

    private bool      followPlayer;
    private Transform goatModelTransform;
    private Vector3   positionVelocity;

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

        Vector3 toPlayer = bodyTransform.position - goatModelTransform.position;
        float   distance = toPlayer.magnitude;

        float t          = Mathf.Clamp01(distance                    / MaxLeashDistance);
        float smoothTime = Mathf.Lerp(PositionSmooth, PositionSmooth * 0.2f, t * t);

        Vector3 targetPos;
        if (distance > MaxLeashDistance)
            targetPos = bodyTransform.position - toPlayer.normalized * MaxLeashDistance;
        else if (distance < MinDistance)
            targetPos = goatModelTransform.position;
        else
            targetPos = bodyTransform.position;

        goatModelTransform.position = Vector3.SmoothDamp(
                goatModelTransform.position,
                targetPos,
                ref positionVelocity,
                smoothTime
        );

        Vector3 lookDir = bodyTransform.position - goatModelTransform.position;

        if (lookDir == Vector3.zero)
            return;

        float      rotBoost        = Mathf.Lerp(1f, RotBoostMax, Mathf.Clamp01(distance / MaxLeashDistance));
        float      rotSmoothFactor = Mathf.Clamp01(RotationSmooth * rotBoost * (Time.deltaTime / 0.02f));
        Quaternion targetRotation  = Quaternion.LookRotation(lookDir.normalized);

        goatModelTransform.rotation = Quaternion.Slerp(
                goatModelTransform.rotation,
                targetRotation,
                rotSmoothFactor
        );
    }

    public override void ButtonActivation()
    {
        followPlayer = !followPlayer;
        isOn         = followPlayer;

        if (!followPlayer)
            positionVelocity = Vector3.zero;

        UpdateColor();
    }
}