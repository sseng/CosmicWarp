using UnityEngine;
using System.Collections;

public class EnemyDropsFuel : EnemyDrops {
	public float fuel = 10.0f;
	public GameObject pickupParticles;
	// Use this for initialization
	void Start()
	{
		direction = new Vector3(Random.Range(-1.0f, 0), Random.Range(-1.0f, 1.0f), 0);		
	}
	
	// Update is called once per frame
	void Update() {
		DropsMovement();
	}
	
	void OnTriggerEnter( Collider other ){
		if (other.tag == "Player"){					
			HeroShip script = other.GetComponent<HeroShip>();
			GameTimer warpFuel = GameObject.Find("GameTimer").GetComponent<GameTimer>();
			
			if (!script.isInvulnerable){
				warpFuel.addFuel(fuel);
				GameObject particle = GameObject.Instantiate(this.pickupParticles, this.gameObject.transform.position, this.pickupParticles.transform.rotation) as GameObject;
				GameObject.Destroy(particle, 1.0f);
				Destroy(this.gameObject);
			}
		}
	}
}
