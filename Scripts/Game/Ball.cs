using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] ReactableAnimator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float moveTime;
    [SerializeField] float animationDelay;
    [SerializeField] AnimationCurve moveCurve;
    
    Color color;
    int colorID, column, row;
    int animDelay => (int)(animationDelay * 1000);
    public int ColorID => colorID;
    public int Column => column;
    public int Row => row;

    public void SetCellID(int column, int row)
    {
        this.column = column;
        this.row = row;
    }

    public void SetColor((int, Color) color)
    {
        colorID = color.Item1;
        spriteRenderer.color = color.Item2;
        this.color = color.Item2;
    }

    public async void MoveTo(Vector2 target)
    {
        Vector2 start = transform.position;
        float time = 0;
        var dist = Mathf.Abs(target.y - start.y - 2);
        var speed = moveTime * dist;
        while (time < speed)
        {
            time += Time.deltaTime;
            transform.position = 
                new Vector2(start.x, start.y - MathF.Abs(start.y - target.y) 
                            * moveCurve.Evaluate(time / speed));
            await Task.Yield();
        }
        transform.position = target;
    }

    public void Select(bool enable, int count = -1)
    {
        animator.Act(enable ? 1 : 0);
        EffectsController.Instance.PlayEffect(1, transform.position, color);
        if(count != -1) audioSource.pitch = 0.5f + count * 0.1f;
        if(enable) audioSource.Play();
    }

    public async void Pop(Queue<Ball> pool)
    {
        animator.Act();
        var bucket = BucketsController.Instance.GetBucket(colorID);
        EffectsController.Instance.PlayEffect(0, transform.position, color);
        EffectsController.Instance.PlayEffect(2, transform.position, color, bucket.transform);
        await Task.Delay(animDelay);
        pool.Enqueue(this);
        gameObject.SetActive(false);
    }
}
