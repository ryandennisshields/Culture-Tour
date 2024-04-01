using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GCU.CultureTour
{
    public class Hammer : Triggered
    {
        [SerializeField]
        private Vector3 desiredPosition;

        [SerializeField]
        private GameObject egg;
        [SerializeField]
        private Material eggCracked;
        [SerializeField]
        private Material eggMoreCracked;

        // insert mesh here for fully broken egg

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
            if (isHolding)
            {
                newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - holdPosition);
                newPosition.x = (desiredPosition.x > 0) ? Mathf.Clamp(newPosition.x, startPosition.x, startPosition.x + desiredPosition.x) : Mathf.Clamp(newPosition.x, startPosition.x + desiredPosition.x, startPosition.x);
                newPosition.y = (desiredPosition.y > 0) ? Mathf.Clamp(newPosition.y, startPosition.y, startPosition.y + desiredPosition.y) : Mathf.Clamp(newPosition.y, startPosition.y + desiredPosition.y, startPosition.y);
                newPosition.z = (desiredPosition.z > 0) ? Mathf.Clamp(newPosition.z, startPosition.z, startPosition.z + desiredPosition.z) : Mathf.Clamp(newPosition.z, startPosition.z + desiredPosition.z, startPosition.z);
                hiddenObject.transform.position = newPosition;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == egg)
            {
                counter++;
                if (counter == 1)
                {
                    egg.GetComponent<Renderer>().material = eggCracked;
                }
                else if (counter == 2)
                {
                    egg.GetComponent<Renderer>().material = eggMoreCracked;
                }
                else if (counter == 3)
                {
                    hiddenObjectScript.Tapped(gameObject);
                }
            }
        }
    }
}
