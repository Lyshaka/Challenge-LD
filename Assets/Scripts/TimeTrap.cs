using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrap : MonoBehaviour
{
	[SerializeField] private GameObject thorns;
	[SerializeField] private float triggerDelay = 0f;
	[SerializeField] private float onDelay = 0.5f;
	[SerializeField] private float offDelay = 2f;
	[SerializeField] private float time = 0.1f;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(DelayCycle(triggerDelay));
	}

	// Update is called once per frame
	void Update()
	{

	}

	IEnumerator DelayCycle(float delay)
	{
		yield return new WaitForSeconds(delay);
		StartCoroutine(Cycle());
	}

	IEnumerator Cycle()
	{
		float elapsedTime = 0f;
		while (elapsedTime < offDelay)
		{
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		elapsedTime = 0f;
		while (elapsedTime < time)
		{
			thorns.transform.localPosition = new Vector2(0f, (1 - (elapsedTime / time)) * -0.7f);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		thorns.transform.localPosition = new Vector2(0f, 0f);
		elapsedTime = 0f;
		while (elapsedTime < onDelay)
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
		while (elapsedTime < time)
		{
			thorns.transform.localPosition = new Vector2(0f, (elapsedTime / time) * -0.7f);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		thorns.transform.localPosition = new Vector2(0f, -0.7f);
		StartCoroutine(Cycle());
	}
}
