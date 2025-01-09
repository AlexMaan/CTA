using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TrafficGenerator : MonoBehaviour
{
    [SerializeField] Car carObj;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float speed, spawnDist, spawnDistRange, laneWidth, placeRange, blockDist;
    [SerializeField] Sprite[] carsSprites;

    Race race;
    List<Car> traffic = new();
    Queue<Car> trafficPool = new();

    public Race Race => race;
    public float lanewidth => laneWidth;
    public float BlockDist => blockDist;
    public List<Car> Traffic => traffic;

    void Awake()
    {
        spawnDist *= ResolutionResizer.ScaleK;
        spawnDistRange *= ResolutionResizer.ScaleK;
        laneWidth *= ResolutionResizer.ScaleK;
        placeRange *= ResolutionResizer.ScaleK;
        blockDist *= ResolutionResizer.ScaleK;
    }

    public void Init(Race race)
    {
        this.race = race;
        StartCoroutine(SpawnWaiting());
    }


    void Update()
    {
        if (!Race.IsActive) return;
        race.Traffic.transform.position += Vector3.left * speed * Time.deltaTime;
    }


    IEnumerator SpawnWaiting()
    {
        if (traffic.Count == 0) GenerateCar();

        float dist = spawnDist + Random.Range(-spawnDistRange, spawnDistRange);
        while (spawnPoint.position.x - traffic.Last().transform.position.x < dist)
        {
            yield return null;
        }
        if (Random.value > 0.1f) GenerateCar();
        else { GenerateCar(0); GenerateCar(2); }
        StartCoroutine(SpawnWaiting());

        var temp = new List<Car>(traffic);
        foreach (var car in temp)
        {
            if (car.transform.position.x < race.EndPoint.position.x)
            {
                trafficPool.Enqueue(car);
                traffic.Remove(car);
            }
        }
    }


    public void GenerateCar(int lane = -1)
    {
        lane = lane == -1 ? Random.Range(0, 3) : lane;
        Car car;
        if (trafficPool.Count > 0)
            car = trafficPool.Dequeue();
        else
        {
            car = Instantiate(carObj, race.Traffic.transform);
            car.SetSprite(carsSprites[Random.Range(0, carsSprites.Length)]);
        }
        PlaceCar(car, lane);
        traffic.Add(car);
        race.BucketsController.UpdateBuckets();
    }

    void PlaceCar(Car car, int lane)
    {
        car.Init(this, lane);
        var range = Random.Range(-placeRange, placeRange);
        var pos = spawnPoint.position;
        switch (lane)
        {
            case 0:
                car.transform.position = new Vector2(pos.x + range, pos.y + laneWidth + range);
                break;
            case 1:
                car.transform.position = new Vector2(pos.x + range, pos.y);
                break;
            case 2:
                car.transform.position = new Vector2(pos.x + range, pos.y - laneWidth + range);
                break;
            default:
                break;
        }
    }
}


