using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory{

    public class InventoryManager : MonoBehaviour
    {
        [System.Serializable]
        public class ItemData
        {
            public Type type;
            public Sprite sprite;
            public int Quantity, HealingPower;
            public bool IsStackable, IsUsable, DisappearAfterUse;
            public string Name, Description;            
            public ItemData(Item item){
                type = item.type;
                sprite = item.sprite;
                Quantity = item.Quantity;
                HealingPower = item.HealingPower;
                IsStackable = item.IsStackable;
                IsUsable = item.IsUsable;
                Name = item.Name;
                Description = item.Description;
                DisappearAfterUse = item.DisappearAfterUse;
            }
        }

        public enum Type{
            Consummable,
            Furniture,
            Tool
        }

        public List<ItemData> Items = new List<ItemData>();//the list of items currently in the inventory

        public Slot[] Slots; //Displayed items
        public GameObject[] Arrows;
        private int fisrtDisplayedItem = 0, SelectedItem = 0;
        public GameObject Options, ItemPrefab, _descriptionWindow;
        public LogManager logManager;
        public DescriptionWindow descriptionWindow;


#region Unity Funcitons
        // Start is called before the first frame update
        void Start()
        {
            SetItems();
        }

        private void Update() {
            if(Input.GetAxis("Horizontal")!=0f || Input.GetAxis("Vertical")!=0f){
                DeselectAllSlot();
            }
        }


#endregion

#region Public Functions

        public void AddItem(ItemData data){
            if(data.IsStackable){
                foreach(ItemData item in Items){
                    if(item.Name == data.Name){
                        item.Quantity +=1;
                        SetItems();
                        return;
                    }
                }
                Items.Add(data);
            }
            else{
                Items.Add(data);
            }
            SetItems();
        }

        public void MoveLeft(){
            fisrtDisplayedItem --;
            DeselectAllSlot();
            SetItems();
        }

        public void MoveRight(){
            fisrtDisplayedItem ++;
            DeselectAllSlot();
            SetItems();
        }

        public void SelectItem(int pos){
            SelectedItem = pos;
            DeselectAllSlot();

            //show options panel
            Options.SetActive(true);

            //put the panel on top of the selected slot
            Vector2 position;
            
            int slotPos = SelectedItem - fisrtDisplayedItem;
            if(slotPos >= 0){
                position = Slots[slotPos].transform.position;
            }
            else{
                position = Slots[Items.Count + slotPos].transform.position;
            }
            position.y += 1.5f;
            Options.transform.position = position;
        }
        
        public void Use(){
            string message;
            if(Items[SelectedItem].IsUsable){
                switch (Items[SelectedItem].type)
                {
                    case Type.Consummable:

                        message = "You eat " + Items[SelectedItem].Name + " . " + "+ " + Items[SelectedItem].HealingPower.ToString()  + " health."; 

                        if(Items[SelectedItem].Quantity > 1){
                            Items[SelectedItem].Quantity --;
                        }
                        else{
                            Items.Remove(Items[SelectedItem]);
                        }
                        SetItems();
                        
                    break;

                    case Type.Furniture:
                    
                        message = "You use " + Items[SelectedItem].Name + " . " + "Whatever this means."; 
                        
                        if(Items[SelectedItem].Quantity > 1){
                            Items[SelectedItem].Quantity --;
                        }
                        else{
                            if(Items[SelectedItem].DisappearAfterUse ){
                                Items.Remove(Items[SelectedItem]);
                            }
                        }
                        SetItems();
                        
                    break;

                    case Type.Tool:
                        message = "You use " + Items[SelectedItem].Name + " . " ; 
                        
                        if(Items[SelectedItem].Quantity > 1){
                            Items[SelectedItem].Quantity --;
                        }
                        else{
                            if(Items[SelectedItem].DisappearAfterUse ){
                                Items.Remove(Items[SelectedItem]);
                            }
                        }
                        SetItems();
                    break;

                    default:
                        message = "You can't use this!";
                    break;
                }
                logManager.LogDisplay(message);
            }
            else{
                logManager.LogDisplay("You can't use this!");
            }

            DeselectAllSlot();
        }

        public void Drop(){
            Vector2 PlayerPos = GameObject.Find("Player").transform.position;
            Vector2 newitemPosition;
            switch (Movement.PlayerMovement.lastDirection)
            {
                case Movement.Direction.LEFT:
                    newitemPosition = new Vector2(PlayerPos.x-1.5f, PlayerPos.y);
                break;

                case Movement.Direction.RIGHT:
                    newitemPosition = new Vector2(PlayerPos.x+1.5f, PlayerPos.y);
                break;

                case Movement.Direction.UP:
                    newitemPosition = new Vector2(PlayerPos.x, PlayerPos.y+1.5f);
                break;

                case Movement.Direction.DOWN:
                    newitemPosition = new Vector2(PlayerPos.x, PlayerPos.y-1.5f);
                break;

                default:
                    newitemPosition = new Vector2(PlayerPos.x, PlayerPos.y-1.5f);
                break;
            }

            GameObject newItem = Instantiate(ItemPrefab, newitemPosition, Quaternion.identity);
            Item newItemManager = newItem.GetComponent<Item>();
            newItemManager.SetDatas(Items[SelectedItem]);

            logManager.LogDisplay("You drop " + Items[SelectedItem].Name);

            if(Items[SelectedItem].Quantity > 1){
                Items[SelectedItem].Quantity --;
            }
            else{
                Items.Remove(Items[SelectedItem]);
            }
            SetItems();

            DeselectAllSlot();
        }

        public void Details(){

            DeselectAllSlot();

            _descriptionWindow.SetActive(true);
            descriptionWindow.DisplayDetails(Items[SelectedItem]);
        }

#endregion

#region Private Functions

        private void SetItems(){

            if(fisrtDisplayedItem <0){
                fisrtDisplayedItem = Items.Count-1;
            }
            else if(fisrtDisplayedItem >= Items.Count){
                fisrtDisplayedItem = 0;
            }

            int ItemNb = Items.Count;
            int pos = 0;
            if(ItemNb != 0){
                if(ItemNb>=5){
                    for(int i = 0; i < 5; i++){
                        if((fisrtDisplayedItem + i) < Items.Count){
                            pos =  fisrtDisplayedItem +i;
                            Slots[i].ShowItem(Items[pos].sprite , Items[pos].Quantity, pos);
                        }
                        else{
                            pos = (fisrtDisplayedItem +i)-Items.Count;
                            Slots[i].ShowItem(Items[pos].sprite , Items[pos].Quantity, pos);
                        }  
                    }
                    DisplayArrow(true);
                }
                else{
                    for(int i = 0; i < ItemNb; i++){
                        Slots[i].ShowItem(Items[i].sprite , Items[i].Quantity, i);
                    }
                    for(int i = ItemNb; i < 5; i++){
                        Slots[i].ShowItem(null , 0, -1);
                    }

                    DisplayArrow(false);
                }
                
            }
            else{
                DisplayArrow(false);
            }
        }

        private void DisplayArrow(bool value){
            foreach(GameObject arrow in Arrows){
                arrow.SetActive(value);
            }
        }

        private void DeselectAllSlot(){
            foreach(Slot slot in Slots){
                slot.Deselected();
            }
            Options.SetActive(false);

            if(_descriptionWindow.activeSelf)
                _descriptionWindow.SetActive(false);

        }
#endregion
    }

}
