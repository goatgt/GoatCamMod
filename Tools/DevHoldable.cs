using System;
using GorillaLocomotion;
using UnityEngine;

namespace GoatCamMod.Tools
{
    public class DevHoldable : HoldableObject
    {
        public bool InHand = false,
            InLeftHand = false,
            PickUp = true,
            DidSwap = false,
            SwappedLeft = true;

        public float Distance = 0.13f,
            ThrowForce = 1.75f;

        public Action OnPickUp;
        public Action OnSwapHands;
        public Action OnPutDown;

        private BoxCollider[] boxes; // Now supports multiple colliders

        private void Awake()
        {
            GameObject obj = GameObject.Find("GoatCameraModModelBetter(Clone)");

            if (obj != null)
            {
                // Get ALL BoxColliders (including children)
                boxes = obj.GetComponentsInChildren<BoxCollider>();

                foreach (var box in boxes)
                {
                    // Set each collider's GameObject to layer 18
                    box.gameObject.layer = 18;
                }
            }

            if (boxes == null || boxes.Length == 0)
            {
                Debug.LogError("[DevHoldable] No BoxColliders found on GoatCameraModModelBetter(Clone)");
            }
        }

        private bool IsHandInsideAnyBox(Vector3 handPos)
        {
            if (boxes == null) return false;

            foreach (var box in boxes)
            {
                if (box != null && box.bounds.Contains(handPos))
                    return true;
            }

            return false;
        }

        public virtual void OnGrab(bool isLeft)
        {
            OnPickUp?.Invoke();
        }

        public virtual void OnDrop(bool isLeft)
        {
            if (isLeft)
            {
                InLeftHand = true;
                InHand = false;
                transform.SetParent(null);
                EquipmentInteractor.instance.leftHandHeldEquipment = null;
            }
            else
            {
                InLeftHand = false;
                InHand = false;
                transform.SetParent(null);
                EquipmentInteractor.instance.rightHandHeldEquipment = null;
            }

            OnPutDown?.Invoke();
        }

        public void Update()
        {
            if (boxes == null || boxes.Length == 0)
                return;

            var leftGrip = ControllerInputPoller.instance.leftControllerGripFloat > 0.6f;
            var rightGrip = ControllerInputPoller.instance.rightControllerGripFloat > 0.6f;

            DidSwap = (!DidSwap || (!SwappedLeft ? leftGrip : rightGrip)) && DidSwap;

            Vector3 leftHandPos = GTPlayer.Instance.LeftHand.controllerTransform.position;
            Vector3 rightHandPos = GTPlayer.Instance.RightHand.controllerTransform.position;

            bool leftInside = IsHandInsideAnyBox(leftHandPos);
            bool rightInside = IsHandInsideAnyBox(rightHandPos);

            var pickLeft =
                PickUp &&
                leftGrip &&
                leftInside &&
                !InHand &&
                EquipmentInteractor.instance.leftHandHeldEquipment == null &&
                !DidSwap;

            var swapLeft =
                InHand &&
                leftGrip &&
                rightGrip &&
                !DidSwap &&
                leftInside &&
                !SwappedLeft &&
                EquipmentInteractor.instance.leftHandHeldEquipment == null;

            if (pickLeft || swapLeft)
            {
                DidSwap = swapLeft;
                SwappedLeft = true;
                InLeftHand = true;
                InHand = true;

                transform.SetParent(GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent);
                GorillaTagger.Instance.StartVibration(true, 0.07f, 0.07f);
                EquipmentInteractor.instance.leftHandHeldEquipment = this;

                if (DidSwap)
                    EquipmentInteractor.instance.rightHandHeldEquipment = null;

                OnGrab(true);
            }
            else if ((!leftGrip && InHand && InLeftHand) || (!PickUp && InHand))
            {
                OnDrop(true);
                return;
            }

            bool pickRight =
                PickUp &&
                rightGrip &&
                rightInside &&
                !InHand &&
                EquipmentInteractor.instance.rightHandHeldEquipment == null &&
                !DidSwap;

            bool swapRight =
                InHand &&
                leftGrip &&
                rightGrip &&
                !DidSwap &&
                rightInside &&
                SwappedLeft &&
                EquipmentInteractor.instance.rightHandHeldEquipment == null;

            if (pickRight || swapRight)
            {
                DidSwap = swapRight;
                SwappedLeft = false;
                InLeftHand = false;
                InHand = true;

                transform.SetParent(GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent);
                GorillaTagger.Instance.StartVibration(false, 0.07f, 0.07f);
                EquipmentInteractor.instance.rightHandHeldEquipment = this;

                if (DidSwap)
                    EquipmentInteractor.instance.leftHandHeldEquipment = null;

                OnGrab(false);
            }
            else if ((!rightGrip && InHand && !InLeftHand) || (!PickUp && InHand))
            {
                OnDrop(false);
                return;
            }
        }

        public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
        {
            throw new NotImplementedException();
        }

        public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
        {
            throw new NotImplementedException();
        }

        public override void DropItemCleanup()
        {
            throw new NotImplementedException();
        }
    }
}