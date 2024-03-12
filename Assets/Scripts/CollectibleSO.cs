using GCU.CultureTour.Logbook;
using GCU.CultureTour.Map;
using System;
using UnityEngine;

namespace GCU.CultureTour
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "GCU/Create Collectible")]
    public class CollectibleSO : ScriptableObject
    {
        // This should be in a localization dictionary
        // FC 2023-11-10
        public const string NO_MORE_HINTS = "No more Hints!";

        public MapMarkerSO MapMarker;
        [Header("Logbook")]
        public string ObjectName;
        public string ObjectText;
        private string DateCollectedText;
        public LogbookCollectibleModel LogbookModel;

        [Header ("VPS Scene information")]
        public string VpsSceneName;
        public string[ ] Hints = new string[ 0 ];

        public bool Collected => CollectedOrder >= 0;
        public int CollectedOrder { get; private set; } = -1; 

        /// <summary>
        /// Mark the collectible as collected (or not). 
        /// The change will be persisted to PlayerPrefs.
        /// </summary>
        /// <param name="collectedOrder">
        /// 0 or a positive number indicates the order in which this collectible was collected.
        /// -1 indicates that this collectible has not been collected.
        /// </param>
        public void MarkCollected(int collectedOrder)
        {
            if (CollectedOrder < -1 )
            {
                CollectedOrder = -1;
            }

            CollectedOrder = collectedOrder;
            Save();
        }

        private const string KEY_PREFIX = "Collectible ";

        public void Load( )
        {
            string key = KEY_PREFIX + name;

            CollectedOrder = PlayerPrefs.HasKey(key) 
                ? PlayerPrefs.GetInt(key) 
                : -1;
        }

        private void Save()
        {
            string key = KEY_PREFIX + name;

            PlayerPrefs.SetInt(key, CollectedOrder);
            DateTime dt = DateTime.Now;
            PlayerPrefs.SetString(ObjectName + "dateCollected", "This object was collected on " + dt.ToString("yyyy-MM--dd"));
            PlayerPrefs.Save();
        }
    }
}