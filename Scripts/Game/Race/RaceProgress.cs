using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceProgress : MonoBehaviour
{
    [SerializeField] float speedK;
    RaceBar raceBar;
    int maxDistance = 500;
    float distance, barSpeed;
    LevelData levelData;
    bool isFinish;
    string last;

    public static Action Finish;

    void Start()
    {
        raceBar = GameObject.FindGameObjectWithTag("RaceBar").GetComponent<RaceBar>();
        levelData = MenuController.CurrentLevelData;
        if (levelData != null) maxDistance = levelData.Goal;
        StartCoroutine(ShowingDistance());
        StartCoroutine(CheckingSpeed());
    }

    void Update()
    {
        if (!Race.IsActive || isFinish) return;
        distance += barSpeed * Time.deltaTime;
        barSpeed = Race.Speed * speedK;
        raceBar.Slider.value = distance / maxDistance;
        if (distance > maxDistance)
        {
            Finish?.Invoke();
            isFinish = true;
        }
    }

    IEnumerator ShowingDistance()
    {
        while (!isFinish)
        {
            var speed = Race.Speed * 15;
            raceBar.Distance.text = ((int)(distance * 100 / maxDistance)).ToString("F0");
            raceBar.MaxDistance.text = speed.ToString("F0");
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator CheckingSpeed()
    {
        while (!isFinish)
        {
            if (last != raceBar.MaxDistance.text)
            {
                last = raceBar.MaxDistance.text;
                raceBar.Animator.Act();
            }
            yield return null;
        }
    }
}
