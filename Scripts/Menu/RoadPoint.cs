using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoadPoint : MonoBehaviour
{
    int levelNumber;
    [SerializeField] TextMeshProUGUI levelText;

    public int LevelNumber => levelNumber;

    public void Init(int level)
    {
        levelNumber = level;
        levelText.text = levelNumber.ToString();
    }
}
