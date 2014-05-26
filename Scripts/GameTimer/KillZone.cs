using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (GameObject gobj in GameObject.FindGameObjectsWithTag("Enemy")){
			if (this.transform.position.x < -11){
				GameObject.Destroy(gobj);
			}
		}
	}
}

