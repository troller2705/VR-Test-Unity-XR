using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerHandsManager : MonoBehaviour
{
    public InputActionReference triggerActionReference;
    public InputActionReference gripActionReference;

    public Animator handAnimator;

    private void Awake()
    {
        handAnimator = GetComponent<Animator>();
        SetupInputActions();
    }

    private void SetupInputActions()
    {
        if(triggerActionReference!=null && gripActionReference!=null)
        {
            triggerActionReference.action.performed += ctx => UpdateHandAnimation("Trigger", ctx.ReadValue<float>());
            triggerActionReference.action.canceled += ctx => UpdateHandAnimation("Trigger", 0);

            gripActionReference.action.performed += ctx => UpdateHandAnimation("Grip", ctx.ReadValue<float>());
            gripActionReference.action.canceled += ctx => UpdateHandAnimation("Grip", 0);
        }
        else
        {
            Debug.LogWarning("Input action references are not set in the inspector");
        }
    }

    private void UpdateHandAnimation(string parameterName, float value)
    {
        if(handAnimator!=null)
        {
            handAnimator.SetFloat(parameterName, value);
        }
    }

    private void OnEnable()
    {
        triggerActionReference?.action.Enable();
        gripActionReference?.action.Enable();
    }

    private void OnDisable()
    {
        triggerActionReference?.action.Disable();
        gripActionReference?.action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
