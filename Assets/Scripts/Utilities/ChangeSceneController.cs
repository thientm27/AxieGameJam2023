using DG.Tweening;
using Services;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class ChangeSceneController : MonoBehaviour
    {

        [SerializeField] private Transform topPad;
        [SerializeField] private Transform downPad;

        private Vector3 _topStart;
        private Vector3 _downStart;
        private AudioService _audioService;

        public void Awake()
        {
            // SERVICES
            if (GameObject.FindGameObjectWithTag(Constants.ServicesTag) != null)
            {
                var services = GameObject.FindGameObjectWithTag(Constants.ServicesTag).GetComponent<GameServices>();
                _audioService = services.GetService<AudioService>();
            }
            
            _topStart = topPad.position;
            _downStart = downPad.position;
        }

        public void Open() 
        {
            _audioService.DoorOpen();
            topPad.DOMove(_topStart + new Vector3(0, 600,0), 0.25f);
            downPad.DOMove(_downStart + new Vector3(0, -900,0), 0.25f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        public void Close(UnityAction onFinish)
        {
            _audioService.DoorClose();
            gameObject.SetActive(true);
            topPad.DOMove(_topStart, 0.25f);
            downPad.DOMove(_downStart, 0.25f).OnComplete(() =>
            {
                onFinish.Invoke();
            });
        }
    }
}
