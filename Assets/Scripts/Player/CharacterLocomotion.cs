using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    public Animator rigController;
    public float jumpHeight = 3;
    public float gravity = 20;
    public float stepDown = 0.3f;
    public float airControl = 0.5f;
    public float jumpDamp = 0.5f;
    public float groundSpeed = 1;
    public float pushPower = 2;
    
    public AudioClip footstepSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    private AudioSource audioSource;
    private bool isWalking;
    private float stepInterval = 0.5f; // Интервал между шагами
    private float nextStepTime;

    Animator animator;
    CharacterController cc;
    ActiveWeapon activeWeapon;
    ReloadWeapon reloadWeapon;
    CharacterAiming characterAiming;
    Vector2 input;

    Vector3 rootMotion;
    Vector3 velocity;
    bool isJumping;

    int isSprintingParam = Animator.StringToHash("isSprinting");
    
    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        activeWeapon = GetComponent<ActiveWeapon>();
        reloadWeapon = GetComponent<ReloadWeapon>();
        characterAiming = GetComponent<CharacterAiming>();
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)         // Добавляем AudioSource, если он отсутствует
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (footstepSound == null)  // Проверка на назначение аудиоклипов
        {
            Debug.LogError("Footstep sound is not assigned.");
        }
        if (jumpSound == null)
        {
            Debug.LogError("Jump sound is not assigned.");
        }
        if (landSound == null)
        {
            Debug.LogError("Land sound is not assigned.");
        }
        
        audioSource.enabled = false;  // Отключаем AudioSource в самом начале

        nextStepTime = 0f;
    }
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);

        UpdateIsSprinting();
        
        if (cc.velocity.magnitude > 0.1f) // Включаем или выключаем AudioSource в зависимости от движения персонажа
        {
            if (!audioSource.enabled)
            {
                audioSource.enabled = true;
            }

            if (cc.isGrounded)
            {
                PlayFootstepSound();
            }
        }
        else
        {
            if (audioSource.enabled)
            {
                audioSource.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    bool IsSprinting()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isFiring = activeWeapon.IsFiring();
        bool isReloading = reloadWeapon.isReloading;
        bool isChangingWeapon = activeWeapon.isChangingWeapon;
        bool isAiming = characterAiming.isAiming;
        return isSprinting && !isFiring && !isReloading && !isChangingWeapon && !isAiming;
    }

    private void UpdateIsSprinting()
    {
        bool isSprinting = IsSprinting();
        animator.SetBool(isSprintingParam, isSprinting);
        rigController.SetBool(isSprintingParam, isSprinting);
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
        MoveCharacter(Time.deltaTime);
    }

    void PlayFootstepSound()
    {
        // Используем cc.velocity для проверки, действительно ли персонаж движется
        if (cc.isGrounded && input.magnitude > 0.1f && cc.velocity.magnitude > 0.1f && Time.time > nextStepTime)
        {
            if (audioSource != null && footstepSound != null)
            {
                EnableAudioSourceTemporarily();
                audioSource.PlayOneShot(footstepSound);
                nextStepTime = Time.time + stepInterval;
            }
        }
    }

    void MoveCharacter(float deltaTime)
    {
        if (isJumping)
        { 
            UpdateInAir(deltaTime); // IsInAir state
        }
        else
        { 
            UpdateOnGround(); // IsGrounded state
        }
    }
    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;

        cc.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        if (!cc.isGrounded)
        {
            cc.Move(-stepDownAmount);
            SetInAir(0);
        }
    }

    private void UpdateInAir(float deltaTime)
    {
        velocity.y -= gravity * deltaTime;
        Vector3 displacement = velocity * deltaTime;
        displacement += CalculateAirControl();
        cc.Move(displacement);
        bool wasJumping = isJumping;
        isJumping = !cc.isGrounded;

        if (wasJumping && !isJumping)
        {
            if (audioSource != null && landSound != null)
            {
                EnableAudioSourceTemporarily();
                audioSource.PlayOneShot(landSound); // звук приземления
            }
        }

        rootMotion = Vector3.zero;
        animator.SetBool("isJumping", isJumping);
    }

    Vector3 CalculateAirControl()
    {
        return ((transform.forward * input.y) + (transform.right * input.x)) * (airControl / 100);
    }
    void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
            if (audioSource != null && jumpSound != null)
            {
                EnableAudioSourceTemporarily();
                audioSource.PlayOneShot(jumpSound); // звук прыжка
            }
        }
    }
    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = (animator.velocity * jumpDamp * groundSpeed) + new Vector3(0, jumpVelocity, 0);
        velocity.y = jumpVelocity;
        animator.SetBool("isJumping", true);
    }

    void EnableAudioSourceTemporarily()
    {
        if (!audioSource.enabled)
        {
            audioSource.enabled = true;
            StartCoroutine(DisableAudioSourceAfterPlay());
        }
    }

    IEnumerator DisableAudioSourceAfterPlay()
    {
        yield return new WaitForEndOfFrame(); // Ждем конца кадра
        while (audioSource.isPlaying)
        {
            yield return null;    // Ждем окончания проигрывания
        }
        audioSource.enabled = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }
}
