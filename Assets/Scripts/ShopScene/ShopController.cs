using System;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShopScene
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private ShopView view;
        [SerializeField] private ShopModel model;

        
        private DisplayShop _currentChoose;
        private int _index;
        private PlayerService _playerService;
        

        private void Awake()
        {
            // SERVICES
            if (GameObject.FindGameObjectWithTag(Constants.ServicesTag) != null)
            {
                var services = GameObject.FindGameObjectWithTag(Constants.ServicesTag).GetComponent<GameServices>();
                _playerService = services.GetService<PlayerService>();
            }
            else
            {
                SceneManager.LoadScene(Constants.EntryScene);
            }
            
            
            // VALUES
            _index = 0;
            _currentChoose = DisplayShop.Armor;
            
            // INIT SHOP 1 VALUE
            int[] tmp =  new int[4];
            tmp[0] = model.armorPrices[_playerService.ArmoryLevel[0]];
            tmp[1] = model.weaponPrices[_playerService.ArmoryLevel[1]];
            tmp[2] = model.bootsPrices[_playerService.ArmoryLevel[2]];
            tmp[3] = model.rocketPrices[_playerService.ArmoryLevel[3]];
            view.contentShops[0].Initialized(_playerService.ArmoryLevel.ToArray(),
                tmp);
            
            // INIT SHOP 2 VALUE
            tmp =  new int[4];
            tmp[0] = model.accessory0[_playerService.AccessoryLevel[0]];
            tmp[1] = model.accessory1[_playerService.AccessoryLevel[1]];
            tmp[2] = model.accessory2[_playerService.AccessoryLevel[2]];
            tmp[3] = model.accessory3[_playerService.AccessoryLevel[3]];
            view.contentShops[1].Initialized(_playerService.AccessoryLevel.ToArray(),
                tmp);
            
            // Display first shop
            for (int i = 0; i < view.contentShops.Count; i++)
            {
                view.contentShops[i].gameObject.SetActive(i == 0);
            }
            
            
        }

        private void Update()
        {
            
        }

        private void HandleBuy()
        {
            
        }
    }


    public enum DisplayShop
    {
        Armor = 0,
        Accessories = 1,
    }
}
