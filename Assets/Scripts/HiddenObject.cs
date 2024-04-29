using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace GCU.CultureTour.VPS
{
    public class HiddenObject : MonoBehaviour
    {
        public UnityEvent OnObjectDiscoveredImmediate;
        public UnityEvent OnObjectDiscovered;

        [SerializeField]
        float discoveryDelay = 5.0f;

        private string message;

        public bool IsDiscovered { get; private set; } = false;

        [SerializeField]
        LocalizedStringTable localizedStringTable;
        private StringTable stringTable;

        public void Tapped ( GameObject go )
        {
            var tableLoading = localizedStringTable.GetTable();
            stringTable = tableLoading;

            message = stringTable.GetEntry("object_found").Value;

            IsDiscovered = true;

            var scene = SceneManager.GetActiveScene();
            var collectible = GameManager.Instance.GetCollectibleSO(scene.name);
            GameManager.Instance.DiscoverObject(collectible);

            // Prevent the user from discovering an object more than once
            go.SetActive( false );

            FindObjectOfType<StatusMessageDisplay>()?
                .DisplayMessage(message, clearQueue: true);

            OnObjectDiscoveredImmediate?.Invoke();

            StartCoroutine(DiscoveredDelay());
        }

        public void SelectAnimationToPlay(int animationToPlay)
        {
            GameManager.Instance.PrepareToPlayAnimation(animationToPlay);
        }

        private IEnumerator DiscoveredDelay()
        {
            yield return new WaitForSeconds(discoveryDelay);

            OnObjectDiscovered?.Invoke();
        }
    }
}
