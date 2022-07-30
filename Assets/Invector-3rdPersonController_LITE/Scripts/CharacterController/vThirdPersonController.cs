using UnityEngine;

namespace Invector.vCharacterController



{
    public class vThirdPersonController : vThirdPersonAnimator
    {

        public GameObject broadsword1;
        public GameObject broadsword2;
        public GameObject greatsword1;
        public GameObject spear1;
        public GameObject spear2;
        public GameObject weapon1;
        public GameObject weapon2;

        public virtual void ControlAnimatorRootMotion()
        {


            if (!this.enabled) return;

            if (inputSmooth == Vector3.zero)
            {
                transform.position = animator.rootPosition;
                transform.rotation = animator.rootRotation;
            }

            if (useRootMotion)
                MoveCharacter(moveDirection);
        }

        public virtual void ControlLocomotionType()
        {
            if (lockMovement) return;

            if (locomotionType.Equals(LocomotionType.FreeWithStrafe) && !isStrafing || locomotionType.Equals(LocomotionType.OnlyFree))
            {
                SetControllerMoveSpeed(freeSpeed);
                SetAnimatorMoveSpeed(freeSpeed);
            }
            else if (locomotionType.Equals(LocomotionType.OnlyStrafe) || locomotionType.Equals(LocomotionType.FreeWithStrafe) && isStrafing)
            {
                isStrafing = true;
                SetControllerMoveSpeed(strafeSpeed);
                SetAnimatorMoveSpeed(strafeSpeed);
            }

            if (!useRootMotion)
                MoveCharacter(moveDirection);
        }

        public virtual void ControlRotationType()
        {
            if (lockRotation) return;

            bool validInput = input != Vector3.zero || (isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

            if (validInput)
            {
                // calculate input smooth
                inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);

                Vector3 dir = (isStrafing && (!isSprinting || sprintOnlyFree == false) || (freeSpeed.rotateWithCamera && input == Vector3.zero)) && rotateTarget ? rotateTarget.forward : moveDirection;
                RotateToDirection(dir);
            }
        }

        public virtual void UpdateMoveDirection(Transform referenceTransform = null)
        {
            if (input.magnitude <= 0.01)
            {
                moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
                return;
            }

            if (referenceTransform && !rotateByWorld)
            {
                //get the right-facing direction of the referenceTransform
                var right = referenceTransform.right;
                right.y = 0;
                //get the forward direction relative to referenceTransform Right
                var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
                // determine the direction the player will face based on input and the referenceTransform's right and forward directions
                moveDirection = (inputSmooth.x * right) + (inputSmooth.z * forward);
            }
            else
            {
                moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
            }
        }

        public virtual void Sprint(bool value)
        {
            var sprintConditions = (input.sqrMagnitude > 0.1f && isGrounded &&
                !(isStrafing && !strafeSpeed.walkByDefault && (horizontalSpeed >= 0.5 || horizontalSpeed <= -0.5 || verticalSpeed <= 0.1f)));

            if (value && sprintConditions)
            {
                if (input.sqrMagnitude > 0.1f)
                {
                    if (isGrounded && useContinuousSprint)
                    {
                        isSprinting = !isSprinting;
                    }
                    else if (!isSprinting)
                    {
                        isSprinting = true;
                    }
                }
                else if (!useContinuousSprint && isSprinting)
                {
                    isSprinting = false;
                }
            }
            else if (isSprinting)
            {
                isSprinting = false;
            }
        }

        public virtual void Strafe()
        {
            isStrafing = !isStrafing;
        }

        public virtual void Jump()
        {
            // trigger jump behaviour
            jumpCounter = jumpTimer;
            isJumping = true;

            // trigger jump animations
            if (input.sqrMagnitude < 0.1f)
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                animator.CrossFadeInFixedTime("JumpMove", .2f);
        }

        public virtual void Dodge()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("SwordRoll") || animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Attack 1")
                || (animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Attack 2")) || (animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Attack 3")))
            {
                return;
            }
            animator.CrossFadeInFixedTime("SwordRoll", 0.1f);
        }

        public virtual void Attack(string input)
        {
            //Sword Combo
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Attack 1") && input.Equals("Left"))
            {
                broadsword1.SetActive(true);
                animator.CrossFadeInFixedTime("Sword Attack 2", 0.1f);
                return;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Attack 2") && input.Equals("Left"))
            {
                broadsword1.SetActive(false);
                broadsword2.SetActive(true);
                animator.CrossFadeInFixedTime("Sword Attack 3", 0.1f);
                return;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Attack 3") || animator.GetCurrentAnimatorStateInfo(0).IsName("SwordRoll"))
            {
                return;
            }

            //Spear combo
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Attack 1") && input.Equals("Right"))
            {
                spear1.SetActive(true);
                animator.CrossFadeInFixedTime("Spear Attack 2", 0.1f);
                return;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Spear Attack 2") && input.Equals("Right"))
            {
                spear1.SetActive(false);
                spear2.SetActive(true);
                animator.CrossFadeInFixedTime("Spear Attack 3", 0.1f);
                return;
            }

            //Greatsword combo
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Attack 2") && input.Equals("Right"))
            {
                broadsword1.SetActive(false);
                greatsword1.SetActive(true);
                animator.CrossFadeInFixedTime("Greatsword Attack 3", 0.1f);
                return;
            }



            animator.CrossFadeInFixedTime("Sword Attack 1", 0.1f);

        }

        public virtual void DisableWeapons()
        {
            broadsword1.SetActive(false);
            broadsword2.SetActive(false);
            greatsword1.SetActive(false);
            spear1.SetActive(false);
            spear2.SetActive(false);
            //weapon1.SetActive(false);
            //weapon2.SetActive(false);

    }
    }
}