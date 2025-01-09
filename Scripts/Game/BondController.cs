using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class BondController : MonoBehaviour
{
    [SerializeField] FieldInputController fieldInput;
    [SerializeField] Bond bondPrefab;

    List<Bond> activeBonds = new();
    Queue<Bond> bondsPool = new();

    void OnEnable()
    {
        FieldInputController.OnBallSelected += AddBond;
        FieldInputController.OnDragResolve += RemoveBoundsAsync;
        RaceProgress.Finish += RemoveBoundsAsync;
    }

    void OnDisable()
    {
        FieldInputController.OnBallSelected -= AddBond;
        FieldInputController.OnDragResolve -= RemoveBoundsAsync;
        RaceProgress.Finish -= RemoveBoundsAsync;
    }

    void Update()
    {
        if (activeBonds.Count > 0) activeBonds.Last().AlignBond(fieldInput.PointerPos);
    }

    public void AddBond(Ball ball)
    {
        if (activeBonds.Count > 0)
        {
            activeBonds.Last().StickBound(ball);

            Bond middle = activeBonds.FirstOrDefault(b => b.Ball == ball);
            int id = activeBonds.IndexOf(middle);
            if (middle != null)
            {
                for (int i = activeBonds.Count - 1; i >= id; i--)
                {
                    PoolBond(activeBonds[i]);
                    activeBonds.RemoveAt(i);
                }
            }
        }
        Bond bond;
        if (bondsPool.Count == 0)
            bond = Instantiate(bondPrefab, transform);
        else bond = bondsPool.Dequeue();
        bond.Init(ball);
        activeBonds.Add(bond);
    }

    async void RemoveBoundsAsync()
    {
        if (activeBonds.Count == 0) return;
        Destroy(activeBonds.Last().gameObject);
        activeBonds.RemoveAt(activeBonds.Count - 1);

        var temp = new List<Bond>(activeBonds);
        activeBonds.Clear();
        foreach (var bound in temp)
        {
            PoolBond(bound);
            await Task.Delay(100);
        }
    }

    void PoolBond(Bond bond)
    {
        bond.Hide();
        bondsPool.Enqueue(bond);
    }
}
