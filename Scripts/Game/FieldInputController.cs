using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class FieldInputController : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Field field;
    bool isDrag = false;
    int pickedColor = -1;
    List<Ball> pickedBalls = new List<Ball>();

    int accuracy = 5;
    Vector2[] pointerPos;
    public Vector2 PointerPos => pointerPos[accuracy - 1];

    public static Action<Ball> OnBallSelected;
    public static Action OnDragResolve;

    void OnEnable() => RaceProgress.Finish += () => isDrag = false;

    void Awake() => pointerPos = new Vector2[accuracy];

    void Update()
    {
        if (isDrag)
        {
            foreach (var pos in pointerPos)
            {
                var ray = Camera.main.ScreenPointToRay(pos);
                var hit = Physics2D.GetRayIntersection(ray, 100, LayerMask.GetMask("Ball"));
                if (hit)
                {
                    var ball = hit.collider.GetComponent<Ball>();
                    if (ball) HandleBall(ball);
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Field.IsFilling) return;
        var ball = eventData.pointerCurrentRaycast.gameObject.GetComponent<Ball>();
        if (ball)
        {
            isDrag = true;
            pickedColor = ball.ColorID;
            HandleBall(ball);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDrag) ResolveDrag();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        pointerPos[0] = PointerPos;
        pointerPos[accuracy - 1] = eventData.position;
        for (int i = 1; i < accuracy - 1; i++)
        {
            pointerPos[i] = pointerPos[i - 1] + (pointerPos[accuracy - 1] - pointerPos[0]) / accuracy;
        }
    }

    void ResolveDrag()
    {
        OnDragResolve?.Invoke();

        isDrag = false;
        foreach (var ball in pickedBalls)
        {
            ball.Select(false);
        }
        field.Collect(pickedBalls);

        pickedBalls.Clear();
        for (int i = 0; i < accuracy; i++)
            pointerPos[i] = Vector2.zero;
    }

    void HandleBall(Ball ball)
    {
        if (ball.ColorID != pickedColor) return;

        switch (ball)
        {
            case var _ when !pickedBalls.Contains(ball)
                            && CheckAdjacent(ball):
                pickedBalls.Add(ball);
                ball.Select(true, pickedBalls.Count);
                OnBallSelected?.Invoke(ball);
                break;
            case var _ when pickedBalls.Contains(ball)
                            && pickedBalls.IndexOf(ball) != pickedBalls.Count - 1:
                var id = pickedBalls.IndexOf(ball);
                for (int i = pickedBalls.Count - 1; i > id; i--)
                {
                    pickedBalls[i].Select(false);
                    pickedBalls.RemoveAt(i);
                    OnBallSelected?.Invoke(ball);
                }
                break;
        }
    }

    bool CheckAdjacent(Ball target)
    {
        if (pickedBalls.Count == 0) return true;
        var prev = pickedBalls[pickedBalls.Count - 1];
        return Mathf.Abs(prev.Column - target.Column) <= 1
            && Mathf.Abs(prev.Row - target.Row) <= 1;
    }
}
