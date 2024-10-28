using Cysharp.Threading.Tasks;

using SpaceAce.Main.GamePause;
using SpaceAce.Main.Saving;

namespace SpaceAce.Main.Audio
{
    public sealed class ContinuousMusicPlayer : MusicPlayer
    {
        public ContinuousMusicPlayer(AudioCollection music,
                                     AudioPlayer audioPlayer,
                                     SavingSystem savingSystem,
                                     GamePauser gamePauser) : base(music,
                                                                   audioPlayer,
                                                                   savingSystem,
                                                                   gamePauser)
        { }

        public override void Initialize()
        {
            base.Initialize();

            MusicCancellation = new();
            PlayMusicForeverAsync(MusicCancellation.Token).Forget();
        }

        public override void Dispose()
        {
            base.Dispose();

            MusicCancellation.Cancel();
            MusicCancellation.Dispose();
        }
    }
}