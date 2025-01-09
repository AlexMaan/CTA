using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class RoadMap : MonoBehaviour
{
    [SerializeField] GameObject mapObj;
    [SerializeField] RoadPoint pointPrefab;
    [SerializeField] Transform startPos;
    [SerializeField] float pointDistance;
    [SerializeField] int pointCount, shiftCount;
    [SerializeField] AnimationCurve moveCurve;
    [SerializeField] float movetime;

    Queue<RoadPoint> map = new();
    int maxLevelCount;
    int currentLevel;


    void Awake()
    {
        maxLevelCount = Loader.Instance.LoaderConfig.MaxLevelCount;
        currentLevel = Loader.Instance.PlayerConfigManager.PlayerProfileData.CurrentLevel;
    }

    void Start() => map = GenerateMap();

    Queue<RoadPoint> GenerateMap()
    {
        var points = new Queue<RoadPoint>();
        for (int i = 0; i < pointCount; i++)
        {
            var point = Instantiate(pointPrefab, mapObj.transform);
            point.transform.position =
                new Vector2(startPos.position.x, startPos.position.y + i * pointDistance);
            points.Enqueue(point);
            point.Init(currentLevel - shiftCount + i);
        }
        return points;
    }

    public void MoveToNextPoint()
    {
        var first = map.Dequeue();
        var last = map.Last();
        first.transform.position = last.transform.position + Vector3.up * pointDistance;
        first.Init(last.LevelNumber + 1);
        map.Enqueue(first);
        first.transform.SetAsLastSibling();
        MoveMap();
    }

    async void MoveMap()
    {
        var startPos = mapObj.transform.position;
        var targetPos = startPos + Vector3.down * pointDistance;
        float time = 0;
        while (time < movetime)
        {
            mapObj.transform.position = Vector2.Lerp(startPos, targetPos,
                                        moveCurve.Evaluate(time / movetime));
            time += Time.deltaTime;
            await Task.Yield();
        }
        mapObj.transform.position = targetPos;
    }
}
