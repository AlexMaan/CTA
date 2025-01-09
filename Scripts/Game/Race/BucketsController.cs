using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class BucketsController : MonoBehaviour
{
    [SerializeField] Bucket bucketObj;
    [SerializeField] float posOffsetY;
    [SerializeField] float bucketsDensity = 0.5f;
    [SerializeField] Transform tutorPoints;

    public static BucketsController Instance { get; private set; }

    List<Bucket> buckets = new();
    Race race;
    List<Car> cars;
    Car currentCar;
    bool isTutorial;

    void OnEnable() => RaceProgress.Finish += Finish;
    void OnDisable() => RaceProgress.Finish -= Finish;

    void Awake() => Instance = this;

    public void Init(Race race)
    {
        this.race = race;
        cars = race.TrafficGenerator.Traffic;
        AddBuckets();
    }

    public Bucket GetBucket(int colorId)
    {
        if (buckets.Count == 0) return null;
        return buckets.Find(b => b.Id == colorId);
    }

    void AddBuckets()
    {
        foreach (var color in ColorPicker.Pool)
        {
            var bucket = Instantiate(bucketObj, race.EndPoint.position + Vector3.left, Quaternion.identity);
            bucket.Init(ColorPicker.Pool.IndexOf(color), color);
            buckets.Add(bucket);
        }
    }

    public void UpdateBuckets()
    {
        if (buckets.Count == 0) return;
        var pending = buckets.Where(b => b.transform.position.x < race.EndPoint.position.x).ToList();

        foreach (var bucket in pending)
        {
            for (var i = cars.IndexOf(currentCar) + 1; i < cars.Count; i++)
            {
                if (Mathf.Abs(cars[i].transform.position.x - race.StartPoint.position.x) < 1)
                {
                    if (Random.value > bucketsDensity) continue;

                    PlaceBucket(bucket, cars[i]);
                    currentCar = cars[i];
                    break;
                }
            }
        }

        void PlaceBucket(Bucket bucket, Car car)
        {
            bucket.transform.position = car.transform.position + Vector3.up * posOffsetY;
            bucket.transform.SetParent(car.transform);
            bucket.Clear();
            car.SpriteRenderer.color = bucket.Color + Color.gray;
        }
    }

    public void FillBucket(int colorId, int count)
    {
        if (buckets.Count == 0) return;
        var bucket = buckets.Find(b => b.Id == colorId);
        var full = bucket.Fill(count);
        if (full && !isTutorial) race.Boost();
    }

    void Finish()
    {
        foreach (var bucket in buckets)
            Destroy(bucket.gameObject);
        buckets.Clear();
    }

    public void InitTutorial(Race race)
    {
        this.race = race;
        isTutorial = true;
        AddBuckets();
        foreach (var bucket in buckets)
        {
            bucket.SetCapacity(5);
            bucket.transform.position = tutorPoints.GetChild(bucket.Id).position;
            bucket.transform.parent = tutorPoints;
            bucket.transform.Rotate(Vector3.forward, 90);
            bucket.transform.localScale = Vector3.one * 1.2f;
        }
    }
}
