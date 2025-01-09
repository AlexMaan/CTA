using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectID
{
    public int id;
    public ParticleSystem effect;
}

public class EffectsController : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] List<EffectID> effectsObj;

    Color currentColor;
    Transform currectTerget;
    List<EffectID> effectsPool = new();

    public static EffectsController Instance { get; private set; }

     void Awake() => Instance = this;

    public void PlayEffect(int id, Vector2 pos, Color color = default, Transform target = default)
    {
        bool colored = color != default;
        bool targeted = target != default;
        if (color != default)
            currentColor = color;
        if (target != default)
            currectTerget = target;

        EffectID effectID;
        if (effectsPool.Count != 0)
        {
            effectID = effectsPool.Find(e => e.id == id);
            if (effectID != null)
            {
                effectID.effect.transform.position = pos;
                StartCoroutine(HandlingEffect(id, effectID.effect, colored, targeted));
                effectsPool.Remove(effectID);
                return;
            }
        }
        var eff = effectsObj.Find(e => e.id == id);
        if (eff == null) return;
        var inst = Instantiate(eff.effect, pos, Quaternion.identity, transform);
        StartCoroutine(HandlingEffect(id, inst, colored, targeted));
    }

    IEnumerator HandlingEffect(int id, ParticleSystem effect, bool colored, bool targeted)
    {
        effect.gameObject.SetActive(true);
        if (colored)
        {
            var main = effect.main;
            float alpha = main.startColor.color.a;
            main.startColor = 
                new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        }
        if (targeted)
        {
            var mover = effect.GetComponent<MovingEffect>();
            if (mover != null) mover.MoveTo(currectTerget);
        }
        yield return new WaitForSeconds(lifeTime);
        PoolEffect(id, effect);
    }

    void PoolEffect(int id, ParticleSystem effect)
    {
        effectsPool.Add(new EffectID { id = id, effect = effect });
        effect.gameObject.SetActive(false);
    }
}
