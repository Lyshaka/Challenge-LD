using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject door;

	void Start()
	{
		door.GetComponent<TimeDoor>().GetTiming();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == 3)
			door.GetComponent<TimeDoor>().OpenDoor();
	}
}
