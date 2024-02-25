using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace GCU.CultureTour
{
    public class Triggered : MonoBehaviour
    {
        public GameObject _object;
        public bool EnableDebug;
        public UnityEvent<GameObject, Collider> TriggerEntered;
        public UnityEvent<GameObject, Collider> TriggerExited;
        public UnityEvent<GameObject> Tapped;
        [SerializeField]
        [Tooltip("Enable this if you also want to move the object with the player's swipe interaction. Desired position will change where the object needs to be to count as collected.")]
        private bool _dragObject;
        [SerializeField]
        [Tooltip("X and Y (and Z) values change what direction the player needs to swipe/move, e.g. a positive Y value means an upward swipe/move.\nThe higher the value, the further the player needs to swipe/move to collect the object.\nZ value is only used if drag object is enabled!")]
        private Vector3 _desiredPosition;
        public UnityEvent<GameObject> Swiping;
        [SerializeField]
        [Tooltip("Duration the player needs to hold the object.")]
        private float _holdDuration;
        public UnityEvent<GameObject> Holding;

        private bool _isHolding;
        private Vector3 _holdPosition;
        private Vector3 _startPosition;
        private Vector3 _newPosition;
        private float _holdTimer;

        private void Start()
        {
            _startPosition = _object.transform.position;
            _holdTimer = _holdDuration;
        }

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

        private void OnMouseDown()
        {
            _holdPosition = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            _isHolding = true;
        }

        // Tapping
        private void OnMouseUp()
        {
            _isHolding = false;
            _holdTimer = _holdDuration;
            Tapped?.Invoke(gameObject);
            if (EnableDebug)
            {
                Debug.Log($"Mouse Up event called on {gameObject.name}.", gameObject);
            }
        }

        private void Update()
        {
            if (_isHolding)
            {
                _newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - _holdPosition);

                _newPosition.x = (_desiredPosition.x > 0) ? Mathf.Clamp(_newPosition.x, _startPosition.x, _startPosition.x + _desiredPosition.x) : Mathf.Clamp(_newPosition.x, _startPosition.x + _desiredPosition.x, _startPosition.x);
                _newPosition.y = (_desiredPosition.y > 0) ? Mathf.Clamp(_newPosition.y, _startPosition.y, _startPosition.y + _desiredPosition.y) : Mathf.Clamp(_newPosition.y, _startPosition.y + _desiredPosition.y, _startPosition.y);
                _newPosition.z = (_desiredPosition.z > 0) ? Mathf.Clamp(_newPosition.z, _startPosition.z, _startPosition.z + _desiredPosition.z) : Mathf.Clamp(_newPosition.z, _startPosition.z + _desiredPosition.z, _startPosition.z);

                if (_dragObject)
                {
                    _object.transform.position = _newPosition;
                }

                _holdTimer -= Time.deltaTime;

                if (_holdTimer <= 0)
                {
                    Holding?.Invoke(gameObject);
                    if (EnableDebug)
                    {
                        Debug.Log($"Hold event called on {gameObject.name}.", gameObject);
                    }
                }
            }

            if (_newPosition == _desiredPosition + _startPosition)
            {
                Swiping?.Invoke(gameObject);
                if (EnableDebug)
                {
                    Debug.Log($"Swipe event called on {gameObject.name}.", gameObject);
                }
            }
        }
    }
}