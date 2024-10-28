using Cysharp.Threading.Tasks;

using SpaceAce.Main.GamePause;
using SpaceAce.Main.GameStates;
using SpaceAce.Main.Saving;

using System;

namespace SpaceAce.Main.Audio
{
    public sealed class LevelsOnlyMusicPlayer : MusicPlayer
    {
        private readonly GameStateLoader _gameStateLoader;

        public LevelsOnlyMusicPlayer(AudioCollection music,
                                     AudioPlayer audioPlayer,
                                     SavingSystem savingSystem,
                                     GamePauser gamePauser,
                                     GameStateLoader gameStateLoader) : base(music,
                                                                             audioPlayer,
                                                                             savingSystem,
                                                                             gamePauser)
        {
            _gameStateLoader = gameStateLoader ?? throw new ArgumentNullException();
        }

        public override void Initialize()
        {
            base.Initialize();

            _gameStateLoader.LevelLoaded += LevelLoadedEventHandler;
            _gameStateLoader.MainMenuLoaded += MainMenuLoadedEventHandler;
        }

        public override void Dispose()
        {
            base.Dispose();

            _gameStateLoader.LevelLoaded -= LevelLoadedEventHandler;
            _gameStateLoader.MainMenuLoaded -= MainMenuLoadedEventHandler;
        }

        private void LevelLoadedEventHandler(object sender, LevelLoadedEventArgs e)
        {
            MusicCancellation = new();
            PlayMusicForeverAsync(MusicCancellation.Token).Forget();
        }

        private void MainMenuLoadedEventHandler(object sender, EventArgs e)
        {
            MusicCancellation.Cancel();
            MusicCancellation.Dispose();
        }
    }
}