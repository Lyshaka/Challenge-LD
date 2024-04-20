using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
	[SerializeField] private GameObject thorns;
	[SerializeField] private float triggerdelay = 0.5f;
	[SerializeField] private float firstTriggerTime = 0.1f;
	[SerializeField] private float secondTriggerTime = 0.1f;
	[SerializeField] private float resetDelay = 0.8f;
	[SerializeField] private float resetTime = 0.1f;

	private bool canActivate = true;


	void OnTriggerEnter2D(Collider2D other)
	{
		if (thorns != null && canActivate)
			StartCoroutine(ActivateTrap());
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	IEnumerator ActivateTrap()
	{
		canActivate = false;
		float elapsedTime = 0f;
		while (elapsedTime < triggerdelay)
		{
			if (elapsedTime < firstTriggerTime)
			{
				thorns.transform.localPosition = new Vector2(0f, -0.7f + (elapsedTime / firstTriggerTime) * 0.1f);
			}
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		elapsedTime = 0f;
		while (elapsedTime < secondTriggerTime)
		{
			thorns.transform.localPosition = new Vector2(0f, -0.7f + (elapsedTime / secondTriggerTime) * 0.7f);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		elapsedTime = 0f;
		while (elapsedTime < resetDelay)
		{
			Collider2D coll = Physics2D.OverlapBox(transform.position, new Vector2(0.9f, 0.9f), 0f, ~(1 << 7));
			if (coll != null)
			{
				//Debug.Log("lol");
				coll.gameObject.GetComponent<PlayerController>().Damage(1f);
			}
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		elapsedTime = 0f;
		while (elapsedTime < resetTime)
		{
			thorns.transform.localPosition = new Vector2(0f, - (elapsedTime / resetTime) * 0.8f);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		canActivate = true;
	}
}
