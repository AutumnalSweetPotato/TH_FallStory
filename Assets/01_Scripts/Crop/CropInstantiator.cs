using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropInstantiator : MonoBehaviour
{
    private Grid grid;
    [SerializeField] private int daysSincDug = -1;
    [SerializeField] private int daysSincWatered = -1;
    [ItemDescription]
    [SerializeField] private int seedItemID = 0;
    [SerializeField] private int growDays = 0;

    private void OnEnable()
    {
        EventHandler.InstantiateCropPrefabsEvent += InstantiateCropPrefabs;
    }
    private void OnDisable()
    {
        EventHandler.InstantiateCropPrefabsEvent -= InstantiateCropPrefabs;
    }
    private void InstantiateCropPrefabs()
    {
        grid = FindObjectOfType<Grid>();
        Vector3Int cropGridPosition = grid.WorldToCell(transform.position);
        SetCropGridProperties(cropGridPosition);
        Destroy(gameObject);
    }

    private void SetCropGridProperties(Vector3Int cropGridPosition)
    {
        if(seedItemID > 0)
        {
            GridPropertyDetails gridPropertyDetails;
            gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cropGridPosition);
            if(gridPropertyDetails == null)
            {
                gridPropertyDetails = new GridPropertyDetails();
            }
            gridPropertyDetails.daysSinceDug = daysSincDug;
            gridPropertyDetails.daysSinceWatered = daysSincWatered;
            gridPropertyDetails.seedItemID = seedItemID;
            gridPropertyDetails.growthDays = growDays;

            GridPropertiesManager.Instance.SetGridPropertyDetails(cropGridPosition.x, cropGridPosition.y, gridPropertyDetails);
        }
    }
}
