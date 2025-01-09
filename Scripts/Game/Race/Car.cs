using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] AnimationCurve turnCurve;
    [SerializeField] float turnTime;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color[] tints;
    TrafficGenerator traffic;
    WaitForSeconds waitBlock = new WaitForSeconds(0.1f);
    WaitForSeconds waitChange = new WaitForSeconds(0.5f);
    Transform car, truck;
    int lane;
    float blockDist;

    public int Lane => lane;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;

    public void Init(TrafficGenerator traffic, int lane)
    {
        this.traffic = traffic;
        ChangeLane(lane);
        car = traffic.Race.Car.GetChild(0);
        truck = traffic.Race.Truck;
        blockDist = traffic.BlockDist + Random.Range(0, blockDist);
        spriteRenderer.color = tints[Random.Range(0, tints.Length)];
        StopAllCoroutines();
        if (lane == 1)
            StartCoroutine(WatchingDistance());
        else
            StartCoroutine(ChangingLane());
    }

    IEnumerator Turn(int dir)
    {
        float time = 0;
        float startY = transform.position.y;
        float endY = startY + dir * traffic.lanewidth;
        float currTurn = turnTime + Random.Range(-turnTime / 2, turnTime / 2);
        while (time < turnTime)
        {
            float y = Mathf.Lerp(startY, endY, turnCurve.Evaluate(time / currTurn));
            transform.position = new Vector2(transform.position.x, y);
            time += Time.deltaTime;
            yield return null;
        }
        if (lane == 1)
            StartCoroutine(WatchingDistance());
    }

    IEnumerator WatchingDistance()
    {
        yield return waitBlock;
        while (Mathf.Abs(transform.position.x - car.position.x) > blockDist
            && Mathf.Abs(transform.position.x - truck.position.x) > blockDist)
        { yield return waitBlock; }

        switch (Random.Range(0, 2))
        {
            case 0:
                StartCoroutine(Turn(1));
                ChangeLane(0);
                break;
            case 1:
                StartCoroutine(Turn(-1));
                ChangeLane(2);
                break;
        }
    }

    IEnumerator ChangingLane()
    {
        var dir = lane == 0 ? -1 : 1;
        while (true)
        {
            yield return waitChange;
            if (Random.value > 0.01f) continue;
            StartCoroutine(Turn(dir));
            ChangeLane(1);
            break;
        }
    }
    void ChangeLane(int id)
    {
        lane = id;
        spriteRenderer.sortingOrder = id + 1;
    }


}
