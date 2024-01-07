using Base.Grid;
using Base.Input;
using Base.TileMap;
using Source.Character;
using Source.Character.Movement;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

namespace Base.DIContainer
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private Tilemap _tileMap = null;

        protected override void Configure(IContainerBuilder builder)
        {
            // Register Unity Tilemap
            builder.RegisterInstance(_tileMap);
        
            // Registering Components
            builder.Register<UnityTileMapWrapper>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TileCollectionAnalyzer>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GroundGrid>(Lifetime.Singleton);
            builder.Register<GridAnalyzer>(Lifetime.Singleton);
            builder.Register<TestActions>(Lifetime.Singleton);
            builder.Register<InputHandler>(Lifetime.Singleton);
        }
    }
}

