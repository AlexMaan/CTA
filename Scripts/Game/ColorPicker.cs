using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] List<Color> colorVariants;
    List<Color> pool = new();

    public static List<Color> Pool { get; private set; }

    public void DefineColors(int count)
    {
        var colors = new List<Color>(colorVariants);
        for (int i = 0; i < count; i++)
        {
            var index = Random.Range(0, colors.Count);
            pool.Add(colors[index]);
            colors.RemoveAt(index);
        }
        Pool = pool;
    }

    public (int, Color) PickColor()
    {
        int id = Random.Range(0, pool.Count);
        Color color = pool[id];
        return (id, color);
    }
}
