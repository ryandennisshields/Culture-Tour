using Niantic.Lightship.Maps.Core.Coordinates;
using Niantic.Lightship.Maps.MapLayers.Components;
using UnityEngine;

namespace GCU.CultureTour.Map
{
    /// <summary>
    /// This code places the objects on the map and gives them their names.
    /// </summary>    
    public class PlaceLocations : MonoBehaviour
    {

        [SerializeField] private LayerGameObjectPlacement mapLayer;
        
        private GameSettingsSO _gameSettings;

        private void Awake()
        {
            _gameSettings = GameManager.Instance.GameSettings;
        }

        private void Start()
        {
            PlaceMarkers();
        }

        private void PlaceMarkers()
        {
            if ( _gameSettings == null )
            {
                Debug.LogError("There are no settings associated with this map.", gameObject);
                return;
            }

            // place non collectible map markers
            foreach (CollectibleSO collectable in _gameSettings.Collectibles)
            {
                if ( collectable == null )
                {
                    return;
                }

                var marker = collectable.MapMarker;

                if ( marker == null )
                {
                    return;
                }

                var obj = mapLayer.PlaceInstance(new LatLng(marker.Lat, marker.Lng), Quaternion.Euler(marker.Rotation), marker.name);
                obj.Value.GetComponent<MapMarkerLogic>()?.Initalise(collectable);
            }


            // place non collectible map markers
            foreach (MapMarkerSO marker in _gameSettings.Markers)
            {
                var obj = mapLayer.PlaceInstance( new LatLng( marker.Lat, marker.Lng ), Quaternion.Euler(marker.Rotation), marker.name);
                obj.Value.GetComponent<MapMarkerLogic>()?.Initalise(marker);
            }
        }
    }
}