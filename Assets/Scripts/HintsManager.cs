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

        public void DisplayHint()
        {
            if ( _hintIndex >= Hints.Length )
            {
                _hintIndex = 0;
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
