using System.Collections;
using UnityEngine;

namespace WinterUniverse
{
    public class WorldManager : Singleton<WorldManager>
    {
        private bool _initialized;
        private PlayerController _player;
        private WorldAudioManager _audioManager;
        private WorldCameraManager _cameraManager;
        private WorldLayerManager _layerManager;
        private WorldUIManager _UIManager;

        public PlayerController Player => _player;
        public WorldAudioManager AudioManager => _audioManager;
        public WorldCameraManager CameraManager => _cameraManager;
        public WorldLayerManager LayerManager => _layerManager;
        public WorldUIManager UIManager => _UIManager;

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(Initialization());
        }

        private IEnumerator Initialization()
        {
            yield return null;
            _player = FindFirstObjectByType<PlayerController>();
            _audioManager = GetComponentInChildren<WorldAudioManager>();
            _cameraManager = GetComponentInChildren<WorldCameraManager>();
            _layerManager = GetComponentInChildren<WorldLayerManager>();
            _UIManager = GetComponentInChildren<WorldUIManager>();
            yield return null;
            _player.Initialize();
            yield return null;
            _initialized = true;
        }

        private void FixedUpdate()
        {
            if (!_initialized)
            {
                return;
            }
            _player.OnFixedUpdate();
        }

        private void LateUpdate()
        {
            if (!_initialized)
            {
                return;
            }
            _cameraManager.OnLateUpdate();
        }
    }
}