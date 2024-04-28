using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GCU.CultureTour
{
    public class Horse : Triggered
    {
        [SerializeField]
        private Vector3 desiredDistance;

        float originalYRotation;
        float originalZRotation;

        void Start()
        {
            originalYRotation = 242.05f;
            originalZRotation = 90f;
            startPosition = hiddenObject.transform.position;
        }

        private void OnMouseDown()
        {
            holdPosition = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            isHolding = true;
        }

        private void OnMouseUp()
        {
            isHolding = false;
        }

        private void Update()
        {
            if (newPosition == desiredDistance + startPosition || newPosition == desiredDistance - startPosition)
            {
                desiredDistance = -desiredDistance;
                counter++;
                if (counter == 4)
                {
                    hiddenObjectScript.Tapped(gameObject);
                }
            }
        }

        private void LateUpdate()
        {
            if (isHolding)
            {
                newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - holdPosition);
                newPosition.x = (desiredDistance.x > 0) ? Mathf.Clamp(newPosition.x, startPosition.x, startPosition.x + desiredDistance.x) : Mathf.Clamp(newPosition.x, startPosition.x + desiredDistance.x, startPosition.x);
                newPosition.y = (desiredDistance.y > 0) ? Mathf.Clamp(newPosition.y, startPosition.y, startPosition.y + desiredDistance.y) : Mathf.Clamp(newPosition.y, startPosition.y + desiredDistance.y, startPosition.y);
                newPosition.z = (desiredDistance.z > 0) ? Mathf.Clamp(newPosition.z, startPosition.z, startPosition.z + desiredDistance.z) : Mathf.Clamp(newPosition.z, startPosition.z + desiredDistance.z, startPosition.z);
                float mouseMovementX = (Input.mousePosition.x - holdPosition.x) * 0.03f;
                Quaternion newRotation = Quaternion.Euler(-mouseMovementX - 90, originalYRotation, originalZRotation);
                hiddenObject.transform.localRotation = newRotation;
            }
        }
    }
}
