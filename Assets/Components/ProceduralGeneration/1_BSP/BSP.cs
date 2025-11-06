using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VTools.RandomService;

[CreateAssetMenu(menuName = "Procedural Generation Method/BSP")]

public class BSP : ProceduralGenerationMethod
{
    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        Debug.Log("BSP");
        var allGrid = new RectInt(xMin:0, yMin:0, Grid.Width, height: Grid.Lenght);
        var root = new BSPNode(allGrid, RandomService);
    }
}


public class BSPNode
{
    private RectInt _bounds;
    private RectInt? _room;
    private RandomService _randomService;
    private BSPNode _child1, _child2;
    private Vector2Int _roomSize = new (5,5);



    public BSPNode(RectInt bounds, RandomService randomService)
    {
        _bounds = bounds;
        _randomService = randomService;

        RectInt splitBoundLeft = new RectInt(_bounds.xMin, bounds.yMin, width:_bounds.width/2, _bounds.height);
        RectInt splitBoundRight = new RectInt(xMin:_bounds.xMin +_bounds.width/2, yMin:bounds.yMin, width: _bounds.width, _bounds.height);

        RectInt splitBoundDown = new RectInt(_bounds.xMin, bounds.yMin, width: _bounds.width , _bounds.height/ 2);
        RectInt splitBoundUp = new RectInt(xMin: _bounds.xMin, yMin: bounds.yMin + bounds.yMin / 2, width: _bounds.width, _bounds.height / 2);

        if (splitBoundLeft.width < _roomSize.x || splitBoundLeft.height < _roomSize.y)
        {
            CreateRooms();
            return;
        }

        if (splitBoundRight.width < _roomSize.x || splitBoundRight.height < _roomSize.y)
        {

            return;
        }


        _child1 = new BSPNode(_bounds, _randomService);
        _child2 = new BSPNode(_bounds, _randomService);
    }


    public void CreateRooms()
    {
        if (_child1 == null && _child2 == null)
        {
            int roomWidth = _randomService.Range(_roomSize.x, _bounds.width); 
            int roomHeight = _randomService.Range(_roomSize.y, _bounds.height);
            int roomX = _bounds.xMin + _randomService.Range(0, _bounds.width - roomWidth); 
            int roomY = _bounds.yMin + _randomService.Range(0, _bounds.height - roomHeight);
            _room = new RectInt(roomX, roomY, roomWidth, roomHeight);
        }
        else 
        { 
            _child1?.CreateRooms(); 
            _child2?.CreateRooms(); 
        }
    }

}
