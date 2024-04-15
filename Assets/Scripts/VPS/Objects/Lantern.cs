using GCU.CultureTour.VPS;
using UnityEngine;

namespace GCU.CultureTour
{
    public class Lantern : Triggered
    {
        [SerializeField]
        private float holdDuration;
        private float hold;
        [SerializeField]
        private GameObject flame;

        [SerializeField]
        private Animation floatAwayAnimation;
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
            if (hold < holdDuration)
                hold = holdDuration;
        }

        private void Update()
        {
            if (isHolding && hold < holdDuration)
            {
                hold += Time.deltaTime;
                flame.transform.localScale = new Vector3(flame.transform.localScale.x + hold * 0.00005f, flame.transform.localScale.y + hold * 0.00005f, flame.transform.localScale.z + hold * 0.00005f);
            }
            if (isHolding && hold >= holdDuration)
            {
                newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - holdPosition);
                newPosition.x = (desiredPosition.x > 0) ? Mathf.Clamp(newPosition.x, startPosition.x, startPosition.x + desiredPosition.x) : Mathf.Clamp(newPosition.x, startPosition.x + desiredPosition.x, startPosition.x);
                newPosition.y = (desiredPosition.y > 0) ? Mathf.Clamp(newPosition.y, startPosition.y, startPosition.y + desiredPosition.y) : Mathf.Clamp(newPosition.y, startPosition.y + desiredPosition.y, startPosition.y);
                newPosition.z = (desiredPosition.z > 0) ? Mathf.Clamp(newPosition.z, startPosition.z, startPosition.z + desiredPosition.z) : Mathf.Clamp(newPosition.z, startPosition.z + desiredPosition.z, startPosition.z);
            }
            if (newPosition == desiredPosition + startPosition)
            {
                floatAwayAnimation.Play();
                hiddenObjectScript.Tapped(gameObject);
            }
        }
    }
}
