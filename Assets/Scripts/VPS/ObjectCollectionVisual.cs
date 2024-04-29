using System.Collections;
using UnityEngine;

namespace GCU.CultureTour
{
    public class ObjectCollectionVisual : MonoBehaviour
    {
        [SerializeField]
        private Transform _collectedObject;

        [SerializeField]
        private ParticleSystem _objectParticles;

        bool _interacted = false;

        public void Animate()
        {
            if (_interacted)
            {
                // Can't interact twice
                return;
            }

            _interacted = true;

            if (_objectParticles != null)
            {
                _objectParticles.Play();
            }
        }
    }
}
