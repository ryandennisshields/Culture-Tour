using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GCU.CultureTour
{
    public class Triggered : MonoBehaviour
    {
        public bool EnableDebug;
        public UnityEvent<GameObject, Collider> TriggerEntered;
        public UnityEvent<GameObject, Collider> TriggerExited;
        public UnityEvent<GameObject> Tapped;

        private void OnTriggerEnter(Collider other)
        {
            TriggerEntered?.Invoke(gameObject, other);
            if (EnableDebug)
            {
                Debug.Log($"{other.gameObject.name} triggered {gameObject.name}.", gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExited?.Invoke(gameObject, other);
            if (EnableDebug)
            {
                Debug.Log($"{other.gameObject.name} exited the trigger collider of {gameObject.name}.", gameObject);
            }
        }

        private void OnMouseUp()
        {
            Tapped?.Invoke(gameObject);
            if (EnableDebug)
            {
                Debug.Log($"Mouse Up event called on {gameObject.name}.", gameObject);
            }
        }

    }
}