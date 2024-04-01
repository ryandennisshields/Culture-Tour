using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GCU.CultureTour
{
    public class Horse : Triggered
    {
        [SerializeField]
        private Vector3[] desiredPositions;

        // Start is called before the first frame update
        void Start()
        {
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
            if (newPosition == desiredPositions[counter] + startPosition)
            {
                counter++;
                if (counter  == desiredPositions.Length - 1)
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
                newPosition.x = (desiredPositions[counter].x > 0) ? Mathf.Clamp(newPosition.x, startPosition.x, startPosition.x + desiredPositions[counter].x) : Mathf.Clamp(newPosition.x, startPosition.x + desiredPositions[counter].x, startPosition.x);
                newPosition.y = (desiredPositions[counter].y > 0) ? Mathf.Clamp(newPosition.y, startPosition.y, startPosition.y + desiredPositions[counter].y) : Mathf.Clamp(newPosition.y, startPosition.y + desiredPositions[counter].y, startPosition.y);
                newPosition.z = (desiredPositions[counter].z > 0) ? Mathf.Clamp(newPosition.z, startPosition.z, startPosition.z + desiredPositions[counter].z) : Mathf.Clamp(newPosition.z, startPosition.z + desiredPositions[counter].z, startPosition.z);
            }
        }
    }
}
