using Niantic.Platform.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GCU.CultureTour.Logbook
{
    public class LogbookPanelManager : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect _scrollRect;

        [Header("Panels")]
        [SerializeField]
        private GameObject [ ] _preCollectiblePanels = Array.Empty<GameObject>();

        [SerializeField]
        private CollectiblePanelController _collectiblePanelPrefab;

        [SerializeField]
        private GameObject[] _postCollectiblePanels = Array.Empty<GameObject>();

        private Dictionary<CollectibleSO, GameObject> _objectPanels = new Dictionary<CollectibleSO, GameObject>();

        private void Start()
        {
            float height = 0f;

            foreach (var panel in _preCollectiblePanels)
            {
                var p = Instantiate(panel, transform);
                
                var rect = p.transform as RectTransform;
                rect.anchoredPosition = new Vector2(0, -height);
                height += rect?.rect.height ?? 0f;
            }

            if (_collectiblePanelPrefab != null)
            {
                var collectables = GameManager.Instance.GameSettings.OrderedCollectiblesList;

                foreach (var collectable in collectables)
                {
                    var panel = Instantiate(_collectiblePanelPrefab.gameObject, transform);
                    var panelController = panel.GetComponent<CollectiblePanelController>();
                    panelController.Initialise(collectable);

                    var rect = panel.transform as RectTransform;
                    rect.anchoredPosition = new Vector2(0, -height);
                    height += rect?.rect.height ?? 0f;

                    _objectPanels.Add(collectable, panel);
                }
            }
            else
            {
                Log.Error("Collectible panel prefab has not been defined.", gameObject);
            }

            foreach (var panel in _postCollectiblePanels)
            {
                var p = Instantiate(panel, transform);
                
                var rect = p.transform as RectTransform;
                rect.anchoredPosition = new Vector2(0, -height);
                height += rect?.rect.height ?? 0f;
            }

            var t = transform as RectTransform;
            t.sizeDelta = new Vector2(0f, height);

            ScrollToMostRecentCollectible();
        }

        public void ScrollToMostRecentCollectible()
        {
            var mostRecentCollectible = GameManager.Instance.GameSettings.Collectibles
                .Where ( c => c.CollectedOrder >= 0 )
                .OrderByDescending ( c=>c.CollectedOrder )
                .FirstOrDefault();

            if (mostRecentCollectible != null)
            {
                var panel = _objectPanels[mostRecentCollectible];
                SnapTo(panel.transform as RectTransform);
            }
        }

        // from https://stackoverflow.com/questions/30766020/how-to-scroll-to-a-specific-element-in-scrollrect-with-unity-ui
        public void SnapTo(RectTransform target)
        {
            if ( target == null)
            {
                return;
            }

            Canvas.ForceUpdateCanvases();
            var contentPanel = transform as RectTransform;
            contentPanel.anchoredPosition =
                    (Vector2)_scrollRect.transform.InverseTransformPoint(contentPanel.position)
                    - (Vector2)_scrollRect.transform.InverseTransformPoint(target.position);
        }
    }
}
