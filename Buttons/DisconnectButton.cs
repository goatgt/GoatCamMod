using UnityEngine;
using Photon.Pun;

namespace GoatCamMod
{
    public class disconnectbutton : GorillaPressableButton
    {
        public override void ButtonActivation()
        {
            Debug.Log("[GoatCamMod] Disconnect Button Pressed");

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
        }

        void Start()
        {
            this.gameObject.layer = 18;

            buttonRenderer = GetComponent<MeshRenderer>();

            unpressedMaterial = new Material(buttonRenderer.material)
            {
                color = Color.white
            };

            pressedMaterial = new Material(buttonRenderer.material)
            {
                color = Color.red
            };
        }
    }
}
