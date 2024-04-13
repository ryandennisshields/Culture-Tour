using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GCU.CultureTour
{
    public class Collided : MonoBehaviour
    {
        public bool EnableDebug;
        public UnityEvent<GameObject, Collision> ColliderEntered;
        public UnityEvent<GameObject, Collision> ColliderExited;

        private void OnCollisionEnter(Collision collision)
        {
            ColliderEntered?.Invoke(gameObject, collision);
            if (EnableDebug)
            {
                Debug.Log($"{collision.gameObject.name} collided with {gameObject.name}.", gameObject);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            ColliderExited?.Invoke(gameObject, collision);
            if (EnableDebug)
            {
                Debug.Log($"{collision.gameObject.name} exited the collider of {gameObject.name}.", gameObject);
            }
        }
    }
}
