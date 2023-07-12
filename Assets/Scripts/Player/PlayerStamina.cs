using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    private float stamina;
    private float lerpTimer;
    private bool canSprint;
    private bool isMoving;
    private bool isGrounded;

    public float maxStamina = 100f;
    public float sprintSpeed = 8f;
    public float regenerationRate = 10f;
    public float depletionRate = 20f;
    public float jumpStaminaCost = 10f;
    public float staminaDepletedCooldown = 3f;
    public float chipSpeed = 2f;
    public Image frontStaminaBar;
    public Image backStaminaBar;

    private PlayerMotor playerMotor;
    private bool staminaDepleted;
    private float staminaDepletedTimer;

    void Start()
    {
        stamina = maxStamina;
        playerMotor = GetComponent<PlayerMotor>();
        canSprint = true;
        isMoving = false;
        staminaDepleted = false;
        staminaDepletedTimer = 0f;
    }

    void Update()
    {
        UpdateStamina();
        Vector2 input = playerMotor.GetMovementInput();

        if (input.magnitude > 0f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        isGrounded = playerMotor.GroundCheck();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canSprint && !playerMotor.IsSprinting() && !playerMotor.IsCrouching())
            {
                if (isMoving)
                {
                    playerMotor.Sprint();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (playerMotor.IsSprinting())
            {
                playerMotor.Sprint();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canSprint && !playerMotor.IsSprinting() && isGrounded && stamina >= jumpStaminaCost)
            {
                playerMotor.Jump();
                stamina -= jumpStaminaCost;
            }
        }

        if (playerMotor.IsSprinting() && isMoving && !playerMotor.IsCrouching())
        {
            stamina -= Time.deltaTime * depletionRate;
            if (stamina <= 0)
            {
                stamina = 0;
                canSprint = false;
                staminaDepleted = true;
                playerMotor.Sprint();
            }
        }
        else
        {
            if (stamina < maxStamina && !staminaDepleted)
            {
                stamina += Time.deltaTime * regenerationRate;
                if (stamina > maxStamina)
                {
                    stamina = maxStamina;
                    canSprint = true;
                }
            }
        }

        float staminaFraction = stamina / maxStamina;

        float fillFront = frontStaminaBar.fillAmount;
        float fillBack = backStaminaBar.fillAmount;

        if (fillBack > staminaFraction)
        {
            frontStaminaBar.fillAmount = staminaFraction;
            backStaminaBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            backStaminaBar.fillAmount = Mathf.Lerp(fillBack, staminaFraction, percentComplete);
        }

        if (fillFront < staminaFraction)
        {
            backStaminaBar.color = Color.green;
            backStaminaBar.fillAmount = staminaFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontStaminaBar.fillAmount = Mathf.Lerp(fillFront, backStaminaBar.fillAmount, percentComplete);
        }

        if (staminaDepleted)
        {
            staminaDepletedTimer += Time.deltaTime;
            if (staminaDepletedTimer >= staminaDepletedCooldown)
            {
                staminaDepleted = false;
                staminaDepletedTimer = 0f;
                canSprint = true;
            }
        }
    }

    public void UpdateStamina()
    {
        float fillFront = frontStaminaBar.fillAmount;
        float fillBack = backStaminaBar.fillAmount;
        float staminaFraction = stamina / maxStamina;

        if (fillBack > staminaFraction)
        {
            frontStaminaBar.fillAmount = staminaFraction;
            backStaminaBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            backStaminaBar.fillAmount = Mathf.Lerp(fillBack, staminaFraction, percentComplete);
        }

        if (fillFront < staminaFraction)
        {
            backStaminaBar.color = Color.green;
            backStaminaBar.fillAmount = staminaFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontStaminaBar.fillAmount = Mathf.Lerp(fillFront, backStaminaBar.fillAmount, percentComplete);
        }
    }
}
