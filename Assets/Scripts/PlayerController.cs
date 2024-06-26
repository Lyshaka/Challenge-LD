using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	[Header("Statistics")]
	[SerializeField] private float maxYValue = -8f;

	[Header("Properties")]
	[SerializeField] private float movementSpeed = 15f;
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private int airJumps = 1;
	[SerializeField] private float dashForce = 30f;

	[Header("Technical")]
	[SerializeField] private Transform playerFeet;
	[SerializeField] private Sprite directionArrow;
	[SerializeField] private Sprite dashArrow;
	[SerializeField] private SpriteRenderer directionSprite;
	[SerializeField] private GameObject debugImage;
	[SerializeField] private Vector2 fullCameraPosition;
	[SerializeField] private float fullCameraSize;

	private Vector2 checkpointPosition = Vector2.zero;

	private float defaultGravity;

	private bool canInput = true;
	private bool invicibility = false;
	private bool debug = false;
	private bool fullCamera = false;
	private float debugSpeed = 20;

	private float horizontalInput;
	private int direction = 1;

	private int remainingAirJumps;

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
		StartCoroutine(BlockInput(0.3f));
		horizontalInput = 0f;
		rb.velocity = Vector2.zero;
		transform.position = checkpointPosition;
	}

	public void Damage(float amount)
	{
		if (!invicibility)
			KillPlayer();
	}

	public void KillPlayer()
	{
		if (!debug && !invicibility)
			TeleportToCheckpoint();
	}

	private void Jump()
	{
		if (!isGrounded)
			remainingAirJumps--;
		rb.velocity = new Vector2(rb.velocity.x, 0f);
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	}

	private void ToggleInvicibility()
	{
		invicibility = !invicibility;
		GetComponent<SpriteRenderer>().color = invicibility ? Color.red : Color.white;
	}

	private void ToggleFullCamera()
	{
		fullCamera = !fullCamera;
		if (fullCamera)
		{
			cameraObj.transform.position = new Vector3(fullCameraPosition.x, fullCameraPosition.y, -10f);
			Camera.main.orthographicSize = fullCameraSize;
		}
		else
		{
			Camera.main.orthographicSize = 8;
		}
		
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
		defaultGravity = rb.gravityScale;
	}

	void Update()
	{
		if (!fullCamera)
			cameraObj.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);

		if (Input.GetKeyDown(KeyCode.H))
		{
			debug = !debug;
			debugImage.SetActive(debug);
			rb.gravityScale = debug ? 0f : defaultGravity;
		}
		if (Input.GetKeyDown(KeyCode.K))
		{
			TeleportToCheckpoint();
		}
		if (Input.GetKeyDown(KeyCode.J))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		if (Input.GetKeyDown(KeyCode.I))
		{
			ToggleInvicibility();
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			ToggleFullCamera();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (debug)
		{
			rb.velocity = Vector2.zero;
			transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * debugSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * debugSpeed * Time.deltaTime, 0f);
			

		}
		else
		{
			if (canInput)
			{
				horizontalInput = Input.GetAxisRaw("Horizontal");

				if ((isGrounded || remainingAirJumps > 0) && Input.GetKeyDown(KeyCode.Space))
					Jump();
				if (canDash && !isDashing && Input.GetKeyDown(KeyCode.LeftShift))
					StartCoroutine(Dash());
			}

			if (transform.position.y < maxYValue)
			{
				KillPlayer();
			}

			if (horizontalInput < 0)
				direction = -1;
			else if (horizontalInput > 0)
				direction = 1;

			directionSprite.flipX = direction == -1 ? true : false;

			isGrounded = IsGrounded();
			if (isGrounded)
			{
				remainingAirJumps = airJumps;
			}
			if (!isDashing && isGrounded)
			{
				canDash = true;
			}
		}
	}

	void FixedUpdate()
	{
		if (!isDashing && !debug)
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

	IEnumerator BlockInput(float time)
	{
		canInput = false;
		yield return new WaitForSeconds(time);
		canInput = true;
	}
}