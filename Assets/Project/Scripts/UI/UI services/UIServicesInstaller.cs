using SpaceAce.Main.Audio;
using SpaceAce.UI;

using UnityEngine;

using Zenject;

public sealed class UIServicesInstaller : MonoInstaller
{
    [SerializeField]
    private UIAudio _uiAudio;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<UIServices>()
                 .AsSingle()
                 .WithArguments(_uiAudio)
                 .NonLazy();
    }
}