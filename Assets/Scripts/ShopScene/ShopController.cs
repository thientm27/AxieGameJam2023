using UnityEngine;

namespace ShopScene
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private ShopView view;
        
        private DisplayShop _currentChoose;
        private int _index;
        
        private void Awake()
        {
            _index = 0;
            _currentChoose = DisplayShop.Armor;
            foreach (var item in   view.contentShops)
            {
                
                // Init shop here
            }
            
        }

        private void Start()
        {
            
        }
    }


    public enum DisplayShop
    {
        Armor = 0,
        Accessories = 1,
    }
}
