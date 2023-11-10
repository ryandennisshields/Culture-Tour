using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GCU.CultureTour
{
    public class HiddenObject : MonoBehaviour
    {
        public UnityEvent OnObjectDiscoveredImmediate;
        public UnityEvent OnObjectDiscovered;

        [SerializeField]
        float discoveryDelay = 5.0f;

        [SerializeField]
        string message = "Discovered";

        public bool IsDiscovered { get; private set; } = false;

        public void Tapped ( GameObject go )
        {
            IsDiscovered = true;
            
            // prevent the user from discovering an object more than once.
            go.SetActive( false );

            FindObjectOfType<StatusMessageDisplay>()?
                .DisplayMessage(message, clearQueue: true);

            OnObjectDiscoveredImmediate?.Invoke();

            StartCoroutine(DiscoveredDelay());
        }

        private IEnumerator DiscoveredDelay()
        {
            yield return new WaitForSeconds(discoveryDelay);

            OnObjectDiscovered?.Invoke();
        }
    }
}
