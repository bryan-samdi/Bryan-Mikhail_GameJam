using UnityEngine;
using NaughtyAttributes;
namespace VHS
{    
    public class InputHandler : MonoBehaviour
    {
            [BoxGroup("Input Data")]
            public CameraInputData cameraInputData = null;
            [BoxGroup("Input Data")]
            public MovementInputData movementInputData = null;
            [BoxGroup("Input Data")]
            public InteractionInputData interactionInputData = null;


            void Start()
            {
                cameraInputData.ResetInput();
                movementInputData.ResetInput();
                interactionInputData.ResetInput();
            }

            void Update()
            {
                GetCameraInput();
                GetMovementInputData();
                GetInteractionInputData();
            }

            void GetInteractionInputData()
            {
                interactionInputData.InteractedClicked = Input.GetKeyDown(KeyCode.E);
                //Debug.Log("E CLICKED" + interactionInputData.InteractedClicked);
                interactionInputData.InteractedReleased = Input.GetKeyUp(KeyCode.E);
                //Debug.Log("E RELEASED" + interactionInputData.InteractedReleased);

            }

        void GetCameraInput()
            {
                cameraInputData.InputVectorX = Input.GetAxis("Mouse X");
                cameraInputData.InputVectorY = Input.GetAxis("Mouse Y");

                cameraInputData.ZoomClicked = Input.GetMouseButtonDown(1);
                cameraInputData.ZoomReleased = Input.GetMouseButtonUp(1);
            }

            void GetMovementInputData()
            {
                movementInputData.InputVectorX = Input.GetAxisRaw("Horizontal");
                movementInputData.InputVectorY = Input.GetAxisRaw("Vertical");

                movementInputData.RunClicked = Input.GetKeyDown(KeyCode.LeftShift);
                movementInputData.RunReleased = Input.GetKeyUp(KeyCode.LeftShift);

                if(movementInputData.RunClicked)
                    movementInputData.IsRunning = true;

                if(movementInputData.RunReleased)
                    movementInputData.IsRunning = false;

                movementInputData.JumpClicked = Input.GetKeyDown(KeyCode.Space);
                movementInputData.CrouchClicked = Input.GetKeyDown(KeyCode.LeftControl);
            }
    }
}