using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI distance, maxDistance;
    [SerializeField] ReactableAnimator animator;


    public Slider Slider => slider;
    public TextMeshProUGUI Distance => distance;
    public TextMeshProUGUI MaxDistance => maxDistance;
    public ReactableAnimator Animator => animator;

}
