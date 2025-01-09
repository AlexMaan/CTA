using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [Range(0, 1)]
    [SerializeField] float probability;

     void OnEnable()
    {
        if(spriteRenderer == null) return;
        bool isSpawn = Random.value < probability;
        spriteRenderer.enabled = isSpawn;
    }
}
