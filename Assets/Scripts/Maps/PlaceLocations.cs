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

        [SerializeField] private MapSettingsSO _mapSettings;
        [SerializeField] private LayerGameObjectPlacement mapLayer;

        private void Start()
        {
            PlaceMarkers();
        }

        private void PlaceMarkers()
        {
            if ( _mapSettings == null )
            {
                Debug.LogError("There are no settings associated with this map.", gameObject);
                return;
            }

            foreach (MapMarkerSO marker in _mapSettings.Markers)
            {
                var obj = mapLayer.PlaceInstance( new LatLng( marker.Lat, marker.Lng ), Quaternion.Euler(marker.Rotation), marker.Name);
                obj.Value.GetComponent<MapMarkerLogic>()?.Initalise(marker);
            }
        }
    }
}