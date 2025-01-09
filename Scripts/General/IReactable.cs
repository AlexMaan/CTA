using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReactable
{
    void Act() { }
    void Act(int id) { }
    void Act(string name) { }
}

