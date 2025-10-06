using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [HideInInspector] public Vector2Int cropGridPosition;
    [SerializeField] private SpriteRenderer cropHarvestSP = null;
    [SerializeField] private Transform harvestActionEffectTransform = null;

    private int harvesActionCount = 0;
    public void ProcessToolAction(ItemDetails equippedItemDetails, bool isToolRight, bool isToolLeft, bool isToolDown, bool isToolUp)
    {
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cropGridPosition.x, cropGridPosition.y);
        if (gridPropertyDetails == null)
        {
            return;
        }

        ItemDetails seedItemDetails = InventoryManager.Instance.GetItemDetailsByID(gridPropertyDetails.seedItemID);
        if (seedItemDetails == null)
        {
            return;
        }
        CropDetails cropDetails = GridPropertiesManager.Instance.GetCropDetails(seedItemDetails.itemID);
        if (cropDetails == null)
        {
            return;
        }

        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            if (isToolRight || isToolUp)
            {
                animator.SetTrigger("usetoolright");
            }
            else if (isToolLeft || isToolDown)
            {
                animator.SetTrigger("usetoolleft");
            }
        }

        if(cropDetails.isHarvestActionEffect)
        {
            EventHandler.CallHarvestActionEffectEvent(harvestActionEffectTransform.position, cropDetails.harvestActionEffect); 
        }

        harvesActionCount += 1;
        int requiredHarvestActions = cropDetails.RequiredHarvestActionsForTool(equippedItemDetails.itemID);
        if (requiredHarvestActions == -1)
        {
            return;
        }

        if (harvesActionCount >= requiredHarvestActions)
        {
            HarvestCrop(cropDetails, gridPropertyDetails, isToolRight, isToolUp, animator);
        }
    }

    private void HarvestCrop(CropDetails cropDetails, GridPropertyDetails gridPropertyDetails, bool isUsingToolRight, bool isUsingToolUp, Animator animator)
    {
        if (cropDetails.isHarvestedAnimation || animator != null)
        {
            if (cropDetails.harvestedSprite != null)
            {
                if (cropHarvestSP != null)
                {
                    cropHarvestSP.sprite = cropDetails.harvestedSprite;
                }
            }
            if (isUsingToolRight || isUsingToolUp)
            {
                animator.SetTrigger("harvestright");
            }
            else
            {
                animator.SetTrigger("harvestleft");
            }
        }

        if(cropDetails.soundName != SoundName.none)
        {
            AudioManager.Instance.PlaySound(cropDetails.soundName);
        }

        gridPropertyDetails.seedItemID = -1;
        gridPropertyDetails.growthDays = -1;
        gridPropertyDetails.daysSinceLastHarvest = -1;
        gridPropertyDetails.daysSinceWatered = -1;

        if (cropDetails.hideCropBeforeHarvestedAnimation)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }

        if(cropDetails.disableCropcollidersBeforeHarvestedAnimation)
        {
            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
        }

        GridPropertiesManager.Instance.SetGridPropertyDetails(cropGridPosition.x, cropGridPosition.y, gridPropertyDetails);

        if (cropDetails.isHarvestedAnimation && animator != null)
        {
            StartCoroutine(ProcessHarvestActionAfterAnimation(cropDetails, gridPropertyDetails, animator));
        }
        else
        {
            HarvestActions(cropDetails, gridPropertyDetails);
        }
    }

    

    private void HarvestActions(CropDetails cropDetails, GridPropertyDetails gridPropertyDetails)
    {
        SpawnHarvestedItems(cropDetails);
        if(cropDetails.harvestTransformItemID > 0)
        {
            CreateHarvesTransformCrop(cropDetails, gridPropertyDetails);
        }
        Destroy(gameObject);
    }

    private void CreateHarvesTransformCrop(CropDetails cropDetails, GridPropertyDetails gridPropertyDetails)
    {
        gridPropertyDetails.seedItemID = cropDetails.harvestTransformItemID;
        gridPropertyDetails.growthDays = 0;
        gridPropertyDetails.daysSinceLastHarvest = -1;
        gridPropertyDetails.daysSinceWatered = -1;

        GridPropertiesManager.Instance.SetGridPropertyDetails(cropGridPosition.x, cropGridPosition.y, gridPropertyDetails);
        GridPropertiesManager.Instance.DisplayPlantedCrop(gridPropertyDetails);
    }

    private void SpawnHarvestedItems(CropDetails cropDetails)
    {
        for (int i = 0; i < cropDetails.cropProducedItemID.Length; i++)
        {
            int cropToProduce;
            if (cropDetails.cropProducedMinQuality[i] == cropDetails.cropProducedMaxQuality[i] ||
                cropDetails.cropProducedMaxQuality[i] < cropDetails.cropProducedMinQuality[i])
            {
                cropToProduce = cropDetails.cropProducedMinQuality[i];
            }
            else
            {
                cropToProduce = UnityEngine.Random.Range(cropDetails.cropProducedMinQuality[i], cropDetails.cropProducedMaxQuality[i]);
            }

            for (int j = 0; j < cropToProduce; j++)
            {
                Vector3 spawnPosition;
                if (cropDetails.spawnCropProducedAtPlayerPosition)
                {
                    InventoryManager.Instance.AddItem(InventoryLocation.Player, cropDetails.cropProducedItemID[i]);
                }
                else
                {
                    spawnPosition = new Vector3(transform.position.x + UnityEngine.Random.Range(-1f, 1f),
                        transform.position.y + UnityEngine.Random.Range(-1f, 1f),
                        0);
                    SceneItemsManager.Instance.InstantiateSceneItems(cropDetails.cropProducedItemID[i], spawnPosition);

                }
            }
        }
    }

    private IEnumerator ProcessHarvestActionAfterAnimation(CropDetails cropDetails, GridPropertyDetails gridPropertyDetails, Animator animator)
    {
        while(!animator.GetCurrentAnimatorStateInfo(0).IsName("Harvested"))
        {
            yield return null;
        }
        HarvestActions(cropDetails, gridPropertyDetails);
    }
}
