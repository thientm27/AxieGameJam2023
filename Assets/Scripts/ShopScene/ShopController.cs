using DG.DemiEditor;
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
                return;
            }


            // VALUES
            _index = 0;
            _currentChoose = DisplayShop.Armor;

            // INIT SHOP 1 VALUE
            int[] tmp = new int[4];
            tmp[0] = model.armorPrices[_playerService.ArmoryLevel[0]];
            tmp[1] = model.weaponPrices[_playerService.ArmoryLevel[1]];
            tmp[2] = model.bootsPrices[_playerService.ArmoryLevel[2]];
            tmp[3] = model.rocketPrices[_playerService.ArmoryLevel[3]];
            view.contentShops[0].Initialized(_playerService.ArmoryLevel.ToArray(),
                tmp);

            // INIT SHOP 2 VALUE
            tmp = new int[4];
            tmp[0] = model.accessory0[_playerService.AccessoryLevel[0]];
            tmp[1] = model.accessory1[_playerService.AccessoryLevel[1]];
            tmp[2] = model.accessory2[_playerService.AccessoryLevel[2]];
            tmp[3] = model.accessory3[_playerService.AccessoryLevel[3]];
            view.contentShops[1].Initialized(_playerService.AccessoryLevel.ToArray(),
                tmp);

            // Display first shop
            view.ChangeTab(_currentChoose);
            view.SetUserWallet(_playerService.UserCoin);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                HandleSelector(true);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                HandleSelector(false);
            }


            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SwitchTab(false);
            }
        }

        private void SwitchTab(bool isRight)
        {
            _index = 0;
            view.contentShops[0].SetIndicator(_index);
            view.contentShops[1].SetIndicator(_index);
            switch (_currentChoose)
            {
                case DisplayShop.Armor:
                    _currentChoose = DisplayShop.Accessories;
                    break;
                case DisplayShop.Accessories:
                    _currentChoose = DisplayShop.Armor;
                    break;
            }

            view.ChangeTab(_currentChoose);
        }

        private void HandleSelector(bool isDown)
        {
            _index = isDown ? _index == 3 ? 0 : _index + 1 : _index == 0 ? 3 : _index - 1;
            switch (_currentChoose)
            {
                case DisplayShop.Armor:
                    view.contentShops[0].SetIndicator(_index);
                    break;
                case DisplayShop.Accessories:
                    view.contentShops[1].SetIndicator(_index);
                    break;
            }
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