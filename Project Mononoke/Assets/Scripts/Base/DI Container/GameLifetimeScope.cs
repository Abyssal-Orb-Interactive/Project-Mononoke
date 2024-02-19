using Base.Grid;
using Base.Input;
using Base.TileMap;
using Source.BuildingModule;
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
        [SerializeField] private IsoCharacterMover _characterMover = null;

        protected override void Configure(IContainerBuilder builder)
        {
            // Register Unity Tilemap
            builder.RegisterInstance(_tileMap);
            builder.RegisterInstance(_objectPlacer);
            builder.RegisterInstance(_objectContainersAssociator);
            builder.RegisterInstance(_buildingsDatabase);
            builder.RegisterInstance(_characterMover);
        
            // Registering Components
            builder.Register<UnityTileMapWrapper>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TileCollectionAnalyzer>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GroundGrid>(Lifetime.Singleton);
            builder.Register<TestActions>(Lifetime.Singleton);
            builder.Register<InputHandler>(Lifetime.Singleton);
            var tileMapWrapper = new UnityTileMapWrapper(_tileMap);
            var tileCollectionAnalyzer = new TileCollectionAnalyzer(tileMapWrapper);
            var gridBuilder = new OnGridBuilder(_objectPlacer, new GroundGrid(tileCollectionAnalyzer), _buildingsDatabase, _objectContainersAssociator);
            builder.Register<OnGridBuilder>(Lifetime.Singleton);
        }
    }
}

