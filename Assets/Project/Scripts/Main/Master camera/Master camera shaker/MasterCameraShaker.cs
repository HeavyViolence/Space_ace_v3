using MessagePack;

using SpaceAce.Auxiliary;
using SpaceAce.Main.GamePause;
using SpaceAce.Main.Saving;

using System;
using System.Collections.Generic;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SpaceAce.Main.MasterCamera
{
    public sealed class MasterCameraShaker : IInitializable, IDisposable, ISavable, IFixedTickable
    {
        public event Action StateChanged;

        private readonly HashSet<ShakeRequest> _shakeRequests = new();
        private readonly HashSet<ShakeRequest> _shakeRequestsToBeDeleted = new();

        private readonly Rigidbody2D _masterCameraBody;

        private readonly GamePauser _gamePauser;
        private readonly SavingSystem _savingSystem;

        private readonly AnimationCurve _shakeCurve;
        private readonly Vector2 _homePosition;

        private MasterCameraShakerSettings _settings = MasterCameraShakerSettings.Default;

        public MasterCameraShakerSettings Settings
        {
            get => _settings;

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                _settings = value;
                StateChanged?.Invoke();
            }
        }

        public string StateName => "Camera shake settings";

        [Inject]
        public MasterCameraShaker(MasterCameraShakerConfig config,
                                  MasterCameraHolder masterCameraHolder,
                                  GamePauser gamePauser,
                                  SavingSystem savingSystem)
        {
            if (config == null)
            {
                throw new ArgumentNullException();
            }

            _settings = config.Settings;
            _shakeCurve = config.ShakeCurve;

            Rigidbody2D body = masterCameraHolder is null ? throw new ArgumentNullException()
                                                          : masterCameraHolder.MasterCameraAnchor.gameObject.GetComponentInChildren<Rigidbody2D>();

            if (body == null)
            {
                throw new MissingComponentException($"Master camera prefab is missing {typeof(Rigidbody2D)}!");
            }

            _masterCameraBody = body;
            _homePosition = _masterCameraBody.position;

            _gamePauser = gamePauser ?? throw new ArgumentNullException();
            _savingSystem = savingSystem ?? throw new ArgumentNullException();
        }

        public void ShakeOnShotFired() => _shakeRequests.Add(_settings.OnShotFired.NewShakeRequest);
        public void ShakeOnDefeat() => _shakeRequests.Add(_settings.OnDefeat.NewShakeRequest);
        public void ShakeOnCollision() => _shakeRequests.Add(_settings.OnCollision.NewShakeRequest);
        public void ShakeOnHit() => _shakeRequests.Add(_settings.OnHit.NewShakeRequest);

        private void ShakeMasterCamera()
        {
            if (_shakeRequests.Count == 0)
            {
                return;
            }

            float totalAmplitude = 0f;
            float averageAmplitude;

            float totalFrequency = 0f;
            float averageFrequency;

            int counter = 0;

            foreach (ShakeRequest request in _shakeRequests)
            {
                totalAmplitude += request.CurrentAmplitude;
                totalFrequency += request.CurrentFrequency;
                counter++;
            }

            averageAmplitude = totalAmplitude / counter;
            averageFrequency = totalFrequency / counter;

            float delta = averageAmplitude * Mathf.Sin(2f * Mathf.PI * averageFrequency);
            Vector2 position = new(MyMath.RandomUnit * delta, MyMath.RandomUnit * delta);

            _masterCameraBody.MovePosition(position);

            foreach (ShakeRequest request in _shakeRequests)
            {
                request.FixedUpdate(_shakeCurve);

                if (request.NormalizedDuration > 1f)
                {
                    _shakeRequestsToBeDeleted.Add(request);
                }
            }

            if (_shakeRequestsToBeDeleted.Count > 0)
            {
                foreach (ShakeRequest request in _shakeRequestsToBeDeleted)
                {
                    _shakeRequests.Remove(request);
                }

                _shakeRequestsToBeDeleted.Clear();
            }

            if (_shakeRequests.Count == 0)
            {
                _masterCameraBody.position = _homePosition;
            }
        }

        #region interfaces

        public void Initialize()
        {
            _savingSystem.Register(this);
        }

        public void Dispose()
        {
            _savingSystem.Deregister(this);
        }

        public byte[] GetState() => MessagePackSerializer.Serialize(Settings);

        public void SetState(byte[] state)
        {
            try
            {
                _settings = MessagePackSerializer.Deserialize<MasterCameraShakerSettings>(state);
            }
            catch (Exception)
            {
                _settings = MasterCameraShakerSettings.Default;
            }
        }

        public void FixedTick()
        {
            if (_gamePauser.Paused == true)
            {
                return;
            }

            ShakeMasterCamera();
        }

        #endregion
    }
}