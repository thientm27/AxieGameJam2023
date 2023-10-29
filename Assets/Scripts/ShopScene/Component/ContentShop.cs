using System.Collections.Generic;
using UnityEngine;

namespace ShopScene.Component
{
    public class ContentShop : MonoBehaviour
    {
        [SerializeField] private List<Item> items;

        public void Initialized(int[] levels, int[] prices)
        {
            SetIndicator(0);
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetPrice(prices[i]);
                items[i].SetLevel(levels[i]);
            }
        }
        
        public void SetIndicator(int index)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetIndicator(i == index);
            }
        }

        public void UpdateNewValue(int index, int level, int newPrice)
        {
            items[index].SetPrice(newPrice);
            items[index].SetLevel(level);
        }
    }

}

