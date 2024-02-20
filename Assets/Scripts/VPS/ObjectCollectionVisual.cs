using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GCU.CultureTour
{
    public class ObjectCollectionVisual : MonoBehaviour
    {
        [SerializeField]
        private Transform _objectToMove;

        [SerializeField]
        private ParticleSystem _objectParticles;

        [SerializeField]
        private float _dwellTime = 2f;

        bool _interacted = false;

        public void Animate()
        {
            if (_interacted)
            {
                // can't interact twice.
                return;
            }

            _interacted = true;

            _objectParticles.Play();

            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(_dwellTime);
        }
    }
}
