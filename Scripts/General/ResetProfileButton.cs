using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetProfileButton : MonoBehaviour
{
    public void ResetProfile()
    {
        PlayerProfileManager.Instance.ResetConfig();
    }
}
