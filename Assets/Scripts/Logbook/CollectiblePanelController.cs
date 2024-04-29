using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

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

        [SerializeField]
        LocalizedStringTable localizedStringTable;
        private StringTable stringTable;

        public void Initialise( CollectibleSO collectible )
        {
            _collectible = collectible;
            UpdateDisplay( );
        }

        public void UpdateDisplay()
        {
            var tableLoading = localizedStringTable.GetTable();
            stringTable = tableLoading;

            if ( _collectible == null ) 
            {
                return;
            }

            if ( _objectName != null )
            {
                _objectName.text = stringTable.GetEntry(_collectible.ObjectName + "_name").Value;
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
                _objectText.text = stringTable.GetEntry(_collectible.ObjectName + "_text").Value;
            }

            if (_dateCollectedText != null)
            {
                _dateCollectedText.text = string.Empty;
            }

            if (_collectible.Collected)
            {
                if (PlayerPrefs.GetString(_collectible.ObjectName + "dateCollected") != null)
                {
                    _dateCollectedText.text = stringTable.GetEntry("date_collected_text").Value + PlayerPrefs.GetString(_collectible.ObjectName + "dateCollected", "No Date Found");
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