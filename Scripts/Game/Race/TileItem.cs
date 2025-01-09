using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class TileItemData
{
    [SerializeField] Terrain type;
    [SerializeField] Sprite sprite;

    public Terrain Type => type;
    public Sprite Sprite => sprite;
}

public class TileItem : MonoBehaviour
{
    [SerializeField] Terrain type;
    [SerializeField] SortingGroup sortingGroup;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float sizeVariation;

    public Terrain Type => type;

    public void Init(Terrain type, Sprite sprite)
    {
        this.type = type;
        spriteRenderer.sprite = sprite;
        transform.localScale = Vector3.one * Random.Range(1 - sizeVariation, 1 + sizeVariation);
    }

    public void SetSorting(bool top)
    {
        sortingGroup.sortingOrder = top ? 1 : 5;
    }
   
}
