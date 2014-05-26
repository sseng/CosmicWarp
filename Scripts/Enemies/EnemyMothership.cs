using UnityEngine;
using System.Collections;

public enum MothershipStates { PHASE1, PHASE2, PHASE3 };

public class EnemyMothership : Enemy
{
	private GameObject target;
	private float attackTimer = 0.0f;
	private float movespeed = 5.0f;
	public GameObject attackProjectile1;
	public GameObject attackProjectile2;
	public GameObject deathParts;
	public GameObject spawnGroup1;
	public GameObject spawnGroup2;
	
	public float attackDelay = 5.0f;
	
	public float primaryAttackDamage = 10.0f;
	public float primaryAttackSpeed = 10.0f;
	
	public float laserAttackDamage = 25.0f;
	
	public float currentHealth = 10000.0f;
	public float totalHealth = 10000.0f;
	
	
	
	private MothershipStates state = MothershipStates.PHASE1;
	private Vector3 gunLocation;
	// Use this for initialization
	void Start ()
	{
		this.gunLocation = new Vector3(-7.0f, 1.0f, 0.0f);
		this.target = GameObject.Find("Target");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ((target.transform.position - this.gameObject.transform.position).magnitude > 0.1f){
			this.gameObject.transform.position += (target.transform.position - this.gameObject.transform.position).normalized * this.movespeed * Time.deltaTime;
		}
		
		if (this.currentHealth <= 0.0f){
			GameObject deathParts = GameObject.Instantiate(this.deathParts, this.gameObject.transform.position, this.deathParts.transform.rotation) as GameObject;
			GameObject.Destroy(deathParts, 5.0f);
			GameObject.Destroy(this.gameObject);
		}
		
		if (this.currentHealth < (this.totalHealth / 4.0f)){
			this.state = MothershipStates.PHASE2;
		}
		
		if (this.currentHealth < (this.totalHealth / 1.5f)){
			this.state = MothershipStates.PHASE3;
		}
		
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player != null){
			if (!player.GetComponent<Warp>().isWarping){
				if (this.attackTimer >= this.attackDelay){
					//InvokeRepeating("PrimaryAttack", 0, this.primaryAttackDelay);
					switch (this.state){
					case MothershipStates.PHASE1:
						float randomAttack1 = Random.Range(0,2);
						
						if (randomAttack1 == 0){
							Invoke("PrimaryAttack", 0.0f);
						}
						else{
							Invoke("SecondaryAttack", 0.0f);
						}
						
						break;
					case MothershipStates.PHASE2:
						float randomAttack2 = Random.Range(0,2);
						
						if (randomAttack2 == 0){
							Invoke("Spawn", 0.0f);
						}
						else{
							Invoke("SecondaryAttack", 0.0f);
						}
						break;
					case MothershipStates.PHASE3:
						this.attackDelay = 2.0f;
						float randomAttack3 = Random.Range(0,3);
						
						if (randomAttack3 == 0){
							Invoke("PrimaryAttack", 0.0f);
						}
						else if (randomAttack3 == 1){
							Invoke("SecondaryAttack", 0.0f);
						}
						else{
							Invoke("LaserAttack", 0.0f);
						}
						break;
					default:
						break;
					}
					this.attackTimer = 0.0f;
				}
			}
		}
		
