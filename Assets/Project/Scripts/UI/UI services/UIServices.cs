using SpaceAce.Main.Audio;
using SpaceAce.Main.Controls;
using SpaceAce.Main.Localization;

using System;

namespace SpaceAce.UI
{
    public sealed record UIServices
    {
        public Localizer Localizer { get; }
        public GameControls GameControls { get; }
        public AudioPlayer AudioPlayer { get; }
        public UIAudio UIAudio { get; }

        public UIServices(Localizer localizer,
                          GameControls gameControls,
                          AudioPlayer audioPlayer,
                          UIAudio uiAudio)
        {
            Localizer = localizer ?? throw new ArgumentNullException();
            GameControls = gameControls ?? throw new ArgumentNullException();
            AudioPlayer = audioPlayer ?? throw new ArgumentNullException();
            UIAudio = uiAudio == null ? throw new ArgumentNullException() : uiAudio;
        }
    }
}