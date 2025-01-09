using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostFrame : MonoBehaviour
{
    [SerializeField] float maxPos, minPos, boostTime, slowTime;
    [SerializeField] AnimationCurve boostCurve;

    public static Action Trucked;

    void OnEnable() => RaceProgress.Finish += Finish;
    void OnDisable() => RaceProgress.Finish -= Finish;

    void Start()
    {
        var levelData = MenuController.CurrentLevelData;
        if (levelData != null) slowTime = levelData.Limitation;
    }

    void Finish() => StopAllCoroutines();

    public void Boost()
    {
        StopAllCoroutines();
        StartCoroutine(Boosting());
    }

    IEnumerator Boosting()
    {
        float time = 0;
        float pos = transform.localPosition.x;
        while (time < boostTime)
        {
            time += Time.deltaTime;
            transform.localPosition = new Vector3(Mathf.Lerp(pos, maxPos, boostCurve.Evaluate(time / boostTime)), 0, 0);
            yield return null;
        }
        time = 0;
        while (time < slowTime)
        {
            time += Time.deltaTime;
            transform.localPosition = new Vector3(Mathf.Lerp(maxPos, minPos, time / slowTime), 0, 0);
            yield return null;
        }
        Trucked?.Invoke();
    }
}
