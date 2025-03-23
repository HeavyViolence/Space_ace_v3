using System.Collections.Generic;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SpaceAce.Main.DI
{
    public sealed class GameServicesLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private List<GameObject> _roots;

        protected override void Configure(IContainerBuilder builder)
        {
            foreach (GameObject root in _roots)
            {
                foreach (ServiceInstaller installer in root.GetComponentsInChildren<ServiceInstaller>())
                {
                    if (installer.gameObject.activeInHierarchy == true)
                    {
                        installer.Install(builder);
                    }
                }

                foreach (GameObject obj in autoInjectGameObjects)
                {
                    foreach (MonoBehaviour behaviour in obj.GetComponentsInChildren<MonoBehaviour>())
                    {
                        builder.RegisterComponent(behaviour);
                    }
                }
            }
        }
    }
}