using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "so_CropDetailsList", menuName = "ScriptableObject/CropDetailsList")]
public class SO_CropDetailsList : ScriptableObject
{
    [SerializeField]
    public List<CropDetails> cropDetailsList;

    public CropDetails GetCropDetails(int seedID)
    {
        return cropDetailsList.Find(x => x.seedID == seedID);
    }
}
