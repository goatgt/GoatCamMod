using UnityEngine;

namespace GoatCamMod
{
    public class closequitmenubutton : GorillaPressableButton
    {
        private GameObject quitGameMenu;

        public override void ButtonActivation()
        {
            Debug.Log("[GoatCamMod] Close Quit Menu Button Pressed");

            if (quitGameMenu != null)
            {
                quitGameMenu.SetActive(false);
            }
            else
            {
                Debug.LogError("[GoatCamMod] QuitGameMenu is NULL!");
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

            // 🔥 Find QuitGameMenu (even if disabled)
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "QuitGameMenu")
                {
                    quitGameMenu = obj;
                    break;
                }
            }

            if (quitGameMenu == null)
            {
                Debug.LogError("[GoatCamMod] Could not find QuitGameMenu in scene!");
            }
        }
    }
}
