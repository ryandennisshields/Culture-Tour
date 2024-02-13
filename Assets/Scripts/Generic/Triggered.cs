using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        public UnityEvent<GameObject> Swiping;
        public UnityEvent<GameObject> Holding;

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

        // Tapping
        private void OnMouseUp()
        {
            Tapped?.Invoke(gameObject);
            if (EnableDebug)
            {
                Debug.Log($"Mouse Up event called on {gameObject.name}.", gameObject);
            }
        }

        private Vector2 startTouchPos;
        private Vector2 endTouchPos;

        private void Update()
        {
            // Swiping Upward
            float swipeDistance = 5f;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouchPos = Input.GetTouch(0).position;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouchPos = Input.GetTouch(0).position;

                if (endTouchPos.y > startTouchPos.y + swipeDistance)
                {
                    Swiping?.Invoke(gameObject);
                    if (EnableDebug) 
                    {
                        Debug.Log($"Swipe upward event called on {gameObject.name}.", gameObject);
                    }
                }
            }

            // Hold
            float holdTimer = 3f;
            if (Input.touchCount > 0)
            {
                Touch first = Input.GetTouch(0);
                if (first.phase == TouchPhase.Stationary)
                {
                    holdTimer -= Time.deltaTime;
                    if (holdTimer <= 0)
                    {
                        Holding?.Invoke(gameObject);
                        if (EnableDebug)
                        {
                            Debug.Log($"Hold event called on {gameObject.name}.", gameObject);
                        }
                        holdTimer = 3f;
                    }
                }
            }
        }
    }
}