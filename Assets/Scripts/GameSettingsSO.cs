using UnityEngine;
using GCU.CultureTour.Map;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine.Rendering;

namespace GCU.CultureTour
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "GCU/Create Game Settings")]
    public class GameSettingsSO : ScriptableObject
    {

        [Header("Collectables")]
        [Tooltip("The order these are in is the order they will appear in the logbook if they are undiscovered.")]
        public CollectableSO[] Collectables = new CollectableSO[0];
        public string AreaTextWhenObejectNotDiscovered = string.Empty;

        [Header("Animations")]
        [Tooltip("The order these are in is the ordert the player will see them played.")]
        public AnimationSO[] Animations = new AnimationSO[0];
        
        [Header("Map Markers (excluding collectables!)")] 
        public MapMarkerSO[] Markers = new MapMarkerSO[0];

        private int _nextCollectable = 0;

        private void Awake()
        {
            // load the status of all collectables
            foreach (var collectable in Collectables)
            {
                collectable.Load();

                // get the largest collected order
                if ( collectable.CollectedOrder > _nextCollectable )
                {
                    _nextCollectable = collectable.CollectedOrder;
                }
            }
        }

        [ProButton]
        public void DiscoverObject(int objectIndex)
        {
            if (Collectables.Length > objectIndex)
            {
                return;
            }

            Collectables[objectIndex].MarkCollected(++_nextCollectable);
        }

#if DEBUG

        [ProButton]
        private void DiscoverAllObjects()
        {
            foreach (var collectable in Collectables)
            {
                collectable.MarkCollected(++_nextCollectable);
            }
        }

        [ProButton]
        private void UndiscoverAllObjects()
        {
            foreach (var collectable in Collectables)
            {
                collectable.MarkCollected(-1);
            }
        }
        
        [ProButton]
        private void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            foreach (var collectable in Collectables)
            {
                collectable.Load();
            }
        }
#endif
    }
}
