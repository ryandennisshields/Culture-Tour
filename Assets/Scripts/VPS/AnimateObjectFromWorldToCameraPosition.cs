using System;
using System.Collections;
using UnityEngine;

namespace GCU.CultureTour.VPS
{
    public class AnimateObjectFromWorldToCameraPosition : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("If left empty, Main Camera will be used.")]
        private Camera _camera = null;

        [SerializeField]
        [Tooltip("Screen position relative to screen width / height. 0,0 is bottom left, 1,1 is top right.")]
        private Vector2 _endPositionScreenPoint;

        [SerializeField]
        private Transform _objectToMove;

        [SerializeField]
        private float _dwellTime = 0.2f;

        [SerializeField]
        private float _movementTime = 2f;

        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("0 means shrink to nothing, 1 keeps the same size.")]
        private float _scaleFactor = 0.6f;

        bool _moved = false;
        bool _moving = false;

        Vector3 _startingScreenPosition;
        Vector3 _startingPosition;
        float _startingDistanceFromCamera;
        Vector3 _endPosition;

        Vector3 _startingSacle;
        Vector3 _endScale;

        public void AnimateMove( )
        {
            if (_moved)
            {
                // can't move twice.
                return;
            }

            _moved = true;
            _moving = true;
            
            // re-parent this object to the camera so that it and its children sticks to camera movements.
            transform.SetParent( _camera.transform, true );

            // get the current position of the object that we want to animate
            _startingScreenPosition = _camera.WorldToScreenPoint( _objectToMove.position );
            _startingDistanceFromCamera = Vector3.Distance(_camera.transform.position, _objectToMove.position);

            _startingSacle = _objectToMove.localScale;
            _endScale = _startingSacle * _scaleFactor ;

            // make sure that update runs at least once before the first pass of StartMovement
            Update();

            StartCoroutine(StartMovement());
        }
        
        private void Awake()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
        }

        private void Update()
        {
            if (!_moving)
            {
                return;
            }

            _startingPosition = _camera.ScreenToWorldPoint(_startingScreenPosition);
            _endPosition = _camera.ScreenToWorldPoint(
                new Vector3(
                    _endPositionScreenPoint.x * _camera.pixelWidth,
                    _endPositionScreenPoint.y * _camera.pixelHeight,
                    _startingDistanceFromCamera));

        }

        private IEnumerator StartMovement()
        {
            yield return new WaitForSeconds(_dwellTime);

            float t;
            float runningTime;

            runningTime = 0f;
            t = 0f;

            if (_movementTime > 0f)
            {
                while (t < 1)
                {
                    runningTime += Time.deltaTime;
                    t = runningTime / _movementTime;

                    _objectToMove.position = Vector3.Lerp(_startingPosition, _endPosition, t);
                    _objectToMove.localScale = Vector3.Lerp(_startingSacle, _endScale, t);

                    yield return null;
                }
            }

            _objectToMove.position = _endPosition;
            _objectToMove.localScale = _endScale;

            _moving = false;
        }
    }
}
