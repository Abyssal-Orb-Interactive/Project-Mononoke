using Base.Grid;
using Base.Input;
using Base.Input.Actions;
using Base.TileMap;
using Source.BuildingModule;
using Source.BuildingModule.Buildings.UI;
using Source.Character;
using Source.Character.Movement;
using Source.InventoryModule;
using Source.InventoryModule.UI;
using Source.PickUpModule;
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
        [SerializeField] private ItemChooseMenu _itemChooseMenu = null;
        [SerializeField] private InventoryTableView _view = null;

        protected override void Configure(IContainerBuilder builder)
        {
            // Register Unity Tilemap
            builder.RegisterInstance(_tileMap);
            builder.RegisterInstance(_objectPlacer);
            builder.RegisterInstance(_objectContainersAssociator);
            builder.RegisterInstance(_buildingsDatabase);
            builder.RegisterInstance(_characterMover);
            builder.RegisterInstance(_itemChooseMenu);
            builder.RegisterInstance(_view);


            // Registering Components
            builder.Register<TestActions>(Lifetime.Singleton);
            builder.Register<TestActionsWrapper>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputHandler>(Lifetime.Singleton);
            // var tileMapWrapper = new UnityTileMapWrapper(_tileMap);
            builder.Register<UnityTileMapWrapper>(Lifetime.Singleton).AsImplementedInterfaces();
            //var tileCollectionAnalyzer = new TileCollectionAnalyzer(tileMapWrapper);
            builder.Register<TileCollectionAnalyzer>(Lifetime.Singleton).AsImplementedInterfaces();
            //var grid = new GroundGrid(tileCollectionAnalyzer);
            builder.Register<GroundGrid>(Lifetime.Singleton);
            builder.Register(_ => new Manipulator(5, 5), Lifetime.Singleton);
            builder.Register(_ => new Inventory(100,100), Lifetime.Singleton);
            builder.Register<InventoryPresenter>(Lifetime.Singleton);
            //var gridBuilder = new OnGridBuilder(_objectPlacer, grid , _buildingsDatabase, _objectContainersAssociator);
            builder.Register<OnGridBuilder>(Lifetime.Singleton);
           // var gridAnalyzer = new GridAnalyzer(_characterMover, grid);
            builder.Register<GridAnalyzer>(Lifetime.Singleton);
        }
    }
}

