using SpaceAce.Main.Audio;
using SpaceAce.Main.Controls;
using SpaceAce.Main.GamePause;
using SpaceAce.Main.GameStates;
using SpaceAce.Main.Localization;
using SpaceAce.Main.MasterCamera;
using SpaceAce.Main.Saving;

using System;

using VContainer;

namespace SpaceAce.Main
{
    public sealed record MainServices
    {
        public AudioPlayer AudioPlayer { get; }
        public MusicPlayer MusicPlayer { get; }
        public GUIAudio GUIAudio { get; }
        public GameControls GameControls { get; }
        public GamePauser GamePauser { get; }
        public GameStateLoader GameStateLoader { get; }
        public Localizer Localizer { get; }
        public MasterCameraHolder MasterCameraHolder { get; }
        public MasterCameraShaker MasterCameraShaker { get; }
        public MasterAudioListenerHolder MasterAudioListenerHolder { get; }
        public SavingSystem SavingSystem { get; }

        [Inject]
        public MainServices(AudioPlayer audioPlayer,
                            MusicPlayer musicPlayer,
                            GUIAudio guiAudio,
                            GameControls gameControls,
                            GamePauser gamePauser,
                            GameStateLoader gameStateLoader,
                            Localizer localizer,
                            MasterCameraHolder masterCameraHolder,
                            MasterCameraShaker masterCameraShaker,
                            MasterAudioListenerHolder masterAudioListenerHolder,
                            SavingSystem savingSystem)
        {
            AudioPlayer = audioPlayer ?? throw new ArgumentNullException(nameof(audioPlayer));
            MusicPlayer = musicPlayer ?? throw new ArgumentNullException(nameof(musicPlayer));
            GUIAudio = guiAudio == null ? throw new ArgumentNullException(nameof(guiAudio)) : guiAudio;
            GameControls = gameControls ?? throw new ArgumentNullException(nameof(gameControls));
            GamePauser = gamePauser ?? throw new ArgumentNullException(nameof(gamePauser));
            GameStateLoader = gameStateLoader ?? throw new ArgumentNullException(nameof(gameStateLoader));
            Localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            MasterCameraHolder = masterCameraHolder ?? throw new ArgumentNullException(nameof(masterCameraHolder));
            MasterCameraShaker = masterCameraShaker ?? throw new ArgumentNullException(nameof(masterCameraShaker));
            MasterAudioListenerHolder = masterAudioListenerHolder ?? throw new ArgumentNullException(nameof(masterAudioListenerHolder));
            SavingSystem = savingSystem ?? throw new ArgumentNullException(nameof(savingSystem));
        }
    }
}