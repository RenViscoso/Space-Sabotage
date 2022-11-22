using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {

	private float timer = 2;

	void Update ()
	{

		if (timer == 0)
		{
			Destroy(this.gameObject);
		}
		else
        {
			timer = Mathf.Clamp(timer - Time.deltaTime, 0, 2);
        }
	
	}
}
