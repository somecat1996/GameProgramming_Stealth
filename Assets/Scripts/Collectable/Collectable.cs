using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collectable item
/// </summary>
public class Collectable : MonoBehaviour
{
    public int value = 1;

    /// <summary>
    /// If player touch this object, add gold to wallet and deactivate
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Wallet.instance.AddGold(value);
            gameObject.SetActive(false);
        }
    }
}
