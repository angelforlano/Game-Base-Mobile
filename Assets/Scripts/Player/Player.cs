using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
	[Header("Player Controller")]
	public Animator anim;
	public Transform interacPoint;
	public Transform shootTarget;
	public Animator cameraAnim;
	public AudioSource shootSfx;
	public ParticleSystem shootVfx;

	public Rig weaponContainer_R;

	//public LeftJoystick leftJoystick;
    //public FixedTouchField cameraTouchField;
	//public CinemachineFreeLook cinemachineFreeLook;
      
    [Header("Controller Variables")]
	[Range(0, 200)] public float hp;
	[Range(0, 10)] public float walkSpeed;
	[Range(0, 10)] public float runSpeed;
	[Range(0, 25)] public float turnSpeed;
	[Range(0.1f, 2f)] public float fireRate;
	[Range(1, 75)] public int magazineSize = 35;
	[Range(1, 300)] public int ammoSize = 35;
	public GameObject shieldDome;

    private Transform camTrans;
	private Vector3 movement;
    private Vector3 relativeMovement;
	private Quaternion newRotation;
	private Vector3 leftJoystickInput;
	private int magazine;
	private int ammo;
	private bool isZoom;
	private float speed;
	private float nextFireTime;

	void Interact()
	{
        Collider[] hitColliders = Physics.OverlapSphere(interacPoint.position, 1);
            
		for (int i = 0; i < hitColliders.Length; i++)
		{
			var interact = hitColliders[i].gameObject.GetComponent<GenericInteractable>();
		    
			if (interact != null)
			{
                interact.Interact();
				return;
			}
		}
	}

	void Start()
	{
		camTrans = Camera.main.transform;
		magazine = magazineSize;
		ammo = ammoSize;
	}

	void Update () 
	{
		isZoom = Input.GetMouseButton(1);

		anim.SetBool("aim", isZoom);
		cameraAnim.SetBool("zoom", isZoom);
		
		if (Input.GetKey(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = false;
		} else{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = true;
		}
		
		if (isZoom)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
			if (Physics.Raycast(ray, out hit, 100)) {
				Transform objectHit = hit.transform;
				shootTarget.position = hit.point;
			} else {
				shootTarget.position = Camera.main.transform.position + Camera.main.transform.forward * 100;
			}
		} else {
			shootTarget.position = Camera.main.transform.position + Camera.main.transform.forward * 10;
		}


        //cinemachineFreeLook.m_XAxis.Value += cameraTouchField.Movement.x * 5 * Time.deltaTime;
        //cinemachineFreeLook.m_YAxis.Value += cameraTouchField.Movement.y / 75 * Time.deltaTime;

        //leftJoystickInput = leftJoystick.GetInputDirection();
		
        //movement = new Vector3(leftJoystickInput.x, 0, leftJoystickInput.y);
		movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		anim.SetBool("left", movement.x < 0);
		anim.SetBool("right", movement.x > 0);
		anim.SetBool("back", movement.z < 0);
		anim.SetBool("running", movement.magnitude > 0);
		
		relativeMovement = camTrans.TransformVector(movement);
		
		if (Input.GetButton("Run"))
		{ 
			speed = runSpeed;
		}
		else 
		{
			speed = walkSpeed;
		}	
		
		if (movement.magnitude > 0)
		{
			if (!isZoom)
			{
				transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
				newRotation = Quaternion.LookRotation(relativeMovement);
				transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
				transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
			} else {
				transform.Translate(movement * speed * Time.deltaTime);
			}
		}

		if (isZoom)
		{
			weaponContainer_R.weight += Time.deltaTime * 3;
			newRotation = Quaternion.LookRotation(Camera.main.transform.forward);
			transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
			transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
		} else {
			weaponContainer_R.weight -= Time.deltaTime * 3;
		}

		if (Input.GetButtonDown("Interact"))
		{
			Interact();
		}

		if (Input.GetMouseButton(0) && magazine > 0 && Time.time > nextFireTime)
		{
			Shoot();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Reload();
		}
    }

	void Shoot()
	{
		magazine -=1;
		nextFireTime = Time.time + fireRate;
			
		anim.SetTrigger("shoot");
		shootSfx.Play();
		shootVfx.Play();
	}

	void Reload()
	{
		if (magazine == magazineSize || ammo <= 0)
		{
			Debug.Log("return");
			return;
		}
		
		var reaming_magazine = magazine;
		var load = magazineSize - reaming_magazine;
		
		if (ammo < load)
		{
			magazine += ammo;
			ammo -= ammo;
		} else {
			ammo-=load;
			magazine+=load;
		}
	}

	public void GetDamage(int _damage)
	{
		if(hp >= 0)
		{
            hp -= _damage;
		
			if(hp <= 0)
			{
				anim.SetBool("dead", true);
			}
		}
	}

	public void GetHp(int _hp)
	{
		hp += _hp;
		hp = Mathf.Clamp(hp, 0, 200);
	}

	public void AddAmmo(int _ammo)
	{
		ammo += _ammo;
	}

	IEnumerator OverSpeed(int _speed, int _time)
	{
		walkSpeed += _speed;
		yield return new WaitForSeconds(_time);
        walkSpeed -= _speed;
	}

	IEnumerator ActiveShield(int _time)
	{
		shieldDome.SetActive(true);
		yield return new WaitForSeconds(_time);
		shieldDome.SetActive(false);
	}

	public void AddSpeed(int _speed, int _time)
	{
		StartCoroutine(OverSpeed(_speed, _time));
	}

	public void AddShield(int _time)
	{
		StartCoroutine(ActiveShield( _time));
	}
}