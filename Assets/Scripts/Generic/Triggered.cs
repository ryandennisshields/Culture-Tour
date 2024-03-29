using GCU.CultureTour.VPS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GCU.CultureTour
{
    public class Triggered : MonoBehaviour
    {
        public GameObject _object;
        [SerializeField]
        [Tooltip("Use this if you want the player to use multiple actions.\nIn each event, use the 'Add Action' function and set the number to the order you want the action to be in.\nFor actions like swiping and holding, remember to add multiple values to the array for each extra action!")]
        private float maxActions;
        public bool EnableDebug;
        public UnityEvent<GameObject, Collider> TriggerEntered;
        public UnityEvent<GameObject, Collider> TriggerExited;
        public UnityEvent<GameObject> Tapped;
        [SerializeField]
        [Tooltip("Enable this if you also want to move the object with the player's swipe interaction. Desired position will change where the object needs to be to count as collected.")]
        private bool _dragObject;
        [SerializeField]
        [Tooltip("X and Y (and Z) values change what direction the player needs to swipe/move, e.g. a positive Y value means an upward swipe/move.\nThe higher the value, the further the player needs to swipe/move to collect the object.\nZ value is only used if drag object is enabled!")]
        private Vector3[] _desiredPosition;
        public UnityEvent<GameObject> Swiping;
        [SerializeField]
        [Tooltip("Duration the player needs to hold the object.")]
        private float[] _holdDuration;
        public UnityEvent<GameObject> Holding;
        [SerializeField]
        private Material _notCompletedMaterial;
        [SerializeField]
        private Material _completedMaterial;
        [SerializeField]
        private GameObject[] _objectsToChangeMaterial;
        public UnityEvent<GameObject> MultipleObjects;

        private int _swipeIndex = 0;
        private int _holdIndex = 0;
        private bool _isHolding;
        private Vector3 _holdPosition;
        private Vector3 _startPosition;
        private Vector3 _newPosition;
        private float _holdTimer;
        private int currentObjectsCollected;

        private void Start()
        {
            _startPosition = _object.transform.position;
            if (_holdDuration.Length != 0)
                _holdTimer = _holdDuration[_holdIndex];
            if (_notCompletedMaterial != null)
            {
                foreach (var obj in _objectsToChangeMaterial)
                {
                    obj.GetComponent<MeshRenderer>().material = _notCompletedMaterial;
                }
            }
        }

        private int counter = 0;

        public void IncreaseCounter() 
        { 
            counter++;
            // Setting this to 3 as a temporary solution, as this implementation isn't flexible
            if (counter == 3)
            {
                GetComponentInParent<HiddenObject>().Tapped(gameObject);
            }
        }

        private float currentStep = 0;
        private bool addActionExecuted = false;

        public void AddAction(float orderNumber)
        {
            if (!addActionExecuted && currentStep == orderNumber)
            {
                if (_swipeIndex < _desiredPosition.Length - 1 && _newPosition == _desiredPosition[_swipeIndex] + _startPosition)
                    _swipeIndex++;
                if (_holdIndex < _holdDuration.Length - 1 && _holdTimer <= 0)
                    _holdIndex++;
                addActionExecuted = true;
                currentStep++;
                if (currentStep == maxActions)
                {
                    GetComponentInParent<HiddenObject>().Tapped(gameObject);
                }
            }
        }

        public void CollectPartofObject(GameObject otherObject)
        {
            currentObjectsCollected++;
            var statusDisplay = FindObjectOfType<StatusMessageDisplay>();
            if (statusDisplay != null)
            {
                // Setting this to "out of 3" as a temporary solution, as this implementation isn't flexible
                statusDisplay.DisplayMessage($"{currentObjectsCollected} / 3 Collected", true);
            }
            otherObject.SetActive(false);
            // Ditto from above
            if (currentObjectsCollected == 3)
            {
                foreach (var obj in _objectsToChangeMaterial)
                {
                    obj.GetComponent<MeshRenderer>().material = _completedMaterial;
                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("true");
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

        private void OnMouseUp()
        {
            _isHolding = false;
            if (_holdDuration.Length != 0)
                _holdTimer = _holdDuration[_holdIndex];
            addActionExecuted = false;
            Tapped?.Invoke(gameObject);
            if (EnableDebug)
            {
                Debug.Log($"Mouse Up event called on {gameObject.name}.", gameObject);
            }
            // Setting this to "out of 3" as a temporary solution, as this implementation isn't flexible
            if (currentObjectsCollected == 3)
            {
                MultipleObjects?.Invoke(gameObject);
            }
        }

        private void Update()
        {
            if (_isHolding)
            {
                _newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - _holdPosition);

                if (_desiredPosition.Length != 0)
                {
                    _newPosition.x = (_desiredPosition[_swipeIndex].x > 0) ? Mathf.Clamp(_newPosition.x, _startPosition.x, _startPosition.x + _desiredPosition[_swipeIndex].x) : Mathf.Clamp(_newPosition.x, _startPosition.x + _desiredPosition[_swipeIndex].x, _startPosition.x);
                    _newPosition.y = (_desiredPosition[_swipeIndex].y > 0) ? Mathf.Clamp(_newPosition.y, _startPosition.y, _startPosition.y + _desiredPosition[_swipeIndex].y) : Mathf.Clamp(_newPosition.y, _startPosition.y + _desiredPosition[_swipeIndex].y, _startPosition.y);
                    _newPosition.z = (_desiredPosition[_swipeIndex].z > 0) ? Mathf.Clamp(_newPosition.z, _startPosition.z, _startPosition.z + _desiredPosition[_swipeIndex].z) : Mathf.Clamp(_newPosition.z, _startPosition.z + _desiredPosition[_swipeIndex].z, _startPosition.z);
                }

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

            if (_desiredPosition.Length != 0)
            {
                if (_newPosition == _desiredPosition[_swipeIndex] + _startPosition)
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
}