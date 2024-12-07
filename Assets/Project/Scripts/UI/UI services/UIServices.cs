using SpaceAce.Auxiliary.Easing;
using SpaceAce.Gameplay.Levels;
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
        public LevelUnlocker LevelUnlocker { get; }
        public BestLevelRunStatisticsCollector BestLevelRunStatisticsCollector { get; }
        public LevelRewardCollector LevelRewardCollector { get; }
        public EasingService EasingService { get; }

        public UIServices(Localizer localizer,
                          GameControls gameControls,
                          AudioPlayer audioPlayer,
                          UIAudio uiAudio,
                          LevelUnlocker levelUnlocker,
                          BestLevelRunStatisticsCollector bestLevelRunStatisticsCollector,
                          LevelRewardCollector levelRewardCollector,
                          EasingService easingService)
        {
            Localizer = localizer ?? throw new ArgumentNullException();
            GameControls = gameControls ?? throw new ArgumentNullException();
            AudioPlayer = audioPlayer ?? throw new ArgumentNullException();
            UIAudio = uiAudio == null ? throw new ArgumentNullException() : uiAudio;
            LevelUnlocker = levelUnlocker ?? throw new ArgumentNullException();
            BestLevelRunStatisticsCollector = bestLevelRunStatisticsCollector ?? throw new ArgumentNullException();
            LevelRewardCollector = levelRewardCollector ?? throw new ArgumentNullException();
            EasingService = easingService ?? throw new ArgumentNullException();
        }
    }
}