using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] Tile tileObj;
    [SerializeField] float tileSize;

    ItemsGenerator itemsGenerator;
    List<Tile> tiles = new();
    Queue<Tile> tilesPool = new();
    GameObject parent;
    Vector2 startPos, endPos;

    void Awake() => tileSize *= ResolutionResizer.ScaleK;

    public void Init(Race race, GameObject parent, ItemsGenerator itemsGenerator)
    {
        this.itemsGenerator = itemsGenerator;
        this.parent = parent;
        startPos = race.StartPoint.position;
        endPos = race.EndPoint.position;

        for (int i = 0; i < 5; i++) GenerateTile();
    }

    void Update()
    {
        if (tiles.Count == 0) return;
        var first = tiles.First();

        if (first.transform.position.x < endPos.x) PoolTile(first);
        if (tiles.Last().transform.position.x < startPos.x) GenerateTile();
    }

    public void GenerateTile()
    {
        Tile tile;
        Vector2 pos;
        if (tiles.Count > 0) pos = tiles.Last().transform.position;
        else pos = endPos;

        if (tilesPool.Count > 0)
        {
            tile = tilesPool.Dequeue();
            tile.transform.position = new Vector3(pos.x + tileSize, pos.y, 0);
        }
        else
        {
            tile = Instantiate(tileObj, new Vector3(pos.x + tileSize, pos.y, 0),
               Quaternion.identity, parent.transform);
        }
        tile.Init();
        tiles.Add(tile);
        itemsGenerator.FillTile(tile);
    }

    void PoolTile(Tile tile)
    {
        tilesPool.Enqueue(tile);
        itemsGenerator.ClearTile(tile);
        tile.gameObject.SetActive(false);
        tiles.RemoveAt(0);
    }
}
