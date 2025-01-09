using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsGenerator : MonoBehaviour
{
    [SerializeField] TileItem itemObj;
    [SerializeField] int[] itemsCount;
    [SerializeField] float[] itemsChance;
    [SerializeField] Vector2 placePos;
    [SerializeField] float placeRange;
    [SerializeField] List<TileItemData> itemsData;
    [SerializeField] float bottomShift;

    List<TileItem> itemsPool = new();
    Tile currentTile;
    bool isTop;

    public List<TileItemData> Items => itemsData;

    void Awake() => placeRange *= ResolutionResizer.ScaleK;

    public void FillTile(Tile tile)
    {
        currentTile = tile;
        isTop = false;
        for (int i = 0; i < 2; i++)
        {
            var itter = CalculateCount();
            for (int j = 0; j < itter; j++) AddItem();
            isTop = true;
        }
    }

    public void ClearTile(Tile tile)
    {
        foreach (var item in tile.Items)
        {
            itemsPool.Add(item);
            item.gameObject.SetActive(false);
        }
        tile.ClearItems();
    }

    void AddItem()
    {
        if (itemsPool.Count > 0)
        {
            TileItem item = itemsPool.FirstOrDefault(i => i.Type == Terrain.Mix || i.Type == Race.Terrain);
            if (item != null)
            {
                itemsPool.Remove(item);
                PlaceItem(item);
                return;
            }
        }
        PlaceItem(GenItem());
    }

    TileItem GenItem()
    {
        TileItemData[] variants;
        variants = itemsData.Where(i => i.Type == Terrain.Mix || i.Type == Race.Terrain).ToArray();
        int index = Random.Range(0, variants.Length);
        TileItemData pick = variants[index];

        var item = Instantiate(itemObj, currentTile.transform);
        item.Init(pick.Type, pick.Sprite);

        return item;
    }

    void PlaceItem(TileItem item)
    {
        var posY = isTop ? placePos.y : -placePos.y - bottomShift;

        item.transform.SetParent(currentTile.transform);
        Vector2 localPos = new Vector2(Random.Range(placePos.x, -placePos.x),
            posY + Random.Range(-placeRange, +placeRange));
        item.transform.localPosition = localPos;

        item.gameObject.SetActive(true);
        currentTile.PlaceItem(item);
        item.SetSorting(isTop);
    }

    int CalculateCount()
    {
        float random = Random.value;
        for (int i = 0; i < itemsChance.Length; i++)
        {
            if (random < itemsChance[i]) return itemsCount[i];
            random -= itemsChance[i];
        }
        return 0;
    }
}

