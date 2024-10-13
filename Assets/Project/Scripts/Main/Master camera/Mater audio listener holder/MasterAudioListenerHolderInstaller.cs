using Zenject;

namespace SpaceAce.Main.MasterCamera
{
    public sealed class MasterAudioListenerHolderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MasterAudioListenerHolder>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}