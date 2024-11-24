using Newtonsoft.Json;

using SpaceAce.Auxiliary.EventArguments;
using SpaceAce.Main.Saving;

using System;

using Zenject;

namespace SpaceAce.Gameplay.Levels
{
    public sealed class LevelUnlocker : IInitializable, IDisposable, ISavable
    {
        public event EventHandler<LevelEventArgs> LevelUnlocked;
        public event EventHandler SavingRequested;
        public event EventHandler<ErrorOccurredEventArgs> ErrorOccurred;

        private readonly SavingSystem _savingSystem;
        private readonly LevelCompleter _levelCompleter;

        public int LastCompletedLevel { get; private set; } = 0;
        public int LastUnlockedLevel => LastCompletedLevel + 1;

        public string SavedDataName => "Unlocked levels";

        public LevelUnlocker(SavingSystem savingSystem, LevelCompleter levelCompleter, int lastUnlockedLevel)
        {
            _savingSystem = savingSystem ?? throw new ArgumentNullException();
            _levelCompleter = levelCompleter ?? throw new ArgumentNullException();
            LastCompletedLevel = lastUnlockedLevel <= 0 ? throw new ArgumentOutOfRangeException()
                                                        : lastUnlockedLevel - 1;
        }

        #region interfaces

        public void Initialize()
        {
            _savingSystem.Register(this);
            _levelCompleter.LevelCompleted += LevelCompletedEventHandler;
        }

        public void Dispose()
        {
            _savingSystem.Deregister(this);
            _levelCompleter.LevelCompleted -= LevelCompletedEventHandler;
        }

        public string GetState() =>
            JsonConvert.SerializeObject(LastCompletedLevel, Formatting.Indented);

        public void SetState(string state)
        {
            try
            {
                int level = JsonConvert.DeserializeObject<int>(state);
                LastCompletedLevel = level > LastCompletedLevel ? level : 0;
            }
            catch (Exception ex)
            {
                LastCompletedLevel = 0;
                ErrorOccurred?.Invoke(this, new(ex));
            }
        }

        public override bool Equals(object obj) =>
            obj is not null && Equals(obj as ISavable);

        public bool Equals(ISavable other) =>
            other is not null && SavedDataName == other.SavedDataName;

        public override int GetHashCode() =>
            SavedDataName.GetHashCode();

        #endregion

        private void LevelCompletedEventHandler(object sender, LevelEventArgs e)
        {
            LastCompletedLevel = e.Level;
            LevelUnlocked?.Invoke(this, e);
        }
    }
}