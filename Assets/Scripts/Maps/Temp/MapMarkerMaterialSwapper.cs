using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace GCU.CultureTour.Map
{
    public class MapMarkerMaterialSwapper : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _mRenderer;

        [SerializeField]
        private Material[] _inRangeMaterial = Array.Empty<Material>();

        [SerializeField]
        private Material[] _outOfRangeMaterial = Array.Empty<Material>();

        [SerializeField]
        private Material[ ] _outOfRangeDiscoveredMaterial = Array.Empty<Material>();

        private CollectibleSO _collectible = null;
        private bool _inRange = false;

        public void Initalise( CollectibleSO marker )
        {
            _collectible = marker;
            SetMaterial( );
        }
        
        public void OnInRange( )
        {
            SetInRange(true);
        }

        public void OnOutOfRange( )
        {
            SetInRange(false);
        }

        public void SetInRange(bool inRange)
        {
            if ( _collectible == null )
            {
                return;
            }

            _inRange = inRange;
            SetMaterial( );
        }

        private void SetMaterial()
        {
            if ( _mRenderer == null )
            {
                return;
            }

            if ( _collectible == null )
            {
                return;
            }

            _mRenderer.materials = _inRange 
                ? _inRangeMaterial 
                : _collectible.Collected 
                    ? _outOfRangeDiscoveredMaterial 
                    : _outOfRangeMaterial;
        }
    } 
}

