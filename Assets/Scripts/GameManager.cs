using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public DroneController _DroneController;

    public Button _FlyButton;
    public Button _LandButton;

    public GameObject _Controls;


    public ARRaycastManager _RaycastManager;
    public ARPlaneManager _PlaneManager;
    List<ARRaycastHit> _HItResult = new List<ARRaycastHit>();

    public GameObject _Drone;

    struct DroneAnimationControls
    {
        public bool _moving;
        public bool _interploatingAsc;
        public bool _interploatingDesc;
        public float _axis;
        public float _direction;
    }

    DroneAnimationControls _MovingLeft;
    DroneAnimationControls _MovingBack;

    void Start()
    {
        _FlyButton.onClick.AddListener(EventOnClickFlyButton);
        _LandButton.onClick.AddListener(EventOnClickLandButton);
    }

    // Update is called once per frame
    void Update()
    {
        //float speedX = Input.GetAxis("Horizontal");
        //float speedZ = Input.GetAxis("Vertical");

        UpdateControls(ref _MovingLeft);
        UpdateControls(ref _MovingBack);

        _DroneController.Move(_MovingLeft._axis * _MovingLeft._direction, _MovingBack._axis * _MovingBack._direction);

        if(_DroneController.isIdle())
        {
            UpdateAr();
        }
  
    }

    void UpdateAr()
    {
        Vector2 positionScreenSpace = Camera.current.ViewportToScreenPoint(new Vector2(0.5f,0.5f));
        _RaycastManager.Raycast(positionScreenSpace, _HItResult, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds);

        if(_HItResult.Count > 0)
        {
            if(_PlaneManager.GetPlane(_HItResult[0].trackableId).alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            {
                Pose pose = _HItResult[0].pose;
                _Drone.transform.position = pose.position;
                _Drone.SetActive(true);
            }
        }
    }

    void UpdateControls(ref DroneAnimationControls _controls)
    {
        if( _controls._moving || _controls._interploatingAsc || _controls._interploatingDesc)
        {
            if(_controls._interploatingAsc)
            {
                _controls._axis += 0.05f;
                if(_controls._axis >= 1.0f)
                {
                    _controls._axis = 1.0f;
                    _controls._interploatingAsc = false;
                    _controls._interploatingDesc = true;
                }
            }
            
            else if(!_controls._moving)
            {
                _controls._axis -= 0.05f;
                if(_controls._axis <= 0.0f)
                {
                    _controls._axis = 0.0f;
                    _controls._interploatingDesc = false;
                }
            }
        }
    }



    void EventOnClickFlyButton()
    {
        if(_DroneController.isIdle())
        {
            _DroneController.TakeOff();
            _FlyButton.gameObject.SetActive(false);
            _Controls.SetActive(true);
        }
    }
    void EventOnClickLandButton()
    {
        if(_DroneController.isFlying())
        {
            _DroneController.Land();
            _Controls.SetActive(false);
            _FlyButton.gameObject.SetActive(true);
            
        }
    }
/////////////////          Left Button           ////////////////////////
    public void EventOnLeftButtonPressed()
    {
        _MovingLeft._moving = true;
        _MovingLeft._interploatingAsc = true;
        _MovingLeft._direction = -1.0f;

    }
    public void EventOnLeftButtonReleased()
    {
        _MovingLeft._moving = false;
    }
//////////////             //Right Button             //////////
    public void EventOnRightButtonPressed()
    {
        _MovingLeft._moving = true;
        _MovingLeft._interploatingAsc = true;
        _MovingLeft._direction = 1.0f;
    }
    public void EventOnRightButtonReleased()
    {
        _MovingLeft._moving = false;
    }

///////////////////             Back Button             ////////////////////
    public void EventOnBackButtonPressed()
    {
        _MovingBack._moving = true;
        _MovingBack._interploatingAsc = true;
        _MovingBack._direction = -1.0f;
    }
    public void EventOnBackButtonReleased()
    {
        _MovingBack._moving = false;
    }
////////////////////          Forward Button              ////////////////////
    public void EventOnForwardButtonPressed()
    {
        _MovingBack._moving = true;
        _MovingBack._interploatingAsc = true;
        _MovingBack._direction = 1.0f;
    }
    public void EventOnForwardButtonReleased()
    {
        _MovingBack._moving = false;
    }

}
