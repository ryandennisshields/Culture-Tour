using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace GCU.CultureTour.Animations
{
    [RequireComponent(typeof(VideoPlayer))]
    [RequireComponent(typeof(SceneLoader))]
    public class LoadVideoClip : MonoBehaviour
    {
        private VideoPlayer _player;
        private AnimationSO _animation;
        private SceneLoader _sceneLoader;

        /// <summary>
        /// The next scene will load once this is set to true.
        /// </summary>
        private bool _loadScene = false;

        private int _loopCounter = 0;

        private void Awake()
        {
            _player = GetComponent<VideoPlayer>();
            _sceneLoader = GetComponent<SceneLoader>();
        }

        private void OnEnable()
        {
            _player.loopPointReached += _player_loopPointReached;
        }

        private void OnDisable()
        {
            _player.loopPointReached -= _player_loopPointReached;
        }

        private void _player_loopPointReached(VideoPlayer source)
        {
            _loopCounter++;
        }

        private IEnumerator Start()
        {
            // get clip from Game manager
            _animation = GameManager.Instance.ClipToPlay;

            if (_animation == null)
            {
                Debug.LogError("Video Player Scene was loaded when no video clip is ready to play.", gameObject);
                _sceneLoader.LoadScene();
                yield break;
            }

            _player.isLooping = _animation.LoopCount == -1;

            _player.clip = _animation.Clip;
            _player.Play();

            float playingTime = 0f;

            yield return null;

            while ( ! _loadScene )
            {
                if (_animation.PlayLength > 0 && playingTime > _animation.PlayLength)
                {
                    // playing length field has been exceed.
                    _loadScene = true;
                }
                
                playingTime += Time.deltaTime;

                if (_animation.LoopCount >= 0)
                {
                    if ( _loopCounter - 1 >= _animation.LoopCount)
                    {
                        _loadScene = true;
                    }
                }

                yield return null;
            }

            _sceneLoader.LoadScene();
        }
    }
}