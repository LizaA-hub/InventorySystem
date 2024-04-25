using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory{

    public class Item : MonoBehaviour
    {
        public string Name;
        public InventoryManager.Type type;
        public Sprite sprite;
        public int Quantity, HealingPower;
        public bool IsStackable, IsUsable, DisappearAfterUse;
        [TextAreaAttribute]
        public string Description;

        private SpriteRenderer Renderer;
        
        InventoryManager inventory;

        // Start is called before the first frame update
        void Start()
        {
            Renderer = GetComponent<SpriteRenderer>();
            if (sprite != null){
                Renderer.sprite = sprite;
            }
        
            inventory = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        }

        void OnTriggerEnter2D(Collider2D other){
            inventory.AddItem(new InventoryManager.ItemData(this));
            Destroy(gameObject);
        }

        public void SetDatas(InventoryManager.ItemData data){
                type = data.type;
                sprite = data.sprite;
                Quantity = data.Quantity;
                HealingPower = data.HealingPower;
                IsStackable = data.IsStackable;
                IsUsable = data.IsUsable;
                Name = data.Name;
                Description = data.Description;
                DisappearAfterUse = data.DisappearAfterUse;
        }
    }

}
