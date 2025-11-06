using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.LightTransport;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;

[CreateAssetMenu(menuName = "Procedural Generation Method/Noise")]
public class NoiseGenerator : ProceduralGenerationMethod
{
    [Header("Noise Parameters")]
    [SerializeField] private FastNoiseLite.NoiseType _NoiseType;
    [SerializeField, Range(0, 1f)] private float _Frequency = 0.5f;
    [SerializeField, Range(0, 2f)] private float _Amplitude = 1f;

    [Header("Fractal Parameters")]
    [SerializeField, Range(0, 2f)] private float _Lacunarity = 1f;
    [SerializeField, Range(0, 2f)] private float _Persistance = 1f;
    [SerializeField, Range(0, 10)] private int _Octave = 3;


    [Header("Height")]
    [SerializeField, Range(-1f, 1f)] private float _WaterHeight;
    [SerializeField, Range(-1f, 1f)] private float _SandHeight;
    [SerializeField, Range(-1f, 1f)] private float _GrassHeight;
    [SerializeField, Range(-1f, 1f)] private float _RockHeight;

    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        FastNoiseLite noise = new FastNoiseLite();
        noise.SetNoiseType(_NoiseType);

        // Gather noise data
        float[,] noiseData = new float[Grid.Width, Grid.Lenght];
        noise.SetFrequency(_Frequency);
        noise.SetFractalGain(_Persistance);
        noise.SetFractalLacunarity(_Lacunarity);

        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Lenght; y++)
            {
                noiseData[x, y] = noise.GetNoise(x, y);
                float noiseHeight = noise.GetNoise(x, y);


                if (Grid.TryGetCellByCoordinates(x, y, out var cell))
                {
                     if (noiseHeight < _WaterHeight)
                    {
                        AddTileToCell(cell, WATER_TILE_NAME, false);
                    }
                    
                    else if (noiseHeight < _SandHeight)
                    {
                        AddTileToCell(cell, SAND_TILE_NAME, false);
                    }
                    else if (noiseHeight < _GrassHeight)
                    {
                        AddTileToCell(cell, GRASS_TILE_NAME, false);
                    }
                    else if (noiseHeight < _RockHeight)
                    {
                        AddTileToCell(cell, ROCK_TILE_NAME, false);
                    }
                }

            }
        }
    }
}
