using UnityEngine;

using VContainer;

namespace SpaceAce.Main.DI
{
    public abstract class ServiceInstaller : MonoBehaviour
    {
        public abstract void Install(IContainerBuilder builder);
    }
}