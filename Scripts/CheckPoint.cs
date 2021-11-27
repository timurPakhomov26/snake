using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{   
   [SerializeField] private MeshRenderer _checkPointMesh;
   private MeshRenderer _player;

    private void Start()
    {
      
    }
    private void Update()
    {
          _player = FindObjectOfType<Snake>().GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player.material.color = _checkPointMesh.material.color;
        }
    }
}