using UnityEngine;

namespace GCU.CultureTour
{
    public class Compass : MonoBehaviour
    {
        public GameObject camera;
        public GameObject compass;
        private Vector3 direction;
        private float northDirection;

        private void Awake()
        {
            if ( camera == null )
            {
                camera = Camera.main.gameObject;
            }
        }

        void Start()
        {
            northDirection = 0f;
        }

        void Update()
        {
            direction.z = camera.transform.eulerAngles.y - northDirection;
            compass.transform.localEulerAngles = direction;
        }
    }
}
