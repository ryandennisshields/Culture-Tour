using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GCU.CultureTour
{
    public class DragandDrop : MonoBehaviour
    {
        // This whole code is sorta temporary. I want to get it inside the Triggered code eventually.

        [SerializeField]
        private GameObject _object;

        private Vector3 mousePosition;
        private bool isDragging = false;

        private Vector3 startPosition;

        public bool clampEnabled = true;
        public Vector3 clampValues = Vector3.zero;

        public Vector3 collectPosition = Vector3.zero;

        public UnityEvent<GameObject> DragDrop;

        private void Start()
        {
            startPosition = transform.position;
        }

        private void OnMouseDown()
        {
            mousePosition = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            isDragging = true;
        }

        private void OnMouseUp()
        {
            isDragging = false;
        }

        private void Update()
        {
            if (isDragging)
            {
                Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
                if (clampEnabled)
                {
                    newPosition.x = Mathf.Clamp(newPosition.x, startPosition.x, startPosition.x + clampValues.x);
                    newPosition.y = Mathf.Clamp(newPosition.y, startPosition.y, startPosition.y + clampValues.y);
                    newPosition.z = Mathf.Clamp(newPosition.z, startPosition.z, startPosition.z + clampValues.z);
                }

                _object.transform.position = newPosition;
            }

            if (_object.transform.position == collectPosition + startPosition)
            {
                DragDrop?.Invoke(gameObject);
            }
        }
    }
}
