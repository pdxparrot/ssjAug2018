using System.Collections;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Audio;

namespace pdxpartyparrot.Core.Audio
{
    public sealed class AudioManager : SingletonBehavior<AudioManager>
    {
        private const string MasterVolumeKey = "audio.volume.master";
        private const string MusicVolumeKey = "audio.volume.music";
        private const string SFXVolumeKey = "audio.volume.sfx";

        [SerializeField]
        private AudioMixer _mixer;

        public AudioMixer Mixer => _mixer;

#region Mixer Groups
        [Header("Mixer Groups")]

        [SerializeField]
        private string _musicMixerGroupName = "Music";

        [SerializeField]
        private string _sfxMixerGroupName = "SFX";
#endregion

        [Space(10)]

#region Attributes
        [Header("Parameters")]

        [SerializeField]
        private string _masterVolumeParameter = "MasterVolume";

        [SerializeField]
        private string _musicVolumeParameter = "MusicVolume";

        [SerializeField]
        private string _sfxVolumeParameter = "SFXVolume";
#endregion

        [Space(10)]

#region SFX
        [Header("SFX")]

        [SerializeField]
        private AudioSource _oneShotAudioSource;
#endregion

        [Space(10)]

#region Music
        [Header("Music")]

        [SerializeField]
        private AudioSource _music1AudioSource;

        [SerializeField]
        private AudioSource _music2AudioSource;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _musicCrossFade;

        // 0 == music1, 1 = music2
        public float MusicCrossFade { get { return _musicCrossFade; } set { _musicCrossFade = Mathf.Clamp01(value); } }

        [SerializeField]
        private float _updateCrossfadeUpdateMs = 100.0f;
#endregion

#region Volume
        public float MasterVolume
        {
            get { return PartyParrotManager.Instance.GetFloat(MasterVolumeKey, 1.0f); }

            set
            {
                value = Mathf.Clamp01(value);

                Mixer.SetFloat(_masterVolumeParameter, value);
                PartyParrotManager.Instance.SetFloat(MasterVolumeKey, value);

                Mute = false;
            }
        }

        public float MusicVolume
        {
            get { return PartyParrotManager.Instance.GetFloat(MusicVolumeKey, 0.5f); }

            set
            {
                value = Mathf.Clamp01(value);

                Mixer.SetFloat(_musicVolumeParameter, value);
                PartyParrotManager.Instance.SetFloat(MusicVolumeKey, value);

                Mute = false;
            }
        }

        public float SFXVolume
        {
            get { return PartyParrotManager.Instance.GetFloat(SFXVolumeKey, 1.0f); }

            set
            {
                value = Mathf.Clamp01(value);

                Mixer.SetFloat(_sfxVolumeParameter, value);
                PartyParrotManager.Instance.SetFloat(SFXVolumeKey, value);

                Mute = false;
            }
        }

        [SerializeField]
        [ReadOnly]
        private bool _mute;

        public bool Mute
        {
            get { return _mute; }

            set
            {
                _mute = value;
                Mixer.SetFloat(_masterVolumeParameter, _mute ? 0.0f : MasterVolume);
            }
        }
#endregion

#region Unity Lifecycle
        private void Awake()
        {
            InitSFXAudioMixerGroup(_oneShotAudioSource);

            InitAudioMixerGroup(_music1AudioSource, _musicMixerGroupName);
            _music1AudioSource.loop = true;

            InitAudioMixerGroup(_music2AudioSource, _musicMixerGroupName);
            _music2AudioSource.loop = true;

            // this ensures we've loaded the volumes from the config
            MasterVolume = MasterVolume;
            MusicVolume = MusicVolume;
            SFXVolume = SFXVolume;

            InitDebugMenu();
        }

        private void Start()
        {
            StartCoroutine(UpdateMusicCrossfade());
        }
#endregion

        public void InitSFXAudioMixerGroup(AudioSource source)
        {
            InitAudioMixerGroup(source, _sfxMixerGroupName);
        }

        private void InitAudioMixerGroup(AudioSource source, string mixerGroupName)
        {
            var mixerGroups = _mixer.FindMatchingGroups(mixerGroupName);
            source.outputAudioMixerGroup = mixerGroups.Length > 0 ? mixerGroups[0] : _mixer.outputAudioMixerGroup;
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            _oneShotAudioSource.PlayOneShot(audioClip);
        }

        public void PlayMusic(AudioClip musicAudioClip)
        {
            StopMusic();

            _music1AudioSource.clip = musicAudioClip;
            _music1AudioSource.Play();
        }

        public void PlayMusic(AudioClip music1AudioClip, AudioClip music2AudioClip)
        {
            StopMusic();

            _music1AudioSource.clip = music1AudioClip;
            _music1AudioSource.Play();

            _music2AudioSource.clip = music2AudioClip;
            _music2AudioSource.Play();
        }

        public void StopMusic()
        {
            _music1AudioSource.Stop();
            _music2AudioSource.Stop();
        }

        private IEnumerator UpdateMusicCrossfade()
        {
            WaitForSeconds wait = new WaitForSeconds(_updateCrossfadeUpdateMs / 1000.0f);
            while(true) {
                _music1AudioSource.volume = 1.0f - _musicCrossFade;
                _music2AudioSource.volume = _musicCrossFade;

                yield return wait;
            }
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "AudioManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("Volume", GUI.skin.box);
                    GUILayout.Label($"Master Volume: {MasterVolume}");
                    GUILayout.Label($"Music Volume: {MusicVolume}");
                    GUILayout.Label($"SFX Volume: {SFXVolume}");
                    GUILayout.Label($"Mute: {Mute}");
                GUILayout.EndVertical();

                GUILayout.BeginVertical("SFX", GUI.skin.box);
                    GUILayout.Label("TODO");
                GUILayout.EndVertical();

                GUILayout.BeginVertical("Music", GUI.skin.box);
                    GUILayout.Label($"Music Crossfade: {MusicCrossFade}");
                GUILayout.EndVertical();
            };
        }
    }
}
