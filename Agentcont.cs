using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UIElements.Experimental;
using Unity.MLAgents.Policies;
using UnityEngine.UIElements;


[RequireComponent(typeof(DecisionRequester))]
public class Agentcont : Agent
{
    public CharacterController controller;
    public CharacterController Target;
    public GameObject a;
    public Agentcont PM;
    public PlayerMovement PM2;
    public float mouseSensitivity = 100f;
    float act; 
    public float speed = 12f;
    public float backspeed = 5f;
    public float gravity = -9.81f;
    Vector3 velocity;
    public Transform groundCheck;
    public float gorundDistance = 0.4f;
    public LayerMask groundMask;
    bool isFireReady = true;
    public Weapon equipWeapon;
    float fireDelay;
    public bool hit;
    
    public float knockbackForce;
    public float knockbackTime;
    public float knockbackCounter;
    public Animator anim;
    public enum EMENY
    {
        BLUE, RED
    }
    public EMENY team = EMENY.BLUE;
    // Start is called before the first frame update
    float chek;
    private BehaviorParameters bps;

    // 초기위치
    private Vector3 initPosBlue = new Vector3(0f,0f,5f);
    private Vector3 initPosRed = new Vector3(0f,0f,-5f);
    private Quaternion initRotBlue = Quaternion.Euler(Vector3.up * 180.0f);
    private Quaternion initRotRed = Quaternion.Euler(new Vector3(0f,0f,0f));

    void InitPlayer()
    {
        transform.localPosition = (team == EMENY.BLUE) ? initPosBlue : initPosRed;
        transform.localRotation = (team == EMENY.BLUE) ? initRotBlue : initRotRed;

    }
    public override void Initialize()
    {
        bps = GetComponent<BehaviorParameters>();
        MaxStep = 10000;
    }
    
    public override void OnEpisodeBegin()
    {
        InitPlayer();
        

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(controller.transform.position);
        sensor.AddObservation(controller.transform.rotation.y);
        sensor.AddObservation(controller.velocity);
        
        sensor.AddObservation(Target.transform.position);
        sensor.AddObservation(Target.transform.rotation.y);
        sensor.AddObservation(Target.velocity);

    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.ContinuousActions;
        
        bool isGrounded;
        
        
        
        isGrounded = Physics.CheckSphere(groundCheck.position, gorundDistance, groundMask);
        
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            
        }
        float x = action[0];
        float z = action[1];
        Vector3 move = transform.right * x + transform.forward * z;

        float mouseX = action[2];
        transform.Rotate(Vector3.up * mouseX);
        act = action[3];
        
        
        if(knockbackCounter <= 0)
        {
             
            if (z >= 0)
            {
                controller.Move(move * speed * Time.fixedDeltaTime);
            }
            else if (z < 0)
            {
                controller.Move(move * backspeed * Time.fixedDeltaTime);
            }
            Attack();
            controller.Move(velocity * Time.fixedDeltaTime);
        }
        else
        {
            knockbackCounter -= Time.fixedDeltaTime;
            SetReward(-0.005f);
        }
        
        if(Target.transform.position.y < -2f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        if(controller.transform.position.y < -2f)
        {
            SetReward(-1.0f);
            EndEpisode();
        }

        velocity.y += gravity * Time.fixedDeltaTime;
        //Debug.Log(action[3]);

        
        chek = (Mathf.Abs(Target.transform.position.x - controller.transform.position.x) + Mathf.Abs(Target.transform.position.z - controller.transform.position.z))/10;
        if(chek < 0.29 && act > 0.5)
        {
            SetReward(0.1f);
        }
        if(PM.hit)
        {
            SetReward(0.5f);
            PM.hit = false;
        }
       
        
        SetReward((4-chek)/4);
        
        anim.SetFloat("z", z);
        anim.SetFloat("x", x);
        anim.SetBool("fly", isGrounded);
        anim.SetFloat("knockback",knockbackCounter);
        
        // Debug.Log($"[0]={action[0]}, [1] = {action[1]}, [2]={action[2]}, [3]={action[3]}");
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        var action = actionsOut.ContinuousActions;
        action.Clear();
        action[0] = Input.GetAxis("Horizontal"); // z
        action[1] = Input.GetAxis("Vertical");  // x
        action[2] = Input.GetAxis("Mouse X") * Time.deltaTime;
        //forward
        if(Input.GetMouseButtonDown(0))
        {
            action[3] = 1;
        }

    }

    void Attack()
    {
        // 공격할 조건만 플레이어에 두고, 공격로직은 무기에 위임한다.
        if (equipWeapon == null)
            return;
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;
        if(act> 0.5 && isFireReady)
        {
            equipWeapon.Use();
            anim.SetTrigger("attack");
            fireDelay = 0;
            
        }

        

        
        
    
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.layer == 3 && other.gameObject.tag == a.tag)
        {
            knockback();
            hit = true;
        }
        if(other.gameObject.layer == 3 && other.gameObject.tag == "Player")
        {
            knockback();
            hit = true;
        }
        

    }
    private void knockback()
    {
        controller.Move(Target.transform.forward * knockbackForce * Time.deltaTime);
        knockbackCounter = knockbackTime;
    }

    



}
