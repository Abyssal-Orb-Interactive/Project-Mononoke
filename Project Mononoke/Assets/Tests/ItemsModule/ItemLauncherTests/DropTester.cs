using System.Collections.Generic;
using Base.Grid;
using Base.Input;
using Base.TileMap;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Source.BuildingModule;
using Source.Character.Movement;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;

namespace Tests.ItemsModule.ItemLauncherTests
{
    public class DropTester
    {
        [Test]
        public void WhenItemLauncherHasUnpassableCellsAroundAndSomePassableCells_AndDropItem_ThenEndDropPositionShouldBeInPassableCells()
        {
            // Arrange.
            var groundCellType = CellType.Grass;
            var waterCellType = CellType.Water;
            var startCellPosition = Vector3.zero;
            var groundCellsPositions = new List<Vector3>(){DirectionToVector3IsoConverter.ToVector(MovementDirection.NorthEast), DirectionToVector3IsoConverter.ToVector(MovementDirection.NorthWest), DirectionToVector3IsoConverter.ToVector(MovementDirection.North)};
            var tileMapAnalyzerSubstitute = Substitute.For<ICellTypeSource>();
            tileMapAnalyzerSubstitute.GetCellTypeAt(Arg.Any<Vector3>()).Returns(
                cellPos => groundCellsPositions.Contains((Vector3)cellPos[0]) ? groundCellType : waterCellType
            );
            var grid = new GroundGrid(tileMapAnalyzerSubstitute);
            var go = new GameObject("Dropper")
            {
                transform =
                {
                    position = startCellPosition
                }
            };
            var itemLauncher = go.AddComponent<ItemLauncher>();
            var onGridObjectPlacer = go.AddComponent<OnGridObjectPlacer>();
            itemLauncher.Initialize(grid);
            var itemDataSubstitute = Substitute.For<IItemData>();
            var item = new Item(itemDataSubstitute);
            var itemView = new GameObject().AddComponent<ItemView>();
            var animPlayer = itemView.gameObject.AddComponent<ParabolicMotionAnimationPlayer>();
            animPlayer.Initialize();
            ItemViewFabric.Initialize(itemView, go.transform, onGridObjectPlacer);
            // Act.
            var endingDropPosition = itemLauncher.DropAndGetEndingDropPosition(item);
            var result = groundCellsPositions.Contains(endingDropPosition);
            // Assert.
            var ri = Random.Range(0, groundCellsPositions.Capacity - 1);
            new { endingDropPositionIsInGroundCellPositions = InputVectorToDirectionConverter.GetMovementDirectionFor(endingDropPosition) }.Should().BeEquivalentTo(groundCellsPositions[ri]);

        }
    }
}