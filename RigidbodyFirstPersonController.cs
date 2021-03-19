using System;
using System.Collections;
using UnityEngine;









namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (CapsuleCollider))]
    public class RigidbodyFirstPersonController : MonoBehaviour
    {

	
        [Serializable]
        public class MovementSettings
        {
			
            public float ForwardSpeed = 8.0f;   // Speed when walking forward
            public float BackwardSpeed = 4.0f;  // Speed when walking backwards
            public float StrafeSpeed = 4.0f;    // Speed when walking sideways
      
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
             public float CurrentTargetSpeed = 8f;

#if !MOBILE_INPUT
            private bool m_Running;
#endif

            public void UpdateDesiredTargetSpeed(Vector2 input)
            {
	           


				if (input == Vector2.zero) return;
				if (input.x > 0.3 || input.x < -0.3)
				{
					//strafe
					CurrentTargetSpeed = StrafeSpeed;
				}
				if (input.y < 0 )
				{
					//backwards
					CurrentTargetSpeed = BackwardSpeed  ;
				}
				if (input.y > 0)
				{
					//forwards
					//handled last as if strafing and moving forward at the same time forwards speed should take precedence
					CurrentTargetSpeed = ForwardSpeed ;
				}
#if !MOBILE_INPUT
	          
#endif
            }

#if !MOBILE_INPUT
            public bool Running
            {
                get { return m_Running; }
            }
#endif
        }

        [Serializable]
        public class AdvancedSettings
        {

            public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
            public float stickToGroundHelperDistance = 0.5f; // stops the character
            public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
            public bool airControl; // can the user control the direction that is being moved in the air
            [Tooltip("set it to 0.1 or more if you get stuck in wall")]
            public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
        }

	    public Vector3 relativeMovement;
        public Vector3 relativeMovementUpdate;
        public Vector3 relativeTransform;

        public Transform cam;
        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public AdvancedSettings advancedSettings = new AdvancedSettings();

        public float Aftergroundedtimer = 0.2f;
        private float aftergrounded;

  

        public DetectObstruction DetectGround;

        public bool IsTitan;
        public Player player;


        private Rigidbody m_RigidBody;
        private CapsuleCollider m_Capsule;
        private float m_YRotation;
        private Vector3 m_GroundContactNormal;
        private bool  m_PreviouslyGrounded, m_Jumping, m_IsGrounded;
		public bool m_Jump;
		public bool canrotate = true;
		public bool rotatecamonly;
		public bool camgoback;
        public bool CanMove = true;


        public bool RoofStop;


        public bool Rolling;


        public bool IAction;
		public bool ishit;
		public bool isatk;
        private Vector3 lastpos;
        public Vector3 Velocity
        {
            get { return m_RigidBody.velocity; }
        }

        public bool Grounded
        {
            get { return m_IsGrounded; }
        }

        public bool Jumping
        {
            get { return m_Jumping; }
        }

        public bool Running
        {
            get
            {
 #if !MOBILE_INPUT
				return movementSettings.Running;
#else
	            return false;
#endif
            }
        }


        private void Start()
        {
            InvokeRepeating("LastPos", 0f, 0.1f);

            m_RigidBody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            mouseLook.Init (transform, cam.transform);
            mouseLook.XSensitivity = PlayerPrefs.GetFloat("SensX");
            mouseLook.YSensitivity = PlayerPrefs.GetFloat("SensY");



            InvokeRepeating("Repeat", 0, .1f);

        
        }
        void Repeat()
        {
            relativeMovement = transform.InverseTransformDirection(m_RigidBody.velocity);
        }

        void LastPos()
        {
            lastpos = transform.position;
            Invoke("UpdatePos", 0.05f);
        }
        void UpdatePos()
        {
            relativeTransform = transform.position - lastpos;

        }
        private void LateUpdate()
        {
			if (canrotate ) {
				RotateView ();
			} 
			if(!canrotate)
			
			{
				RotateOveride ();
			}
			if (rotatecamonly) {
				RotateOverideCamOnly ();
			} 



		
        }

		private void RotateView()
		{

		
			transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);


			cam.transform.localRotation = Quaternion.Euler (cam.transform.localRotation.x, 0, 0);



			//avoids the mouse looking if the game is effectively paused
			if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

			// get the rotation before it's changed
			float oldYRotation = transform.eulerAngles.y;

			mouseLook.LookRotation (transform, cam.transform);

			if (m_IsGrounded || advancedSettings.airControl)
			{
				// Rotate the rigidbody velocity to match the new direction that the character is looking
				Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
				m_RigidBody.velocity = velRotation*m_RigidBody.velocity;
			}
		}

		private void RotateOveride()
		{
			if (!rotatecamonly) {
				mouseLook.LookRotationOverride (transform, cam.transform);
			}
	

		}

		private void RotateOverideCamOnly()
		{
			mouseLook.LookRotationOverideCam (transform, cam.transform);

		}

		public void CamGoBack(float speed)
		{
			mouseLook.CamGoBack(transform, cam.transform ,speed);

		}


        private void Update()
        {
            GroundCheck();
            RoofCheck();

            if ((ControlFreak2.CF2Input.GetKeyDown(KeyCode.Space) || (ControlFreak2.CF2Input.GetKeyDown(KeyCode.J))) && !m_Jump && !Rolling)
            {
                m_Jump = true;
            }

        }
        private void FixedUpdate()
        {

            relativeMovementUpdate = transform.InverseTransformDirection(m_RigidBody.velocity);
            Vector2 input = GetInput();

           

            float h = input.x;
            float v = input.y;
            Vector3 inputVector = new Vector3(h, 0, v);
            inputVector = Vector3.ClampMagnitude(inputVector, 1);
            if(CanMove && Grounded)
            {
                if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
                {
                    if (!IsTitan)
                    {
                        if (ControlFreak2.CF2Input.GetAxisRaw("Vertical") > 0.3f)
                        {
                            m_RigidBody.AddRelativeForce(0, 0, Time.deltaTime * movementSettings.ForwardSpeed * player.currentSpeed * 0.5f * Mathf.Abs(inputVector.z));
                        }
                        if (ControlFreak2.CF2Input.GetAxisRaw("Vertical") < -0.3f)
                        {
                            m_RigidBody.AddRelativeForce(0, 0, Time.deltaTime * -movementSettings.BackwardSpeed * player.currentSpeed * 0.5f * Mathf.Abs(inputVector.z));
                        }
                        if (ControlFreak2.CF2Input.GetAxisRaw("Horizontal") > 0.5f)
                        {
                            m_RigidBody.AddRelativeForce(Time.deltaTime * movementSettings.StrafeSpeed * player.currentSpeed * 0.5f * Mathf.Abs(inputVector.x), 0, 0);
                        }
                        if (ControlFreak2.CF2Input.GetAxisRaw("Horizontal") < -0.5f)
                        {
                            m_RigidBody.AddRelativeForce(Time.deltaTime * -movementSettings.StrafeSpeed * player.currentSpeed * 0.5f * Mathf.Abs(inputVector.x), 0, 0);
                        }
                    }
                   
                }
            }
            if(m_IsGrounded)
            {
                aftergrounded = Aftergroundedtimer;
            }
            else
            {
                aftergrounded -= Time.deltaTime;
            }

        }


 
        private Vector2 GetInput()
        {
			
            
            Vector2 input = new Vector2

                {
				x =ControlFreak2.CF2Input.GetAxisRaw("Horizontal"),
				y = ControlFreak2.CF2Input.GetAxisRaw("Vertical")
                };


			movementSettings.UpdateDesiredTargetSpeed(input);
            return input;
        }



        /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
        private void GroundCheck()
		{

			m_PreviouslyGrounded = m_IsGrounded;
			if (DetectGround.Obstruction) {
				m_IsGrounded = true;
			} else {
				m_IsGrounded = false;
				m_GroundContactNormal = Vector3.up;
			}
			if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping) {
				m_Jumping = false;
			}

         
        }
        void RoofCheck()
        {
            RoofStop = false;

            Vector2 input = GetInput();
            Ray myRay = new Ray(transform.position, -transform.up); // cast a Ray from the position of our gameObject into our desired direction.

            RaycastHit hit;

            if (Physics.Raycast(myRay, out hit, 1.3f))
            {
                if (hit.collider.CompareTag("Wall")) // Our Ray has hit the ground
                {

                    if (Vector3.Angle(Vector3.up, hit.normal) >= 5f) // Here we get the angle between the Up Vector and the normal of the wall we are checking against: 90 for straight up walls, 0 for flat ground. You can set "steepSlopeAngle" to any angle you wish. 
                    {
                        if (Grounded && !(Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && m_RigidBody.velocity.magnitude < 5f && !ControlFreak2.CF2Input.GetKeyDown(KeyCode.Space))
                        {
                            RoofStop = true;
                        }
                    }
                }

            }
        }


    }
	}

