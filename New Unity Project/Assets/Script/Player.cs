using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private float inputDirection; //to jest werktor do movevectora x
    private float verticalVelocity; // to jest do wektora y

	private float speed = 5.0f;
	private float gravity = 30.0f;
	private float jumpForce = 10.0f;
	private bool secondJumpAvail = false; // podwojny skok

	private Vector3 moveVector; //(float,float,float) (x,y,z) /2.5d tylko x i y
	private Vector3 lastMotion;
    private CharacterController controller;

	// Use this for initialization
	void Start () 
	{
		
        controller = GetComponent<CharacterController>();


	
	}
	
	// Update is called once per frame
	void Update () {
		
		moveVector = Vector3.zero;
        inputDirection = Input.GetAxis("Horizontal") * speed;
	
		if (IsControllerGrounded()) // start pentli grawitacja + skok
        {
            verticalVelocity = 0;

            if (Input.GetKeyDown(KeyCode.Space)) // skok na spacji
            {
				verticalVelocity = jumpForce; //podniesienie wysokosci skok na y
				secondJumpAvail = true;
            }

			moveVector.x = inputDirection; // na x
        }
        else //warunek jesli jest inaczej
        {
            if (Input.GetKeyDown(KeyCode.Space)) // skok na spacji
            {
                if(secondJumpAvail)
                { 
					verticalVelocity = jumpForce; //podniesienie wysokosci skok na y ograniczenie petla w pentli zeby zrobiloy sie tylko 2 skoki secondJumpAvail
                    secondJumpAvail = false;
                }
            }
            verticalVelocity -= gravity * Time.deltaTime;
			moveVector.x = lastMotion.x;
        }

		moveVector.y = verticalVelocity; // na y
        //moveVector = new Vector3(inputDirection,verticalVelocity,0);
        controller.Move(moveVector * Time.deltaTime);
		lastMotion = moveVector;
	
	}


	private bool IsControllerGrounded()//naprawianie sterowania klockiem przy skoku i chodzeniu
	{
		Vector3 leftRayStart;
		Vector3 rightRayStart;

		leftRayStart = controller.bounds.center;
		rightRayStart = controller.bounds.center;

		leftRayStart.x -= controller.bounds.extents.x;
		rightRayStart.x += controller.bounds.extents.x;

		Debug.DrawRay(leftRayStart,Vector3.down,Color.red);
		Debug.DrawRay(rightRayStart,Vector3.down,Color.green);

		if (Physics.Raycast(leftRayStart,Vector3.down,(controller.height/2) + 0.1f))
			return true;
		if (Physics.Raycast(rightRayStart,Vector3.down,(controller.height/2) + 0.1f))
			return true;

		return false;
	}
	private void OnControllerColliderHit(ControllerColliderHit hit) // dotyk colidera


	{
		if (controller.collisionFlags == CollisionFlags.Sides) { // collision flags zalerzy od tego gdzie mamy kolizie jaki rozmiar itp
			if (Input.GetKeyDown (KeyCode.Space)) {
				Debug.DrawRay (hit.point, hit.normal, Color.red, 2.0f);
				moveVector = hit.normal * speed;
				verticalVelocity = jumpForce;
				secondJumpAvail = true;
			}
		}
		//zbieranie sfer
		switch (hit.gameObject.tag) 
		{
		case "Coin":// obiekt z nazwa coin zostaje zniszczony przy dotknieciu go
			LevelMenager.Instance.CollectCoin();
			Destroy (hit.gameObject);
			break;

		case "Jumppad":
			verticalVelocity = jumpForce * 2;
			break;

		case "Teleport":
			transform.position = hit.transform.GetChild (0).position;// tranform jest taki sam jak dziecko z przypisanego
			break;

		case "Winbox":
			LevelMenager.Instance.Win ();
			break;

		default:
			break;
		}
	}
}
