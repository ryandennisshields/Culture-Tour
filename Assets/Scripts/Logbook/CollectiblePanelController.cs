using System;
using UnityEngine;

namespace GCU.CultureTour.Logbook
{
    public class CollectiblePanelController : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _objectName;
        
        [SerializeField]
        private TMPro.TextMeshProUGUI _objectText;

        [SerializeField]
        private TMPro.TextMeshProUGUI _dateCollectedText;
        
        [SerializeField]
        private Transform _logbookModelHolder;

        CollectibleSO _collectible;

        public void Initialise( CollectibleSO collectible )
        {
            _collectible = collectible;
            UpdateDisplay( );
        }

        public void UpdateDisplay()
        {

            if ( _collectible == null ) 
            {
                return;
            }

            if ( _objectName != null )
            {
                _objectName.text = _collectible.ObjectName;
            }

            if (_logbookModelHolder != null)
            {
                // Remove any existing children
                for (int i = 0; i < _logbookModelHolder.childCount; i++)
                {
                    Destroy(_logbookModelHolder.GetChild(i));
                }

                if (_collectible.LogbookModel != null)
                {
                    var modelGO = Instantiate(_collectible.LogbookModel.gameObject, _logbookModelHolder);
                    var model = modelGO.GetComponent<LogbookCollectibleModel>();
                    model.Initialise ( _collectible.Collected );
                }
            }

            if ( _objectText != null)
            {
                _objectText.text = _collectible.ObjectText;
            }

            if (_dateCollectedText != null)
            {
                _dateCollectedText.text = "";
            }

            if (_collectible.Collected)
            {
                if (PlayerPrefs.GetString(_collectible.ObjectName + "dateCollected") != null)
                {
                    _dateCollectedText.text = PlayerPrefs.GetString(_collectible.ObjectName + "dateCollected", "No Date Found");
                }
            }
        }

        public void PlayAnimation()
        {
            if ( _collectible?.CollectedOrder > 0 )
            {
                GameManager.Instance.PrepareToPlayAnimation(_collectible.CollectedOrder);
            }
        }
    }
}