using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;

[CreateAssetMenu(menuName = "Procedural Generation Method/Cellular Automata")]
public class CellularAutomata : ProceduralGenerationMethod
{
    [Header("Cellular Automata Settings")]
    [SerializeField] private int roomDensity;

    private bool[,] map;
    private List<Cell> AllCells;

    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        AllCells = new List<Cell>();

        BuildGround();

        InitializeMap();
        DisplayMap(map);
        await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);

        for (int step = 0; step < _maxSteps; step++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            map = SimulateStep(map);
            DisplayMap(map);

            await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
        }

    }

    private void BuildGround()
    {
        var groundTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");

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

    private void InitializeMap()
    {
        map = new bool[Grid.Width, Grid.Lenght];
        var rand = new System.Random();

        for (int x = 0; x < Grid.Width; x++)
        {
            for (int z = 0; z < Grid.Lenght; z++)
            {
                map[x, z] = rand.Next(0, 100) < roomDensity;
            }
        }
    }

    private bool[,] SimulateStep(bool[,] oldMap)
    {
        bool[,] newMap = new bool[Grid.Width, Grid.Lenght];

        for (int x = 0; x < Grid.Width; x++)
        {
            for (int z = 0; z < Grid.Lenght; z++)
            {
                int waterNeighbors = CountWaterNeighbors(oldMap, x, z);

                if (oldMap[x, z])
                    newMap[x, z] = waterNeighbors >= 4;
                else
                    newMap[x, z] = waterNeighbors >= 5;
            }
        }

        return newMap;
    }

    private int CountWaterNeighbors(bool[,] map, int x, int z)
    {
        int count = 0;
        for (int nx = x - 1; nx <= x + 1; nx++)
        {
            for (int nz = z - 1; nz <= z + 1; nz++)
            {
                if (nx == x && nz == z) continue;

                if (nx < 0 || nz < 0 || nx >= Grid.Width || nz >= Grid.Lenght)
                {
                    count++;
                }
                else if (map[nx, nz])
                {
                    count++;
                }
            }
        }
        return count;
    }

    private void DisplayMap(bool[,] map)
    {
        var groundTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");
        var waterTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Water");

        for (int x = 0; x < Grid.Width; x++)
        {
            for (int z = 0; z < Grid.Lenght; z++)
            {
                if (!Grid.TryGetCellByCoordinates(x, z, out var cell))
                    continue;

                var template = map[x, z] ? waterTemplate : groundTemplate;
                GridGenerator.AddGridObjectToCell(cell, template, false);
            }
        }
    }
}