		this.attackTimer += Time.deltaTime;
	}
	
	private void PrimaryAttack(){
		for (int i = 0; i < 30; i++){
			Invoke("CreateProjectile1", (i * 0.05f));
		}
	}
	
	private void SecondaryAttack(){
		for (int i = 0; i < 20; i++){
			Invoke("CreateProjectile2", (i * 0.05f));
		}
	}
	
	private void Spawn(){
		float random = Random.Range(0,2);
		if (random == 0){
			GameObject spawn = GameObject.Instantiate(this.spawnGroup1, this.gameObject.transform.position, this.spawnGroup1.transform.rotation) as GameObject;
			GameObject.Destroy(spawn, 10.0f);
		}
		else{
			GameObject spawn = GameObject.Instantiate(this.spawnGroup2, this.gameObject.transform.position, this.spawnGroup2.transform.rotation) as GameObject;
			GameObject.Destroy(spawn, 10.0f);
		}
	}
	
	private void LaserAttack(){
		GameObject laser = GameObject.Instantiate(this.attackProjectile2, this.gameObject.transform.position + this.gunLocation, this.attackProjectile2.transform.rotation) as GameObject;
		
		EnemyProjectile pscript = laser.GetComponent<EnemyProjectile>();
		pscript.direction = Vector3.zero;
		pscript.speed = 0.0f;
		pscript.damage = this.laserAttackDamage;
		
		GameObject.Destroy(laser, 5.0f);
	}
	
	private void CreateProjectile1(){
		GameObject primary = GameObject.Instantiate(this.attackProjectile1, this.gameObject.transform.position + this.gunLocation, this.attackProjectile1.transform.rotation) as GameObject;
		
		EnemyProjectile pscript = primary.GetComponent<EnemyProjectile>();
		pscript.direction = (GameObject.FindGameObjectWithTag("Player").transform.position - this.enemy.transform.position).normalized;
		pscript.speed = this.primaryAttackSpeed;
		pscript.damage = this.primaryAttackDamage;
		
		GameObject.Destroy(primary, 5.0f);
	}
	
	private void CreateProjectile2(){
		GameObject primary = GameObject.Instantiate(this.attackProjectile1, this.gameObject.transform.position + this.gunLocation, this.attackProjectile1.transform.rotation) as GameObject;
		EnemyProjectile pscript1 = primary.GetComponent<EnemyProjectile>();
		pscript1.direction = new Vector3(-1.0f, 0.4f, 0.0f);
		pscript1.speed = this.primaryAttackSpeed;
		pscript1.damage = this.primaryAttackDamage;
		GameObject.Destroy(primary, 5.0f);
		
		GameObject secondary = GameObject.Instantiate(this.attackProjectile1, this.gameObject.transform.position + this.gunLocation, this.attackProjectile1.transform.rotation) as GameObject;
		EnemyProjectile pscript2 = secondary.GetComponent<EnemyProjectile>();
		pscript2.direction = new Vector3(-1.0f, -0.4f, 0.0f);
		pscript2.speed = this.primaryAttackSpeed;
		pscript2.damage = this.primaryAttackDamage;
		GameObject.Destroy(secondary, 5.0f);
		
		GameObject third = GameObject.Instantiate(this.attackProjectile1, this.gameObject.transform.position + this.gunLocation, this.attackProjectile1.transform.rotation) as GameObject;
		EnemyProjectile pscript3 = third.GetComponent<EnemyProjectile>();
		pscript3.direction = new Vector3(-1.0f, 0.0f, 0.0f);
		pscript3.speed = this.primaryAttackSpeed;
		pscript3.damage = this.primaryAttackDamage;
		GameObject.Destroy(third, 5.0f);
		
		GameObject fourth = GameObject.Instantiate(this.attackProjectile1, this.gameObject.transform.position + this.gunLocation, this.attackProjectile1.transform.rotation) as GameObject;
		EnemyProjectile pscript4 = fourth.GetComponent<EnemyProjectile>();
		pscript4.direction = new Vector3(-1.0f, 0.75f, 0.0f);
		pscript4.speed = this.primaryAttackSpeed;
		pscript4.damage = this.primaryAttackDamage;
		GameObject.Destroy(fourth, 5.0f);
		
		GameObject fifth = GameObject.Instantiate(this.attackProjectile1, this.gameObject.transform.position + this.gunLocation, this.attackProjectile1.transform.rotation) as GameObject;
		EnemyProjectile pscript5 = fifth.GetComponent<EnemyProjectile>();
		pscript5.direction = new Vector3(-1.0f, -0.75f, 0.0f);
		pscript5.speed = this.primaryAttackSpeed;
		pscript5.damage = this.primaryAttackDamage;
		GameObject.Destroy(fifth, 5.0f);
	}
}

