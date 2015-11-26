using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour 
{
    private Animator animatorController;
    //Menu
    private int RunID;
    private int DisposeID;
    private int JumpOnBuildingID;
    //InGame
    private int JumpID;

    public event Action OnMenuAnimationsExit;


    public RuntimeAnimatorController inGameAnimatorController;
    public RuntimeAnimatorController menuAnimatorController;


    void Awake()
    {
        animatorController = this.GetComponent<Animator>();
        if (animatorController == null) throw new MissingReferenceException("Missing a reference to the Animator in the PlayerAnimatorController on the " + this.gameObject.name);

        animatorController.runtimeAnimatorController = menuAnimatorController;
        animatorController.applyRootMotion = true;

        RunID = Animator.StringToHash("Run");
        DisposeID = Animator.StringToHash("Dispose");
        JumpOnBuildingID = Animator.StringToHash("JumpOnBuilding");

        JumpID = Animator.StringToHash("Jump");
    }
    void Start()
    {
        OnMenuAnimatorControllerActivated();
    }
    private void OnMenuAnimatorControllerActivated()
    { 
        var runOverAnimationState = this.animatorController.GetBehaviour<AnimationStateEventDelegator>();

        if (runOverAnimationState == null) throw new MissingReferenceException("Missing a reference to the AnimationStateEventDelegator in the PlayerAnimatorController referencing the animator on " + this.gameObject.name);
        runOverAnimationState.OnExit += () => { if (OnMenuAnimationsExit != null) OnMenuAnimationsExit(); };
        runOverAnimationState.OnExit += () => SwapController(inGameAnimatorController);
    
    }

    public void Run()
    {
        animatorController.SetTrigger(RunID);
    }
    public void JumpOnBuilding()
    {
        animatorController.SetTrigger(JumpOnBuildingID);
    }

    public void JumpFromBuilding()
    {
        animatorController.SetTrigger(DisposeID);
    }
    public void Jump()
    {
        animatorController.SetTrigger(JumpID);
    }

    private void SwapController(RuntimeAnimatorController runtimeAnimatorController)
    {
        animatorController.applyRootMotion = !animatorController.applyRootMotion;
        animatorController.runtimeAnimatorController = runtimeAnimatorController;
    }
    public void Restart()
    {
        SwapController(menuAnimatorController);
        OnMenuAnimatorControllerActivated();
    }

}
