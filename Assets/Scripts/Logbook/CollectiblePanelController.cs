using System;
using UnityEngine;

namespace GCU.CultureTour.Logbook
{
    public class CollectiblePanelController : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _areaName;
        
        [SerializeField]
        private TMPro.TextMeshProUGUI _areaText;
        
        [SerializeField]
        private TMPro.TextMeshProUGUI _modelDescription;
        
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

            if ( _areaName != null )
            {
                _areaName.text = _collectible.AreaName;
            }

            if (_logbookModelHolder != null)
            {
                // remove any existing children
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

            if ( _areaText != null)
            {
                _areaText.text = _collectible.AreaText;
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