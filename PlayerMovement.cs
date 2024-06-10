using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterController controller;
    
    enum Type { Melee, Shield };
    
    public float speed = 12f;
    public float backspeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public Transform em;
    public float gorundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    Vector3 Pc;
    bool isGrounded;
    bool de = false;
    public Animator anim;
    bool hit = false;
    // Update is called once per frame
    public Weapon equipWeapon;
    public Weapon Shleid;
    
    bool isFireReady = true;
    
    public float fireDelay;
    float sh;

    public float knockbackForce;
    public float knockbackTime;
    public float knockbackCounter;

    
    
    void Update()
    {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, gorundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            anim.SetBool("jump", false);
            if (de)
            {
                fireDelay = -1f;
                de = false;
            }
            
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * x + transform.forward * z;

        if(fireDelay < 0)
        {
            move = Vector3.zero;
        }
        
        
        
        if(knockbackCounter <= 0)
        {
            if (Input.GetMouseButton(1))
            {
                controller.Move(move * speed * Time.deltaTime/3);
                sh = 0.1f;

            }
            else
            {
                sh = 1f;
                if (z >= 0)
                {
                    controller.Move(move * speed * Time.deltaTime);
                }
                else if (z < 0)
                {
                    controller.Move(move * backspeed * Time.deltaTime);
                }
            }

            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                anim.SetBool("jump", true);
            }
            if (Input.GetMouseButton(1))
            {
                ShieldAttack();
            }
            else if(!isGrounded)
            {
                JumpAttack();
            }
            else
            {
                Attack();
            }
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            knockbackCounter -= Time.deltaTime;
        }
        
        

        
        velocity.y += gravity * Time.deltaTime;
        
        
        
        anim.SetFloat("z", z);
        anim.SetFloat("x", x);
        anim.SetBool("fly", isGrounded);
        anim.SetBool("shield", Input.GetMouseButton(1));
        anim.SetFloat("knockback",knockbackCounter);

    }

    void Attack()
    {
        // 공격할 조건만 플레이어에 두고, 공격로직은 무기에 위임한다.
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(Input.GetMouseButtonDown(0) && isFireReady) {
            equipWeapon.Use();
            anim.SetTrigger("attack");
            fireDelay = 0;
        }
    
    }

    void ShieldAttack()
    {
        // 공격할 조건만 플레이어에 두고, 공격로직은 무기에 위임한다.
        if (Shleid == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = Shleid.rate < fireDelay;

        if(Input.GetMouseButtonDown(0) && isFireReady) {
            Shleid.Use();
            anim.SetTrigger("attack");
            fireDelay = 0;
        }
    
    }
    void JumpAttack()
    {
        // 공격할 조건만 플레이어에 두고, 공격로직은 무기에 위임한다.
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(Input.GetMouseButtonDown(0) && isFireReady) {
            equipWeapon.Use();
            anim.SetTrigger("attack");
            de = true;
            
               
        }
    
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.layer == 3 && other.gameObject.tag == "Pc")
        {
            knockback();
        }
        if (other.gameObject.layer == 3 && other.gameObject.tag == "Agent")
        {
            knockback();
        }

    }
    private void knockback()
    {
        controller.Move(em.transform.forward * knockbackForce * sh * Time.deltaTime);
        knockbackCounter = knockbackTime;
    }
    




}
