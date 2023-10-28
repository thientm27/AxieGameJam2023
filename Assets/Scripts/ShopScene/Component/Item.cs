using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ShopScene.Component
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private GameObject indicator;
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private List<GameObject> levels;

        public void SetIndicator(bool isActive)
        {
            indicator.SetActive(isActive);
        }
        
        public void SetPrice(int vale)
        { 
            price.text = vale.ToString();
        }
        
        public void SetLevel(int current)
        {
            for (int i = 0; i < current; i++)
            {
                levels[i].SetActive(true);
            }
        }
    }
}
