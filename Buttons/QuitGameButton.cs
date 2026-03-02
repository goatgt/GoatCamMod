using UnityEngine;

namespace GoatCamMod
{
    public class quitgamebutton : GorillaPressableButton
    {
        public override void ButtonActivation()
        {
            Debug.Log("[GoatCamMod] Quit Game Button Pressed");

            QuitGame();
        }

        void Start()
        {
            this.gameObject.layer = 18;

            buttonRenderer = GetComponent<MeshRenderer>();

            Material unpressedMat = new Material(buttonRenderer.material)
            {
                color = Color.white
            };

            Material pressedMat = new Material(buttonRenderer.material)
            {
                color = Color.red
            };

            unpressedMaterial = unpressedMat;
            pressedMaterial = pressedMat;
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            Debug.Log("[GoatCamMod] Stopping Play Mode (Editor)");
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Debug.Log("[GoatCamMod] Quitting Application");
            Application.Quit();
#endif
        }
    }
}
