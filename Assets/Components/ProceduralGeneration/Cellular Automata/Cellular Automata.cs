using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;

[CreateAssetMenu(menuName = "Procedural Generation Method/Cellular Automata")]
public class CellularAutomata : ProceduralGenerationMethod
{
    [Header("Cellular Automata Settings")]
    [SerializeField, Range(0, 100)] private int _waterDensity = 45; 

    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        BuildGround();

        System.Random random = new System.Random();

        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Lenght; y++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (Grid.TryGetCellByCoordinates(x, y, out var cell))
                {
                    int chance = random.Next(0, 100);
                    if (chance < _waterDensity)
                    {
                        AddTileToCell(cell, WATER_TILE_NAME, true);
                    }
                }
            }
        }

        await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);

        for (int i = 0; i < _maxSteps; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            for (int x = 0; x < Grid.Width; x++)
            {
                for (int y = 0; y < Grid.Lenght; y++)
                {
                    if (Grid.TryGetCellByCoordinates(x, y, out var cell))
                    {
                        int waterNeighbors = CountWaterNeighbors(x, y);

                        // Exemple de règle simple : l’eau se propage ou disparaît selon ses voisines
                        if (cell.GridObject.Template.Name == WATER_TILE_NAME)
                        {
                            if (waterNeighbors < 3)
                                AddTileToCell(cell, GRASS_TILE_NAME, true);
                        }
                        else
                        {
                            if (waterNeighbors > 4)
                                AddTileToCell(cell, WATER_TILE_NAME, true);
                        }
                    }
                }
            }

            await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
        }

    }

    private void BuildGround()
    {
        var groundTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>(GRASS_TILE_NAME);

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

    private int CountWaterNeighbors(int x, int y)
    {
        int count = 0;

        for (int nx = x - 1; nx <= x + 1; nx++)
        {
            for (int ny = y - 1; ny <= y + 1; ny++)
            {
                if (nx == x && ny == y)
                    continue;

                if (Grid.TryGetCellByCoordinates(nx, ny, out var neighbor))
                {
                    if (neighbor.GridObject.Template.Name == WATER_TILE_NAME)
                        count++;
                }
                else
                {
                    count++;
                }
            }
        }

        return count;
    }
}
