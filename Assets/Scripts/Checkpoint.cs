using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	[SerializeField] private Transform pos;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == 3)
			other.GetComponent<PlayerController>().SetCheckpoint(pos.position);
	}
}
