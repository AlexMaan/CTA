using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] BondController bondController;
    [SerializeField] ColorPicker colorPicker;
    [SerializeField] GameObject fieldObj;
    [SerializeField] Ball ballprefab;
    [SerializeField] float animationDelay;
    [SerializeField] AudioSource source;
    int animDelay => (int)(animationDelay * 1000);
    GameController gameController;
    FieldConfig config;

    public static bool IsFilling { get; private set; }
    Queue<Ball> ballsPool = new();
    List<Column> columns = new();
    float cell;

    void Awake() => IsFilling = false;

    public void Init(GameController controller)
    {
        gameController = controller;
        config = Loader.Instance.PlayerConfigManager.PlayerProfileData.FieldConfig;
        colorPicker.DefineColors(config.ColorsCount);
        cell = config.CellSize * ResolutionResizer.ScaleK;
        Generate();
    }

    void Generate()
    {
        fieldObj.transform.localPosition =
            new Vector2(-(config.ColumnsCount - 1) * cell / 2,
                        -(config.RowsCount - 1) * cell / 2);

        for (int i = 0; i < config.ColumnsCount; i++)
        {
            var cells = new List<Cell>();
            for (int j = 0; j < config.RowsCount; j++)
            {
                Vector2 pos = fieldObj.transform.position + new Vector3(i * cell, j * cell, 0);
                cells.Add(new Cell(i, j, pos, AddBall(pos, i, j)));
            }
            columns.Add(new Column(cells));
        }
    }

    public void Collect(List<Ball> picks)
    {
        if (picks.Count < config.MatchCount) return;
        gameController.ConvertToProduct(picks.Count, picks[0].ColorID);

        List<Ball> balls = new();
        foreach (var pick in picks)
        {
            foreach (var cell in columns[pick.Column].Cells)
            {
                if (cell.Row == pick.Row)
                {
                    balls.Add(cell.Ball);
                    cell.Clear();
                    break;
                }
            }
        }
        List<int> order = new();
        foreach (var pick in picks)
        {
            if (!order.Contains(pick.Column))
                order.Add(pick.Column);
        }
        PopBallsAsync(balls, order);
        IsFilling = true;
    }

    Ball AddBall(Vector2 pos, int column, int row)
    {
        Ball ball = null;
        if (ballsPool.Count == 0)
            ball = Instantiate(ballprefab, pos, Quaternion.identity, fieldObj.transform);
        else
        {
            ball = ballsPool.Dequeue();
            ball.gameObject.SetActive(true);
            ball.transform.position = pos;
        }
        ball.SetCellID(column, row);
        ball.SetColor(colorPicker.PickColor());
        return ball;
    }

    async void PopBallsAsync(List<Ball> balls, List<int> order)
    {
        source.Play();
        foreach (var ball in balls)
        {
            await Task.Delay(animDelay / 2);
            ball.Pop(ballsPool);
        }
        RefillAsync(order);
    }

    async void RefillAsync(List<int> order)
    {
        foreach (var columnId in order)
        {
            FillColumnAsync(columnId);
            await Task.Delay(animDelay);
        }
    }

    async void FillColumnAsync(int id)
    {
        var column = columns[id];
        int rows = column.Cells.Count;
        int empty = 0;
        for (int i = 0; i < rows; i++)
        {
            if (column.Cells[i].IsEmpty) empty++;
            else if (empty != 0)
            {
                FillCell(column.Cells[i - empty], column.Cells[i].Ball);
                await Task.Delay(animDelay);
            }
        }
        for (int i = rows - empty; i < rows; i++)
        {
            var pos = column.Cells[rows - 1].Pos + Vector2.up * cell;
            var ball = AddBall(pos, id, rows - 1);
            FillCell(column.Cells[i], ball);
            await Task.Delay(animDelay);
        }
        IsFilling = false;
    }

    void FillCell(Cell gap, Ball ball)
    {
        gap.SetBall(ball);
        ball.MoveTo(gap.Pos);
    }

    //Temp Tutorial

}
