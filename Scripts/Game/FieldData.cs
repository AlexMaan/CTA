using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    int column;
    int row;
    Vector2 pos;
    Ball ball;

    public int Column => column;
    public int Row => row;
    public Ball Ball => ball;
    public Vector2 Pos => pos;

    public Cell(Ball ball)
    {
        this.ball = ball;
        column = 0;
        row = 0;
        pos = Vector2.zero;
    }

    public Cell(int column, int row, Vector2 pos, Ball ball = null)
    {
        this.row = row;
        this.column = column;
        this.ball = ball;
        this.pos = pos;
    }

    public bool IsEmpty => ball == null;
    public void Clear() => ball = null;

    public void SetBall(Ball ball)
    {
        this.ball = ball;
        ball.SetCellID(column, row);
    }
}

[System.Serializable]
public class Column
{
    List<Cell> cells;
    public List<Cell> Cells => cells;
    public Column(List<Cell> cells)
    {
        this.cells = cells;
    }
}

[System.Serializable]
public struct FieldConfig
{
    [SerializeField] int rowsCount, columnsCount, colorsCount, matchCount;
    [SerializeField] float cellSize;

    public int RowsCount => rowsCount;
    public int ColumnsCount => columnsCount;
    public int ColorsCount => colorsCount;
    public int MatchCount => matchCount;
    public float CellSize => cellSize;
}

