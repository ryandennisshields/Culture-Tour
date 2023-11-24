using Niantic.Lightship.Maps.Core.Coordinates;
using Niantic.Lightship.Maps.MapLayers.Components;
using UnityEngine;

namespace GCU.CultureTour.Map
{
    public class PlaceLocations : MonoBehaviour
    {
        /// <summary>
        /// This code places the objects on the map and gives them their names.
        /// </summary>

        [SerializeField] private GameSettingsSO _gameSettings;
        [SerializeField] private LayerGameObjectPlacement mapLayer;

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

            // place non collectable map markers
            foreach (CollectableSO collectable in _gameSettings.Collectables)
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
                obj.Value.GetComponent<MapMarkerLogic>()?.Initalise(marker);
            }


            // place non collectable map markers
            foreach (MapMarkerSO marker in _gameSettings.Markers)
            {
                var obj = mapLayer.PlaceInstance( new LatLng( marker.Lat, marker.Lng ), Quaternion.Euler(marker.Rotation), marker.name);
                obj.Value.GetComponent<MapMarkerLogic>()?.Initalise(marker);
            }
        }
    }
}