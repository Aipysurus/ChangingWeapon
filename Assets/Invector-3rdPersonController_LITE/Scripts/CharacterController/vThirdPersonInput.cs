using UnityEngine;

namespace Invector.vCharacterController
{
    public class vThirdPersonInput : MonoBehaviour
    {
        #region Variables       

        [Header("Controller Input")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;
        public KeyCode lockInput = KeyCode.Tab;
        public KeyCode dodgeInput = KeyCode.LeftControl;

        public KeyCode attackInput1 = KeyCode.Mouse0;
        public KeyCode attackInput2 = KeyCode.Mouse1;

        [Header("Camera Input")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        [HideInInspector] public vThirdPersonController cc;
        [HideInInspector] public vThirdPersonCamera tpCamera;
        [HideInInspector] public Camera cameraMain;

        public float attackWindow = 0.4f;
        private float attackDelay = 0f;

        private bool canMove = true;




        //Boss Transform
        public Transform bossTransform;
        public bool locked = false;
        #endregion

        protected virtual void Start()
        {
            InitilizeController();
            InitializeTpCamera();
        }

        protected virtual void FixedUpdate()
        {
            cc.UpdateMotor();               // updates the ThirdPersonMotor methods
            cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
            cc.ControlRotationType();       // handle the controller rotation type
        }

        protected virtual void Update()
        {
            InputHandle();                  // update the input methods
            cc.UpdateAnimator();            // updates the Animator Parameters
        }

        public virtual void OnAnimatorMove()
        {
            cc.ControlAnimatorRootMotion(); // handle root motion animations 
        }

        #region Basic Locomotion Inputs

        protected virtual void InitilizeController()
        {
            cc = GetComponent<vThirdPersonController>();

            if (cc != null)
                cc.Init();
        }

        protected virtual void InitializeTpCamera()
        {
            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }
        }

        protected virtual void InputHandle()
        {
            if (canMove) { 
                MoveInput();    
            }
            else
            {
                StopMoving();
            }
            CameraInput();
            SprintInput();
            JumpInput();
            StrafeInput();
            DodgeInput();
            AttackInput();
        }

        public virtual void StopMoving()
        {
            cc.input.x = 0f;
            cc.input.z = 0f;
        }

        public virtual void MoveInput()
        {
            cc.input.x = Input.GetAxis(horizontalInput);
            cc.input.z = Input.GetAxis(verticallInput);
        }

        public virtual void LockInput()
        {
            if (Input.GetKeyDown(lockInput))
            {
                if (!locked)
                {
                    tpCamera.SetTarget(bossTransform);
                }
                else
                {
                    tpCamera.SetTarget(null);
                }
            }
        }

        protected virtual void CameraInput()
        {
            if (!cameraMain)
            {
                if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                else
                {
                    cameraMain = Camera.main;
                    cc.rotateTarget = cameraMain.transform;
                }
            }

            if (cameraMain)
            {
                cc.UpdateMoveDirection(cameraMain.transform);
            }

            if (tpCamera == null)
                return;

            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

            tpCamera.RotateCamera(X, Y);
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        /// <summary>
        /// Conditions to trigger the Jump animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool JumpConditions()
        {
            return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove;
        }

        /// <summary>
        /// Input to trigger the Jump 
        /// </summary>
        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput) && JumpConditions())
                cc.Jump();
        }


        /// <summary>
        /// Input to trigger the Dodge 
        /// </summary>
        protected virtual void DodgeInput()
        {

            if (Input.GetKeyDown(dodgeInput) && cc.isGrounded)
                cc.Dodge();
        }


        /// <summary>
        /// Input to trigger the Attacks 
        /// </summary>
        protected virtual void AttackInput()
        {
            //Case for start attacking
            if (Input.GetKeyDown(attackInput1) && cc.isGrounded && attackDelay <= 0)
            {
                canMove = false;
                cc.Attack("Left");
                attackDelay = attackWindow;

            }
            else if (Input.GetKeyDown(attackInput2) && cc.isGrounded && attackDelay <= 0)
            {
                canMove = false;
                cc.Attack("Right");
                attackDelay = attackWindow;
            }
            //Case for combo delay
            else
            {
                attackDelay -= Time.deltaTime;
                if (attackDelay <= 0)
                {
                    canMove = true;
                    cc.DisableWeapons();
                }
            }

        }

        #endregion       
    }
}