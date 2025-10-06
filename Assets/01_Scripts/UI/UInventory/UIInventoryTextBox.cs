using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryTextBox : MonoBehaviour
{
    [SerializeField] private Text textTop1;
    [SerializeField] private Text textTop2;
    [SerializeField] private Text textTop3;
    [SerializeField] private Text textBottom1;
    [SerializeField] private Text textBottom2;
    [SerializeField] private Text textBottom3;

    public void SetTextboxText(string textTop1, string textTop2, string textTop3, string textBottom1, string textBottom2, string textBottom3)
    {
        
        this.textTop1.text = textTop1;
        this.textTop2.text = textTop2;
        this.textTop3.text = textTop3;
        this.textBottom1.text = textBottom1;
        this.textBottom2.text = textBottom2;
        this.textBottom3.text = textBottom3;
        
    }
}
