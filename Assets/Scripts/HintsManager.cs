using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace GCU.CultureTour
{
    public class HintsManager : MonoBehaviour
    {
        private string[] Hints => _collectible?.Hints ?? Array.Empty<string>();
        private CollectibleSO _collectible = null;

        int _hintIndex = 0;

        [SerializeField]
        LocalizedStringTable localizedStringTable;
        private StringTable stringTable;

        public void DisplayHint()
        {
            if ( _hintIndex >= Hints.Length )
            {
                _hintIndex = 0;
            }
            var tableLoading = localizedStringTable.GetTable();
            stringTable = tableLoading;
            ShowHintInStatusMessage(stringTable.GetEntry(_collectible.ObjectName + "_hint_" + _hintIndex++).Value);
        }

        private void Awake()
        {
            var scene = SceneManager.GetActiveScene();
            _collectible = GameManager.Instance.GetCollectibleSO(scene.name);
        }

        private void ShowHintInStatusMessage( string hint )
        {
            var statusDisplay = FindObjectOfType<StatusMessageDisplay>();
            if (statusDisplay != null)
            {
                statusDisplay.DisplayMessage(hint, clearQueue: true);
            }
        }
    }
}
