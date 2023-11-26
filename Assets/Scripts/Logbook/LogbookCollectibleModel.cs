using System;
using UnityEngine;

namespace GCU.CultureTour.Logbook
{
    public class LogbookCollectibleModel : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _mRenderer;

        [SerializeField]
        private Material[] _notCollectedMaterials = Array.Empty<Material>( );

        [SerializeField]
        private Material[] _collectedMaterials = Array.Empty<Material>( );

        private bool _hasBeenCollected = false;

        public void Initialise(bool hasBeenCollected)
        {
            _hasBeenCollected = hasBeenCollected;
            SetMaterials( );
        }

        private void SetMaterials ( )
        {
            if ( _mRenderer == null )
            {
                Debug.LogError("MeshRenderer not set for logbook model.", gameObject);
                return;
            }

            _mRenderer.materials = _hasBeenCollected ? _collectedMaterials : _notCollectedMaterials;
        }
    }
}
