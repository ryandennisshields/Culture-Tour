using com.cyborgAssets.inspectorButtonPro;
using System;
using System.Collections;
using System.Collections.Generic;
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

                    _gameSettings = Resources.Load<GameSettingsSO>("Default Game Settings");   
                    
                }
                
                Reload();

                return;
            }

            Destroy(gameObject);
        }

        [SerializeField]
        private GameSettingsSO _gameSettings;

        public GameSettingsSO GameSettings => _gameSettings;

        public AnimationSO ClipToPlay = null;

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

        public void PrepareToPlayAnimation(int animationToPlay)
        {
            var collectibles = _gameSettings.Collectibles;
            if (animationToPlay == 0)
            {
                if (collectibles.Any(c => c.name == "Sword" && c.Collected) && collectibles.Any(c => c.name == "Cloak" && c.Collected))
                    ClipToPlay = _gameSettings.Animations[0];
            }
            if (animationToPlay == 1)
            {
                if (collectibles.Any(c => c.name == "Hammer" && c.Collected) && collectibles.Any(c => c.name == "Toy Wooden Horse" && c.Collected))
                    ClipToPlay = _gameSettings.Animations[1];
            }
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

            // Load the status of all collectibles
            foreach (var collectable in _gameSettings.Collectibles)
            {
                collectable.Load();

                // Get the largest collected order
                if (collectable.CollectedOrder > _nextCollectable)
                {
                    _nextCollectable = collectable.CollectedOrder;
                }
            }
        }

#if DEBUG

        [ProButton]
        public void DiscoverAllObjects()
        {
            foreach (var collectable in _gameSettings.Collectibles)
            {
                collectable.MarkCollected(++_nextCollectable);
            }
        }

        [ProButton]
        public void UndiscoverAllObjects()
        {
            foreach (var collectable in _gameSettings.Collectibles)
            {
                collectable.MarkCollected(-1);
            }

            Reload();
        }

        [ProButton]
        public void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Reload();
        }

        internal IEnumerable<object> GetUncollectedCollectibles()
        {
            throw new NotImplementedException();
        }

        public bool ShowLog;

        uint queueSize = 15;
        Queue myLogQueue = new Queue();

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            myLogQueue.Enqueue("[" + type + "] : " + logString);
            if (type == LogType.Exception)
                myLogQueue.Enqueue(stackTrace);
            while (myLogQueue.Count > queueSize)
                myLogQueue.Dequeue();
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width - 400, 0, 400, Screen.height));
            GUILayout.Label("\n" + string.Join("\n", myLogQueue.ToArray()));
            GUILayout.EndArea();
        }

#endif

    }
}