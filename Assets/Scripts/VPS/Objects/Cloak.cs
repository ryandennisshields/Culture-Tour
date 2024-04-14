using GCU.CultureTour.VPS;
using UnityEngine;

namespace GCU.CultureTour
{
    public class Cloak : Triggered
    {
        [SerializeField]
        private GameObject userInteraction;
        [SerializeField]
        private Animation cloakAnimation;

        [SerializeField]
        private Vector3 desiredPosition;

        void Start()
        {
            startPosition = gameObject.transform.position;
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
                gameObject.transform.position = newPosition;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            cloakAnimation.Play();
            hiddenObjectScript.Tapped(userInteraction);
        }
    }
}
