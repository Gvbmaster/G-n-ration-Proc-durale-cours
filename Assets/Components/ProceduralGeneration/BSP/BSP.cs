using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VTools.RandomService;
[CreateAssetMenu(menuName = "Procedural Generation Method/BSP")]
public class BSP : ProceduralGenerationMethod
{
    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        Debug.Log("BSP Generation Started");
        // Création du rectangle total        var allGrid = new RectInt(0, 0, Grid.Width, Grid.Length);
        // Création du nœud racine        var root = new BSPNode(allGrid, RandomService);
        // Découpage récursif du BSP (4 niveaux par défaut)        root.SplitRecursively(4);
        // Création des salles        root.CreateRooms();
        // Visualisation (Debug.DrawLine)        root.DrawDebug();
        Debug.Log("BSP Generation Finished");
    }
}

public class BSPNode
{
    private RectInt _bounds; private RandomService _randomService; private BSPNode _child1, _child2; private RectInt _room;
    private const int MinRoomSize = 5;
    public BSPNode(RectInt bounds, RandomService randomService) { _bounds = bounds; _randomService = randomService; }
    // --- Divise récursivement le nœud en deux sous-nœuds ---    public void SplitRecursively(int depth)    {        if (depth <= 0) return;        if (_bounds.width < MinRoomSize * 2 || _bounds.height < MinRoomSize * 2) return;
    bool splitHorizontally = _randomService.NextBool();
        if (splitHorizontally)        {            int splitY = _randomService.NextInt(_bounds.yMin + MinRoomSize, _bounds.yMax - MinRoomSize); RectInt top = new RectInt(_bounds.xMin, splitY, _bounds.width, _bounds.yMax - splitY); RectInt bottom = new RectInt(_bounds.xMin, _bounds.yMin, _bounds.width, splitY - _bounds.yMin);
    _child1 = new BSPNode(top, _randomService); _child2 = new BSPNode(bottom, _randomService);
}    else
{
    int splitX = _randomService.NextInt(_bounds.xMin + MinRoomSize, _bounds.xMax - MinRoomSize); RectInt left = new RectInt(_bounds.xMin, _bounds.yMin, splitX - _bounds.xMin, _bounds.height); RectInt right = new RectInt(splitX, _bounds.yMin, _bounds.xMax - splitX, _bounds.height);
    _child1 = new BSPNode(left, _randomService); _child2 = new BSPNode(right, _randomService);
}
_child1.SplitRecursively(depth - 1); _child2.SplitRecursively(depth - 1);    }
    // --- Crée une salle dans chaque feuille ---    public void CreateRooms()    {        if (_child1 == null && _child2 == null)        {            int roomWidth = _randomService.NextInt(MinRoomSize, _bounds.width);            int roomHeight = _randomService.NextInt(MinRoomSize, _bounds.height);
            int roomX = _bounds.xMin + _randomService.NextInt(0, _bounds.width - roomWidth); int roomY = _bounds.yMin + _randomService.NextInt(0, _bounds.height - roomHeight);
_room = new RectInt(roomX, roomY, roomWidth, roomHeight);        }        else { _child1?.CreateRooms(); _child2?.CreateRooms(); }    }
    // --- Affiche les rectangles dans la scène ---    public void DrawDebug()    {        // Limites du nœud        Debug.DrawLine(new Vector3(_bounds.xMin, _bounds.yMin), new Vector3(_bounds.xMax, _bounds.yMin), Color.gray, 20);        Debug.DrawLine(new Vector3(_bounds.xMax, _bounds.yMin), new Vector3(_bounds.xMax, _bounds.yMax), Color.gray, 20);        Debug.DrawLine(new Vector3(_bounds.xMax, _bounds.yMax), new Vector3(_bounds.xMin, _bounds.yMax), Color.gray, 20);        Debug.DrawLine(new Vector3(_bounds.xMin, _bounds.yMax), new Vector3(_bounds.xMin, _bounds.yMin), Color.gray, 20);
        // Salle (en vert)        if (_room.width > 0 && _room.height > 0)        {            Debug.DrawLine(new Vector3(_room.xMin, _room.yMin), new Vector3(_room.xMax, _room.yMin), Color.green, 20);            Debug.DrawLine(new Vector3(_room.xMax, _room.yMin), new Vector3(_room.xMax, _room.yMax), Color.green, 20);            Debug.DrawLine(new Vector3(_room.xMax, _room.yMax), new Vector3(_room.xMin, _room.yMax), Color.green, 20);            Debug.DrawLine(new Vector3(_room.xMin, _room.yMax), new Vector3(_room.xMin, _room.yMin), Color.green, 20);        }
        _child1?.DrawDebug(); _child2?.DrawDebug();    }}