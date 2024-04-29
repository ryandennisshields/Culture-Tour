using UnityEngine;

namespace GCU.CultureTour
{
    public class FaceCamera : MonoBehaviour
    {
        Camera _mainCamera;

        void Awake()
        {
            _mainCamera = Camera.main;
        }

        void LateUpdate()
        {
            transform.LookAt(_mainCamera.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}
