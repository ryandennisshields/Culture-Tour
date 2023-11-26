using com.cyborgAssets.inspectorButtonPro;
using System;
using System.Linq;
using UnityEngine;

namespace GCU.CultureTour
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance
        {
            get
            {
                if ( _instance == null )
                {
                    var go = new GameObject("Game Manager");
                    DontDestroyOnLoad( go );
                    _instance = go.AddComponent<GameManager>();
                }

                return _instance;
            }
        }

        private static GameManager _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;

                if (_gameSettings == null)
                {
#if DEBUG
                    _gameSettings = Resources.Load<GameSettingsSO>("Default Game Settings");
#else
                    Debug.LogError("Game settings must be set in the inspector.", gameObject);
#endif
                }
                
                Reload();

                return;
            }

            Destroy(gameObject);
        }

        [SerializeField]
        private GameSettingsSO _gameSettings;

        public GameSettingsSO GameSettings => _gameSettings;

        public AnimationSO ClipToPlay => _gameSettings.Animations[0];

        /// <summary>
        /// Gets the collectible for a specific VPS Scene
        /// </summary>
        /// <param name="sceneName">The name of the VPS scene.</param>
        /// <returns>A Collectible or null.</returns>
        public CollectibleSO GetCollectibleSO ( string sceneName )
        {
            return _gameSettings.Collectibles
                .Where(c => c.VpsSceneName == sceneName)
                .FirstOrDefault();
        }

        public void PrepareToPlayAnimation(int collectedOrder)
        {
            // to do - this is temporary 
            // do nothing as we only have one clip to play.
        }

        private int _nextCollectable = 0;

        [ProButton]
        public void DiscoverObject(int objectIndex)
        {
            if (_gameSettings.Collectibles.Length < objectIndex)
            {
                return;
            }

            _gameSettings.Collectibles[objectIndex].MarkCollected(++_nextCollectable);
        }

        public void DiscoverObject(CollectibleSO collectible)
        {
            for (int i = 0; i < _gameSettings.Collectibles.Length; i++)
            {
                if (_gameSettings.Collectibles[i] == collectible)
                {
                    DiscoverObject(i);
                    return;
                }
            }

            Debug.LogWarning("Collectible could not be found in the list of collectibles.");
        }

        [ProButton]
        public void Reload()
        {
            _nextCollectable = 0;

            // load the status of all collectibles
            foreach (var collectable in _gameSettings.Collectibles)
            {
                collectable.Load();

                // get the largest collected order
                if (collectable.CollectedOrder > _nextCollectable)
                {
                    _nextCollectable = collectable.CollectedOrder;
                }
            }
        }

#if DEBUG

        [ProButton]
        private void DiscoverAllObjects()
        {
            foreach (var collectable in _gameSettings.Collectibles)
            {
                collectable.MarkCollected(++_nextCollectable);
            }
        }

        [ProButton]
        private void UndiscoverAllObjects()
        {
            foreach (var collectable in _gameSettings.Collectibles)
            {
                collectable.MarkCollected(-1);
            }
        }

        [ProButton]
        private void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Reload();
        }

#endif

    }
}