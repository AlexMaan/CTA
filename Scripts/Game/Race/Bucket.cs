using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    [SerializeField] Transform bar;
    [SerializeField] SpriteRenderer[] spriteRenderer;
    [SerializeField] ReactableAnimator animator;
    [Range(1, 100)]
    [SerializeField] int fillMax;
    int fillAmount;
    int id;
    Color color;
    bool isFull;

    public int Id => id;
    public Color Color => color;

    public void SetCapacity(int capacity) => fillMax = capacity;

    public void Init(int id, Color color)
    {
        this.id = id;
        this.color = color;
        foreach (var sr in spriteRenderer) 
            sr.color = new Color(color.r, color.g, color.b, sr.color.a);
        Clear();
    }

    public bool Fill(int count)
    {
        fillAmount += count;
        if (isFull) return true;
        if (fillAmount >= fillMax) isFull = true;
        fillAmount = Mathf.Clamp(fillAmount, 0, fillMax);
        StartCoroutine(Filling((float)fillAmount / fillMax));

        if (fillAmount != fillMax) return false;
        else
        {
            animator.Act(1);
            StartCoroutine(Shifting());
            return true;
        }
    }

    IEnumerator Filling(float amount)
    {
        yield return new WaitForSeconds(0.8f);
        float time = 0;
        float current = bar.localScale.x;
        while (time < 1)
        {
            time += Time.deltaTime;
            bar.localScale = new Vector3(Mathf.Lerp(current, amount, time / 1), 1, 1);
            yield return null;
        }
    }

    public void Clear()
    {
        isFull = false;
        fillAmount = 0;
        bar.localScale = new Vector3(0, 1, 1);
        animator.Act(0);
        StopAllCoroutines();
    }

    IEnumerator Shifting()
    {
        yield return new WaitForSeconds(3);
        transform.position += Vector3.left * 10;
        transform.SetParent(null);
    }
}
