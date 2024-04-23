using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDoor : MonoBehaviour
{
	[SerializeField] private float duration = 5f;

	[SerializeField] private GameObject doorUp;
	[SerializeField] private GameObject doorDown;
	[SerializeField] private GameObject timerDoor;
	[SerializeField] private GameObject timerLever;



	private Coroutine c;

	public void OpenDoor()
	{
		if (c != null)
			StopCoroutine(c);
		c = StartCoroutine(AnimateDoor());
	}

	public float GetTiming()
	{
		return duration;
	}

    // Start is called before the first frame update
    void Start()
    {
        //OpenDoor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	IEnumerator AnimateDoor()
	{
		float elapsedTime = 0f;
		doorUp.SetActive(false);
		doorDown.SetActive(false);
		while (elapsedTime < duration)
		{
			timerDoor.GetComponent<Image>().fillAmount = 1 - (elapsedTime / duration);
			timerLever.GetComponent<Image>().fillAmount = 1 - (elapsedTime / duration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		timerDoor.GetComponent<Image>().fillAmount = 0;
		timerLever.GetComponent<Image>().fillAmount = 0;
		doorUp.SetActive(true);
		doorDown.SetActive(true);
	}
}
