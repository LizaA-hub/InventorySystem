using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inventory{
public class DescriptionWindow : MonoBehaviour
{
    public TMP_Text Name,Description;
    public Image ItemImage;

    public void DisplayDetails(InventoryManager.ItemData data){
        Name.text = data.Name;
        Description.text = data.Description;
        ItemImage.sprite = data.sprite;
    }
}
}