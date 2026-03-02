using UnityEngine;

namespace GoatCamMod
{
    public class quitgamemenubutton : GorillaPressableButton
    {
        private GameObject quitGameMenu;

        public override void ButtonActivation()
        {
            Debug.Log("[GoatCamMod] Quit Menu Button Pressed");

            isOn = !isOn;
            UpdateColor();

            if (quitGameMenu != null)
            {
                quitGameMenu.SetActive(isOn);
            }
            else
            {
                Debug.LogError("[GoatCamMod] QuitGameMenu is STILL NULL!");
            }
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

            // 🔥 Find QuitGameMenu EVEN IF DISABLED
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "QuitGameMenu")
                {
                    quitGameMenu = obj;
                    break;
                }
            }

            if (quitGameMenu != null)
            {
                quitGameMenu.SetActive(false); // hidden by default
                Debug.Log("[GoatCamMod] QuitGameMenu found successfully.");
            }
            else
            {
                Debug.LogError("[GoatCamMod] Could not find QuitGameMenu anywhere in scene!");
            }
        }
    }
}
