using System;
using System.Collections;
using UnityEngine;

public class MovingEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem[] childEffects;
    [SerializeField] float moveTime;
    [SerializeField] AnimationCurve moveCurveX;
    [SerializeField] ParticleSystem afterEffetc;

    Transform target;

    public void MoveTo(Transform target)
    {
        this.target = target;
        StartCoroutine(Moving());
        RecolorChildren();
    }

    IEnumerator Moving()
    {
        float time = 0;
        var start = transform.position;
        while (time < moveTime)
        {
            time += Time.deltaTime;
            if (target == null) break;
            transform.position = Vector2.Lerp(start, target.position, moveCurveX.Evaluate(time / moveTime));
            yield return null;
        }
        gameObject.SetActive(false);
        EffectsController.Instance.PlayEffect(3, transform.position);
    }

    void RecolorChildren()
    {
        var eff = gameObject.GetComponent<ParticleSystem>();
        if (eff == null) return;
        var mian = eff.main;
        foreach (var effect in childEffects)
        {
            var child = effect.main;
            float alpha = child.startColor.color.a;
            child.startColor = 
                new Color(mian.startColor.color.r, 
                          mian.startColor.color.g, 
                          mian.startColor.color.b, alpha);
        }
    }
}

