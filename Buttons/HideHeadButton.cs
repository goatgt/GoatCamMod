using System.Linq;
using GorillaNetworking;
using UnityEngine;

namespace GoatCamMod;

public class HideHeadButton : GorillaPressableButton
{
    private GameObject headCosmetics;

    public void Start()
    {
        gameObject.layer = 18;

        buttonRenderer = GetComponent<MeshRenderer>();

        unpressedMaterial = new Material(buttonRenderer.material)
        {
                color = Color.white,
        };

        pressedMaterial = new Material(buttonRenderer.material)
        {
                color = Color.red,
        };
    }

    public override void ButtonActivation()
    {
        Debug.Log("[Monke Mod] Pressed Button");

        isOn = !isOn;
        UpdateColor();

        VRRig rig = VRRig.LocalRig;

        try
        {
            CosmeticsController.CosmeticItem[] headItems = rig.cosmeticSet.items.Where(item =>
                    item.itemCategory is CosmeticsController.CosmeticCategory.Face
                                         or CosmeticsController.CosmeticCategory.Hat).ToArray();

            foreach (CosmeticsController.CosmeticItem cosmeticItem in headItems)
            {
                CosmeticItemInstance cosmeticObject = rig.cosmeticsObjectRegistry.Cosmetic(cosmeticItem.displayName);

                CosmeticsController.CosmeticSlots slot =
                        cosmeticItem.itemCategory == CosmeticsController.CosmeticCategory.Face
                                ? CosmeticsController.CosmeticSlots.Face
                                : CosmeticsController.CosmeticSlots.Hat;

                if (isOn)
                    cosmeticObject.EnableItem(slot, rig);
                else
                    cosmeticObject.DisableItem(slot);
            }
        }
        catch
        {
            // Ignored
        }
    }
}