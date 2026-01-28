using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    PlayerInput playerInput;
    CharacterController characterController;
    public float xRunMultiplier = 3.0f;
    public float zRunMultiplier = 3.0f;
    Vector2 currentMovementInput;
    int isWalkingHash;
    int isRunningHash;
    Vector3 currentMovement;
    public float playerSpeed = 1.5f;
    Vector3 currentRunMov;
    Animator animator;
    bool isMovementPressed;
    bool isRunPressed;
    public float rotationFactorPerFrame = 0.125f;
    void Awake(){
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        
    }
    void gravityHandler(){
        if(characterController.isGrounded){
            float groundedVal = -0.5f;
            currentMovement.y = groundedVal;
            currentRunMov.y = groundedVal;
        }else{
            float goodGrav = -9.8f;
            currentMovement.y = goodGrav;
            currentRunMov.y = goodGrav;
        }
    }
    void onRun(InputAction.CallbackContext context){
            isRunPressed = context.ReadValueAsButton();
    }
    void onMovementInput (InputAction.CallbackContext context){
            currentMovementInput = context.ReadValue<Vector2>();
            currentMovement.x = currentMovementInput.x*playerSpeed;
            currentMovement.z = currentMovementInput.y*playerSpeed;
            currentRunMov.x = currentMovementInput.x*xRunMultiplier;
            currentRunMov.z = currentMovementInput.y*zRunMultiplier;
            isMovementPressed = currentMovementInput.x!=0 || currentMovementInput.y!=0;
    }
    void rotationHandler(){
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        Quaternion currentRotation = transform.rotation;
    
        if(isMovementPressed){
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation,targetRotation,rotationFactorPerFrame*Time.deltaTime);
        }
    }
    void animationHandler(){
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        if(isMovementPressed && !isWalking){
            animator.SetBool(isWalkingHash,true);
        }
        else if(!isMovementPressed && isWalking){
            animator.SetBool(isWalkingHash,false);
        }
        if((isMovementPressed && isRunPressed) && !isRunning){
            animator.SetBool(isRunningHash,true);
        }else if((!isMovementPressed || !isRunPressed) && isRunning){
            animator.SetBool(isRunningHash,false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        gravityHandler();
        animationHandler();
        if(isRunPressed){
            characterController.Move(currentRunMov*Time.deltaTime);
        }else{
            characterController.Move(currentMovement*Time.deltaTime);
        }
        rotationHandler();
    }
    void OnEnable(){
        playerInput.CharacterControls.Enable();
    }
    void OnDisable(){
        playerInput.CharacterControls.Disable();
    }
}
