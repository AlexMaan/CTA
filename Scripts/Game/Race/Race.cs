using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum Terrain { Country, City, Mix }

public class Race : MonoBehaviour
{
    [SerializeField] RoadGenerator roadGenerator;
    [SerializeField] TrafficGenerator trafficGenerator;
    [SerializeField] ItemsGenerator itemsGenerator;
    [SerializeField] BucketsController bucketsController;
    [SerializeField] RaceProgress raceProgress;
    [SerializeField] GameObject view, viewUI, tutorView, road, tiles, traffic;
    [SerializeField] Transform startPoint, endPoint;
    [SerializeField] ReactableAnimator car, truck;
    [SerializeField] BoostFrame boostFrame;
    [SerializeField] float startAnimTime;
    [SerializeField] float startSpeed, maxSpeed, speedBoost;
    GameController gameController;

    public static bool IsActive { get; private set; }
    public static float Speed { get; private set; }
    public static Terrain Terrain { get; private set; }

    public Transform StartPoint => startPoint;
    public Transform EndPoint => endPoint;
    public GameObject Traffic => traffic;
    public Transform Car => car.transform;
    public Transform Truck => truck.transform;
    public RaceProgress RaceProgress => raceProgress;
    public TrafficGenerator TrafficGenerator => trafficGenerator;
    public BucketsController BucketsController => bucketsController;

    void OnEnable()
    {
        BoostFrame.Trucked += LoseRace;
        RaceProgress.Finish += WinRace;
    }
    void OnDisable()
    {
        BoostFrame.Trucked -= LoseRace;
        RaceProgress.Finish -= WinRace;
    }

    void Awake()
    {
        IsActive = false;
        Speed = 0;
        Terrain = Terrain.Mix;
        tutorView.SetActive(false);
    }

    public void Init(GameController controller)
    {
        gameController = controller;
        if (gameController.LevelData.IsTutorial == true)
            LoadTutorial();
        else
        {
            roadGenerator.Init(this, tiles, itemsGenerator);
            trafficGenerator.Init(this);
            bucketsController.Init(this);
            StartCoroutine(StartRace());
        }
    }

    IEnumerator StartRace()
    {
        yield return new WaitForSeconds(startAnimTime);
        Boost(true);
        IsActive = true;
        StartCoroutine(ChangingSpeed(startSpeed, 2));
    }

    void Update()
    {
        if (!IsActive) return;
        road.transform.position += Vector3.left * Speed * Time.deltaTime;
    }

    public void Boost(bool init = false)
    {
        int id = init ? 0 : 1;
        car.Act(id);
        boostFrame.Boost();
        Speed += speedBoost;
        if (Speed > maxSpeed) Speed = maxSpeed;
    }

    void WinRace()
    {
        car.Act(2);
        truck.Act(0);
        gameController.EndCondition(true);
        StartCoroutine(ChangingSpeed(Speed, 1));
    }

    void LoseRace()
    {
        car.Act(3);
        truck.Act(0);
        gameController.EndCondition(false);
        StartCoroutine(ChangingSpeed(startSpeed / 2, 1));
        EffectsController.Instance.PlayEffect(4, car.transform.position);
    }

    IEnumerator ChangingSpeed(float targetSpeed, float accelK)
    {
        if (targetSpeed > Speed)
        {
            while (Speed < targetSpeed)
            {
                Speed += Time.deltaTime * accelK;
                yield return null;
            }
        }
        else
        {
            while (Speed > targetSpeed)
            {
                Speed -= Time.deltaTime * accelK;
                yield return null;
            }
        }
    }
    void LoadTutorial()
    {
        view.SetActive(false);
        tutorView.SetActive(true);
        bucketsController.InitTutorial(this);
        viewUI.GetComponent<CanvasGroup>().alpha = 0;
    }
}
