using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inventory{
    public class Slot : MonoBehaviour
    {
        public Image selectedPanel, itemSprite;
        public TMP_Text quantity;

        private int ItemNb = -1;

        InventoryManager inventory;

#region Unity Functions
        
        // Start is called before the first frame update
        void Start()
        {
            inventory = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
#endregion

#region Public Functions
        public void Selected(){
            if(ItemNb>=0){
                inventory.SelectItem(ItemNb);
                selectedPanel.enabled = true;
            }
        }

        public void Deselected(){
            selectedPanel.enabled = false;
        }

        public void ShowItem(Sprite _sprite, int _quantity, int id){
            ItemNb = id;

            if(_sprite != null){
                itemSprite.enabled = true;
                itemSprite.sprite = _sprite;

                if(_quantity>1){
                    ShowQuantity(_quantity);
                }
                else{
                    HideQuantity();
                }
            }
            else{
                itemSprite.enabled = false;
                HideQuantity();
            }
        }

        public void HideItem(){
            itemSprite.enabled = false;
            }

#endregion

#region Private Functions

        private void ShowQuantity(int value){
            quantity.enabled = true;
            quantity.text = value.ToString();
        }

        private void HideQuantity(){
            quantity.enabled = false;
        }

#endregion
    }
}
