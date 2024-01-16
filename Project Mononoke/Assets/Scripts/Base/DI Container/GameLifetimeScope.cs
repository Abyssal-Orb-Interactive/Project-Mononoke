using Base.Grid;
using Base.Input;
using Base.TileMap;
using Source.BuildingSystem;
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
        [SerializeField] private OnGridObjectPlacer _objectPlacer = null;
        [SerializeField] private ObjectContainersAssociator _objectContainersAssociator = null;
        [SerializeField] private BuildingsDatabaseSo _buildingsDatabase = null;

        protected override void Configure(IContainerBuilder builder)
        {
            // Register Unity Tilemap
            builder.RegisterInstance(_tileMap);
            builder.RegisterInstance(_objectPlacer);
            builder.RegisterInstance(_objectContainersAssociator);
            builder.RegisterInstance(_buildingsDatabase);
        
            // Registering Components
            builder.Register<UnityTileMapWrapper>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TileCollectionAnalyzer>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GroundGrid>(Lifetime.Singleton);
            builder.Register<TestActions>(Lifetime.Singleton);
            builder.Register<InputHandler>(Lifetime.Singleton);
            builder.Register<OnGridBuilder>(Lifetime.Singleton);
        }
    }
}

