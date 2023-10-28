using System.Collections.Generic;
using UnityEngine;

namespace ShopScene
{
    public class ShopModel : MonoBehaviour
    {
        public List<int> armorPrices;
        public List<int> weaponPrices;
        public List<int> bootsPrices;
        public List<int> rocketPrices;
        
        
        public List<int> accessory0;
        public List<int> accessory1;
        public List<int> accessory2;
        public List<int> accessory3;

        public int GetPriceAmory(int typeIndex, int currentLevel)
        {
            switch (typeIndex)
            {
                case 0:
                {
                    return armorPrices[currentLevel];
                }
                case 1:
                {
                    return weaponPrices[currentLevel];
                }
                case 2:
                {
                    return bootsPrices[currentLevel];
                }
                case 3:
                {
                    return rocketPrices[currentLevel];
                }
            }

            return -1;
        }
        public int GetPriceAccessory(int typeIndex, int currentLevel)
        {
            switch (typeIndex)
            {
                case 0:
                {
                    return accessory0[currentLevel];
                }
                case 1:
                {
                    return accessory1[currentLevel];
                }
                case 2:
                {
                    return accessory2[currentLevel];
                }
                case 3:
                {
                    return accessory3[currentLevel];
                }
            }

            return -1;
        }
    }
}