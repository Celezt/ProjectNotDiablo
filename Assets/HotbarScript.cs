
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarScript : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerControls controller;
    private void Awake()
    {
        controller = new PlayerControls();
    }

    private void OnEnable()
    {
        controller.Enable();
        controller.Ground.Hotbar1.performed += Test;
        
        
    }

    private void OnDisable()
    {
        controller.Ground.Hotbar1.performed -= Test;
       
        controller.Disable();
    }
    public void Test(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Hello");
        }
    }
}
