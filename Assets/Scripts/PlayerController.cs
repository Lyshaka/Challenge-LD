using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] private float movementSpeed = 15f;
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private float dashForce = 30f;

	[Header("Technical")]
	[SerializeField] private Transform playerFeet;

	private Vector2 checkpointPosition = Vector2.zero;

	private float horizontalInput;
	private int direction = 1;

	private bool isGrounded = true;
	private bool canDash = true;
	private bool isDashing = false;
	private float dashTime = 0.15f;

	private Rigidbody2D rb;
	private GameObject cameraObj;

	public void SetCheckpoint(Vector2 pos)
	{
		checkpointPosition = pos;
	}

	public void Damage(float amount)
	{
		Debug.Log("Dead !");
		transform.position = checkpointPosition;
	}

	private void Jump()
	{
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	}

	private bool IsGrounded()
	{
		return (Physics2D.OverlapCircle(playerFeet.position, 0.1f, (1 << 6)));
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		cameraObj = Camera.main.gameObject;
	}

	void Update()
	{
		cameraObj.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
		horizontalInput = Input.GetAxis("Horizontal");


		if (Input.GetAxisRaw("Horizontal") < 0)
			direction = -1;
		else if (Input.GetAxisRaw("Horizontal") > 0)
			direction = 1;

		isGrounded = IsGrounded();
		if (!isDashing && isGrounded)
		{
			canDash = true;
		}

		if (isGrounded && Input.GetKeyDown(KeyCode.Space))
			Jump();
		if (canDash && !isDashing && Input.GetKeyDown(KeyCode.LeftShift))
			StartCoroutine(Dash());
	}

	void FixedUpdate()
	{
		if (!isDashing)
		{
			rb.velocity = new Vector2(horizontalInput * movementSpeed, rb.velocity.y);
		}
	}

	IEnumerator Dash()
	{
		canDash = false;
		isDashing = true;
		float defaultGravity = rb.gravityScale;
		rb.gravityScale = 0;
		rb.velocity = new Vector2(dashForce * direction, 0f);
		yield return new WaitForSeconds(dashTime);
		rb.gravityScale = defaultGravity;
		isDashing = false;
	}
}