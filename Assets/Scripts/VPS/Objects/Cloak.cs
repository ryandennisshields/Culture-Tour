using GCU.CultureTour.VPS;
using UnityEngine;

namespace GCU.CultureTour
{
    public class Cloak : Triggered
    {
        [SerializeField]
        private Animation cloakAnimation;

        [SerializeField]
        private Vector3 desiredPosition;

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
                newPosition.x = Mathf.Clamp(newPosition.x, startPosition.x - desiredPosition.x, startPosition.x + desiredPosition.x);
                newPosition.y = Mathf.Clamp(newPosition.y, startPosition.y - desiredPosition.y, startPosition.y + desiredPosition.y);
                newPosition.z = Mathf.Clamp(newPosition.z, startPosition.z - desiredPosition.z, startPosition.z + desiredPosition.z);
            }

            if (newPosition.x == desiredPosition.x + startPosition.x || newPosition.y == desiredPosition.y + startPosition.y || newPosition.z == desiredPosition.z + startPosition.z || newPosition.x == startPosition.x - desiredPosition.x || newPosition.y == startPosition.y - desiredPosition.y || newPosition.z == startPosition.z - desiredPosition.z)
            {
                cloakAnimation.Play();
                hiddenObjectScript.Tapped(gameObject);
            }
        }
    }
}
