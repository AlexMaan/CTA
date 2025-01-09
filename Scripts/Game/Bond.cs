using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bond : MonoBehaviour
{
    [SerializeField] SpriteRenderer line;
    [SerializeField] GameObject end;
    [SerializeField] float maxDistance;
    public Ball Ball { get; private set; }


    public void Init(Ball ball)
    {
        gameObject.SetActive(true);
        transform.position = ball.transform.position;
        Ball = ball;
    }

    public void Hide()
    {
        end.gameObject.SetActive(true);
        gameObject.SetActive(false);
        line.size = new Vector2(0, line.size.y);
        end.transform.localPosition = new Vector2(0, end.transform.localPosition.y);
    }

    public void AlignBond(Vector2 pos, bool world = false)
    {
        if (pos == Vector2.zero) return;

        if(!world) pos = Camera.main.ScreenToWorldPoint(pos);
        Vector2 dir = pos - (Vector2)transform.position;
        transform.right = dir;

        float distance = Vector2.Distance(transform.position, pos);
        distance = Mathf.Min(distance, maxDistance);

        line.size = new Vector2(distance, line.size.y);
        end.transform.localPosition = new Vector2(distance, end.transform.localPosition.y);

    }

    public void StickBound(Ball ball)
    {
        AlignBond(ball.transform.position, true);
        end.gameObject.SetActive(false);
    }
}
