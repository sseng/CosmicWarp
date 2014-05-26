using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour {
	public float secondsLeft = 300.0f;
	public float currentFuel = 0.0f;
	public float maxFuel = 100.0f;
	public GUIStyle timerStyle;
	public GUIStyle buttonStyle;
	//public GUIStyle fuelTex = new GUIStyle();
	//public Texture2D texture = new Texture2D(128, 128);
	
	private float bossBarLength;
	
	private bool destroyed = false;
	private float timer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player != null){
			//Debug.Log(this.secondsLeft);
			Debug.Log(player.GetComponent<Warp>().isWarping);
			if (!player.GetComponent<Warp>().isWarping){
				Debug.Log("still doing this");
				this.secondsLeft -= Time.deltaTime;
				if (this.secondsLeft < 0.0f){
					if (!this.destroyed){
						GameObject deathparts = GameObject.Instantiate(player.GetComponent<HeroShip>().deathParts, player.transform.position, Quaternion.identity) as GameObject;
						GameObject.Destroy(player);
						GameObject.Destroy(deathparts, 5.0f);
						this.destroyed = true;
					}
				}
			}
		}
		//fuelBarLength = (Screen.width / 2) * (currentFuel/maxFuel);
		if (GameObject.FindGameObjectWithTag("Boss") != null){
			float currentHealth = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyMothership>().currentHealth;
			float maxHealth = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyMothership>().totalHealth;
			
			//Debug.Log(currentHealth);
			bossBarLength = (Screen.width / 6) * (currentHealth/maxHealth);
		}
	}
	
	void OnGUI(){
		int timeInt = (int)this.secondsLeft;
		string minutes = (Mathf.FloorToInt(this.secondsLeft/60.0f)).ToString();
		string seconds = (timeInt % 60).ToString();
		if (seconds.Length == 1){
			seconds = "0" + seconds;
		}
		
		if (this.secondsLeft >= 0.0f){
			GUI.Label( new Rect(0.0f, 0.0f, Screen.width, Screen.height), "Time Remaining: " + minutes + ":" + seconds, timerStyle);
			//GUI.Label( new Rect(0.0f, 30.0f, Screen.width, Screen.height), "Fuel Remaining: " + currentFuel, timerStyle);
			
			GUI.Box(new Rect(10, Screen.height - 40, 60, 25), "Fuel: " + currentFuel);
			GUI.Box(new Rect(70, Screen.height - 40, currentFuel, 25), "");
			GUI.Box(new Rect(Screen.width* 3/5, Screen.height - 40, bossBarLength, 25), "MotherShip");
		}
		else{
			if (GUI.Button( new Rect(Screen.width/2.0f - Screen.width/20.0f, Screen.height/2.0f - Screen.width/20.0f, Screen.width/10.0f, Screen.height/10.0f), "Restart")){
				Application.LoadLevel("scene");
			}
		}
		
	}
	
	public void addTime(float amount){
		if(secondsLeft > 0){
			secondsLeft += amount;
		}
	}
	
	public void addFuel(float amount){
		if(currentFuel <= maxFuel){			
			currentFuel += amount;
		}
	}
}
