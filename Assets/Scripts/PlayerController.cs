using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Statistics")]
	[SerializeField] private float maxYValue = -8f;

	[Header("Properties")]
	[SerializeField] private float movementSpeed = 15f;
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private float dashForce = 30f;

	[Header("Technical")]
	[SerializeField] private Transform playerFeet;
	[SerializeField] private Sprite directionArrow;
    [SerializeField] private Sprite dashArrow;
    [SerializeField] private SpriteRenderer directionSprite;

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

	public void TeleportToCheckpoint()
	{
		rb.velocity = Vector2.zero;
		transform.position = checkpointPosition;
	}

	public void Damage(float amount)
	{
		Debug.Log("Dead !");
		KillPlayer();
	}

	public void KillPlayer()
	{
		TeleportToCheckpoint();
	}

	private void Jump()
	{
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	}

	private bool IsGrounded()
	{
		//return (Physics2D.OverlapCircle(playerFeet.position, 0.1f, (1 << 6)));
		return (Physics2D.OverlapBox(playerFeet.position, new Vector2(0.8f, 0.1f), 0, (1 << 6)));
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		cameraObj = Camera.main.gameObject;
	}

	void Update()
	{
		cameraObj.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
		horizontalInput = Input.GetAxisRaw("Horizontal");

		if (transform.position.y < maxYValue)
		{
			KillPlayer();
		}

		if (Input.GetKeyDown(KeyCode.K))
		{
			TeleportToCheckpoint();
		}

		if (Input.GetAxisRaw("Horizontal") < 0)
			direction = -1;
		else if (Input.GetAxisRaw("Horizontal") > 0)
			direction = 1;

		directionSprite.flipX = direction == -1 ? true : false;

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
		directionSprite.sprite = dashArrow;
		float defaultGravity = rb.gravityScale;
		rb.gravityScale = 0;
		rb.velocity = new Vector2(dashForce * direction, 0f);
		yield return new WaitForSeconds(dashTime);
        directionSprite.sprite = directionArrow;
        rb.gravityScale = defaultGravity;
		isDashing = false;
	}
}