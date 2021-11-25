
using System;
using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
	private Transform player;
	private void Start()
	{
		player = FindObjectOfType<Snake>().transform;
	}
	
	private  void FixedUpdate()
	{
		Camera.main.transform.position = new Vector3(player.transform.position.x,
			player.transform.position.y + 4.5f, player.transform.position.z + 7f);
	}
				
}























