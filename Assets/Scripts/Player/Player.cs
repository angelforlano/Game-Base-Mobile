using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
	[Header("Player Controller")]
	public Animator anim;
	public Transform interacPoint;
	public LeftJoystick leftJoystick;
      public FixedTouchField cameraTouchField;
	public CinemachineFreeLook cinemachineFreeLook;
      
      [Header("Controller Variables")]
	[Range(0, 200)] public float hp;
      [Range(0, 10)] public float walkSpeed;
      [Range(0, 10)] public float runSpeed;
      [Range(0, 25)] public float turnSpeed;
	public GameObject shieldDome;

      [Header("Playable Variables")]
	public int score;
	public int ammo;

      private Transform _camTrans;
	private Vector3 movement;
      private Vector3 relativeMovement;
	private Quaternion newRotation;
	private float speed;
	private Vector3 leftJoystickInput;

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

	private void Start()
	{
            _camTrans = Camera.main.transform;
	}

	
	// Update is called once per frame
	void Update () 
	{
            cinemachineFreeLook.m_XAxis.Value += cameraTouchField.Movement.x * 5 * Time.deltaTime;
            cinemachineFreeLook.m_YAxis.Value += cameraTouchField.Movement.y / 75 * Time.deltaTime;

            leftJoystickInput = leftJoystick.GetInputDirection();
		// movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            movement = new Vector3(leftJoystickInput.x, 0, leftJoystickInput.y);
		relativeMovement = _camTrans.TransformVector(movement);
		
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
			anim.SetFloat("speed", 1);
			
			transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

			newRotation = Quaternion.LookRotation(relativeMovement); //Para calcular la rotacion, pero no la hace
			transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
			transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0); //LockRot! //Para hacer rotar al Character
		} else
		{
			anim.SetFloat("speed", 0);
            }

		if (Input.GetButtonDown("Interact"))
		{
			Interact();
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
            
		if (hp > 100)
            {
			hp = 100;
            }
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