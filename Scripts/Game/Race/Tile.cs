using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Queue<TileItem> items = new();

    public Queue<TileItem> Items => items;


    public void Init()
    {
        gameObject.SetActive(true);
    }

    public void PlaceItem(TileItem item) => items.Enqueue(item);

    public void ClearItems() => items.Clear();
}
