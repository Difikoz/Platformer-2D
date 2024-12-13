using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class PawnSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<AudioClip> _getHitClips = new();
        [SerializeField] private List<AudioClip> _deathClips = new();

        private PawnController _pawn;

        public void Initialize()
        {
            _pawn = GetComponent<PawnController>();
        }

        public void PlayGetHitClip()
        {
            if (_getHitClips.Count > 0)
            {
                PlaySound(WorldAudioManager.ChooseRandomClip(_getHitClips));
            }
        }

        public void PlayDeathClip()
        {
            if (_deathClips.Count > 0)
            {
                PlaySound(WorldAudioManager.ChooseRandomClip(_deathClips));
            }
        }

        public void PlaySound(AudioClip clip, bool randomizePitch = true, float volume = 1f, float minPitch = 0.9f, float maxPitch = 1.1f)
        {
            if (clip == null)
            {
                return;
            }
            _audioSource.volume = volume;
            _audioSource.pitch = randomizePitch ? Random.Range(minPitch, maxPitch) : 1f;
            _audioSource.PlayOneShot(clip);
        }
    }
}