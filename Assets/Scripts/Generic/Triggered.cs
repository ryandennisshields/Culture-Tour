using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GCU.CultureTour
{
    public class Triggered : MonoBehaviour
    {

        public UnityEvent<GameObject, Collider> TriggerEntered;
        public UnityEvent<GameObject, Collider> TriggerExited;
        public UnityEvent<GameObject> Tapped;

        private void OnTriggerEnter(Collider other)
        {
            TriggerEntered?.Invoke(gameObject, other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExited?.Invoke(gameObject, other);
        }

        private void OnMouseUp()
        {
            Tapped?.Invoke(gameObject);
        }

    }
}