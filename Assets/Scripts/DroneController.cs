using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{

    enum DroneState
    {
        DRONE_STATE_IDLE,
        DRONE_STATE_START_TAKINGOFF,
        DRONE_STATE_TAKINGOFF,
        DRONE_STATE_MOVING_UP,
        DRONE_STATE_FLYING,
        DRONE_STATE_START_LANDING,
        DRONE_STATE_LANDING,
        DRONE_STATE_LANDED,
        DRONE_STATE_WAIT_ENGINE_STOP,
    }

    DroneState _State;

    Animator _Anim;
    Vector3 _Speed  = new Vector3(0.0f,0.0f,0.0f);
    
    public float _SpeedMultiplyer = 10.0f;


    public bool isIdle()
    {
        return (_State == DroneState.DRONE_STATE_IDLE);
    }
    public void TakeOff()
    {
        _State = DroneState.DRONE_STATE_START_TAKINGOFF;
        
    }

    public bool isFlying()
    {
        return (_State == DroneState.DRONE_STATE_FLYING);
    }
    public void Land()
    {
        _State = DroneState.DRONE_STATE_START_LANDING;
    }


    void Start()
    {
        _Anim = GetComponent<Animator>();
        _State = DroneState.DRONE_STATE_IDLE;
        

    }

    public void Move(float _speedX, float _speedZ){
        _Speed.x = _speedX;
        _Speed.z = _speedZ; 
        UpdateDrone();
       

    }
    void UpdateDrone()
    {


        switch (_State)
        {
            case DroneState.DRONE_STATE_IDLE:
                break;
            case DroneState.DRONE_STATE_START_TAKINGOFF:
                _Anim.SetBool("TakeOff",true);
                _State = DroneState.DRONE_STATE_TAKINGOFF;
                break;
            case DroneState.DRONE_STATE_TAKINGOFF:
                if(_Anim.GetBool("TakeOff")==false)
                {
                    _State = DroneState.DRONE_STATE_MOVING_UP;
                }
                break;
            case DroneState.DRONE_STATE_MOVING_UP:
                if(_Anim.GetBool("MoveUp")== false)
                {
                    _State = DroneState.DRONE_STATE_FLYING;
                }
                break;
            case DroneState.DRONE_STATE_FLYING:
                float angleZ = -30.0f * _Speed.x * 50.0f * Time.deltaTime;
                float angleX = 30.0f * _Speed.z * 50.0f * Time.deltaTime;

                Vector3 rotation = transform.localRotation.eulerAngles;
                transform.localPosition +=_Speed * _SpeedMultiplyer* Time.deltaTime;
                transform.localRotation = Quaternion.Euler(angleX,rotation.y,angleZ);
                break;
            case DroneState.DRONE_STATE_START_LANDING:
                _Anim.SetBool("MoveDown",true);
                _State = DroneState.DRONE_STATE_LANDING;
                break;

            case DroneState.DRONE_STATE_LANDING:
                if(_Anim.GetBool("MoveDown") ==false)
                {
                    _State = DroneState.DRONE_STATE_LANDED;
                }
                break;
            case DroneState.DRONE_STATE_LANDED:
                _Anim.SetBool("Land",true);
                _State = DroneState.DRONE_STATE_WAIT_ENGINE_STOP;
                break;
            case DroneState.DRONE_STATE_WAIT_ENGINE_STOP:
                if(_Anim.GetBool("Land")==false)
                {
                    _State = DroneState.DRONE_STATE_IDLE;
                }
                break;
        }

     }
}
