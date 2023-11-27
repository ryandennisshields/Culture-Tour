using UnityEngine;

namespace GCU.CultureTour.Map
{
    public class MapMarkerSpriteSwapper : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Sprite _inRangeSprite;

        [SerializeField]
        private Sprite _outOfRangeSprite;

        [SerializeField]
        private Sprite _outOfRangeDiscoveredSprite;

        private CollectibleSO _collectible = null;
        private bool _inRange = false;

        public void Initalise( CollectibleSO marker )
        {
            _collectible = marker;
            SetSprite( );
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
            SetSprite( );
        }

        private void SetSprite()
        {
            if ( _spriteRenderer == null )
            {
                return;
            }

            if ( _collectible == null )
            {
                return;
            }

            _spriteRenderer.sprite = _inRange 
                ? _inRangeSprite 
                : _collectible.Collected 
                    ? _outOfRangeDiscoveredSprite 
                    : _outOfRangeSprite;
        }
    } 
}

