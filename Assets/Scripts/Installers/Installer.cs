using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Installer : MonoInstaller
{
    [SerializeField] GameWorld gameWorld;
    [SerializeField] ItemFactory itemFactory;
    [SerializeField] Database database;
    [SerializeField] ParticlesFactory particlesFactory;
    [SerializeField] UIFactory uIFactory;
    [SerializeField] GameData gameData;
    [SerializeField] SaveLoad saveLoad;

    public override void InstallBindings()
    {
        Container.Bind<GameWorld>().FromInstance(gameWorld).AsSingle();
        Container.QueueForInject(gameWorld);

        Container.Bind<ItemFactory>().FromInstance(itemFactory).AsSingle();
        Container.QueueForInject(itemFactory);

        Container.Bind<Database>().FromInstance(database).AsSingle();
        Container.QueueForInject(database);

        Container.Bind<ParticlesFactory>().FromInstance(particlesFactory).AsSingle();
        Container.QueueForInject(particlesFactory);

        Container.Bind<UIFactory>().FromInstance(uIFactory).AsSingle();
        Container.QueueForInject(uIFactory);

        Container.Bind<GameData>().FromInstance(gameData).AsSingle();
        Container.QueueForInject(gameData);

        Container.Bind<SaveLoad>().FromInstance(saveLoad).AsSingle();
        Container.QueueForInject(saveLoad);
        //Container.Bind<GameWorld>().FromNew().AsSingle();
    }
}
