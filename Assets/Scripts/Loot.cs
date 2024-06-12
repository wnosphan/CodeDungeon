using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] float destroyTime = 0.5f;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Coin"){
            Destroy(other.gameObject, destroyTime);
            Debug.Log("Picked up coin");
        }
    }
}
