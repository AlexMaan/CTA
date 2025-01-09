using UnityEngine;

public class MovingTex : MonoBehaviour
{
    [SerializeField] Vector2 offsetSpeed;
    SpriteRenderer spriteRenderer;
    Material material;

    void Start()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    void Update()
    {
        material.mainTextureOffset += offsetSpeed * Time.deltaTime;
    }
}
