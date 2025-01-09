using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionResizer : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] int referenceWidth = 1080;
    [SerializeField] int referenceHigth = 1920;
    float smoothK = 0.2f;

    public static float ScaleK;

    void Awake()
    {
        ScaleK = 1;
        float refRario = (float)referenceWidth / referenceHigth;
        float ratio = (float)Screen.width / Screen.height;
        ScaleK = Mathf.Lerp(ratio / refRario, 1, smoothK);
    }

    void Start() => target.localScale = new Vector3(ScaleK, ScaleK, 1);
}
