using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;

namespace Components.ProceduralGeneration.SimpleRoomPlacement
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Simple Room Placement")]
    public class SimpleRoomPlacement : ProceduralGenerationMethod
    {
        [Header("Room Parameters")]
        [SerializeField] private int _maxRooms;
        [SerializeField] private int _RoomSpacing;
        
        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            // Declare variables here

            for (int i = 0; i < _maxRooms; i++)
            {
                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();

                int x = RandomService.Range(0, Grid.Width);
                int z = RandomService.Range(0, Grid.Lenght);

                RectInt room = new RectInt(x, yMin:z, width:10, height:10);
                if (CanPlaceRoom(room, _RoomSpacing))
                {
                    PlaceRoom(room);
                }
                // Waiting between steps to see the result.
                await UniTask.Delay(GridGenerator.StepDelay, cancellationToken : cancellationToken);
            }
            
            // Final ground building.
            BuildGround();
        }

        private void PlaceRoom(RectInt room)
        {
            for (int ix = room.xMin; ix < room.xMax; ix++)
            {
                for (int iy = room.yMin; iy < room.yMax; iy++)
                {
                    if (Grid.TryGetCellByCoordinates(ix, iy, out var cell))
                    {
                        AddTileToCell(cell, ROOM_TILE_NAME, true);
                    }
                }
            }
        }
        private void BuildGround()
        {
            var groundTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");
            
            // Instantiate ground blocks
            for (int x = 0; x < Grid.Width; x++)
            {
                for (int z = 0; z < Grid.Lenght; z++)
                {
                    if (!Grid.TryGetCellByCoordinates(x, z, out var chosenCell))
                    {
                        Debug.LogError($"Unable to get cell on coordinates : ({x}, {z})");
                        continue;
                    }
                    
                    GridGenerator.AddGridObjectToCell(chosenCell, groundTemplate, false);
                }
            }
        }

        private void BuildPath()
        {

        }
    }
}