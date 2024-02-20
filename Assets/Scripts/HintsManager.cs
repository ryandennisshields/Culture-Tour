using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GCU.CultureTour
{
    public class HintsManager : MonoBehaviour
    {
        private string[] Hints => _collectible?.Hints ?? Array.Empty<string>();
        private CollectibleSO _collectible = null;

        int _hintIndex = 0;
        bool _allHintsUsed = false;

        public int HowManyHintsHaveBeenUsed
        {
            get
            {
                if (_allHintsUsed)
                {
                    return Hints.Length;
                }

                return _hintIndex;
            }
        }

        public void DisplayHint()
        {
            if ( _hintIndex >= Hints.Length )
            {
                _hintIndex = 0;

                if ( ! _allHintsUsed )
                {
                    ShowHintInStatusMessage(CollectibleSO.NO_MORE_HINTS);
                    _allHintsUsed = true;
                    return;
                }
            }

            ShowHintInStatusMessage(Hints[_hintIndex++]);
        }

        public void DisplayObjectCollected() 
        {
            ShowHintInStatusMessage("Object Found!\nAdded to Logbook");
        }

        private void Awake()
        {
            var scene = SceneManager.GetActiveScene();
            _collectible = GameManager.Instance.GetCollectibleSO(scene.name);
        }

        /// <summary>
        /// This is just a temporary solution.
        /// 2023-11-10
        /// </summary>
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
