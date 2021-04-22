// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputAction/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Ground"",
            ""id"": ""6c854398-15ce-4750-8cf3-07a42d6e246f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""991cd16d-deb6-4d2b-81da-2e125a18b1a2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""Value"",
                    ""id"": ""827484eb-d62d-46a8-a0de-aed6b920ff8a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Value"",
                    ""id"": ""92ca1554-eefa-4747-ab7e-fd1e822a29de"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CursorPosition"",
                    ""type"": ""Value"",
                    ""id"": ""4d31b998-47a3-43d2-a362-12179cb9de02"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CursorDelta"",
                    ""type"": ""Value"",
                    ""id"": ""b6b343a3-90d9-4bdd-a91d-ffecfd9df545"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hotbar1"",
                    ""type"": ""Button"",
                    ""id"": ""a2b79585-394b-4420-8f11-5ece4feede00"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hotbar2"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4a372bb0-bae7-4a9c-828f-7c4dbc846aa8"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hotbar3"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e6be1264-67f3-41c1-84f5-f8e2cdc83ded"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hotbar4"",
                    ""type"": ""PassThrough"",
                    ""id"": ""364b4740-af37-4d0c-816c-83e52baf6f11"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hotbar5"",
                    ""type"": ""PassThrough"",
                    ""id"": ""21ff7d32-2a7a-4530-ac66-aa490c08dce2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""71913b06-59c4-42ee-b5ff-dfd77d3b90d5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fc1e7a83-abbe-4f6c-ac29-3986364736b7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""26fe42a2-5662-481d-93d6-46d467d6c3ec"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1652142b-9f70-463f-ba83-14e95d28745d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""585a8fe2-df76-4a90-9de0-3207b88ccf3a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""201857c2-b572-4c7b-bd21-82b405f7ed3e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""75689cca-256c-4ca4-ba29-5f3f4d634114"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9a686a2f-e164-4f09-8f47-a4b6dae81df3"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ce7efbc1-095c-42a1-a3d9-a1d05789eb96"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""39196142-76b0-4faa-94fb-3a8b626c4ae4"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3ae0a0bc-b96c-4704-a753-3bb2a8ecdccf"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bcde9af-02d4-4d0e-a5fd-9ae9001840c5"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""5a4f75f1-e980-4564-92e6-efe90d08d9f5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""dd3c54c1-5961-43c6-8896-94c04b7ea4cb"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""783e46b9-e578-49af-982d-27fed54d8fae"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0122e279-b3ec-4018-98a6-a67b44c7215f"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e6163a0f-4c9c-4093-a19e-37d3a94b4dcc"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller"",
                    ""id"": ""4acc982b-c604-4236-8f84-7607a146e6b1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""346f48f8-f5c8-4a43-ae8c-464bd49b8db7"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""30f30c09-313b-43bb-90cb-2e4432bfc761"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""505d4958-1634-4a68-bbf2-29c14ad93087"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1e314544-9c3e-4c10-9465-256e75eea6a3"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""646ad07f-c0b0-4850-9016-7a3f2101af30"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8076dc1f-31ba-43ca-8b3a-2b79f2c71f4c"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""edb7375f-3200-4704-be6d-5c70b0622eee"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f589f58-773d-4799-8393-56fc167eeb26"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""972b1039-889f-4437-b88d-48db7c87368b"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hotbar1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1dde90c-93bf-4a96-9ca6-9fc0f70dd31a"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Hotbar2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14cb0842-741b-4fa2-b9bd-7c254ba35fd1"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Hotbar3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2eaedb5b-3740-42d8-9a31-2a234e19b9d8"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Hotbar4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42ce0e3e-86ce-46a3-a9a6-9b8db8f23d7f"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Hotbar5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard And Mouse"",
            ""bindingGroup"": ""Keyboard And Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Ground
        m_Ground = asset.FindActionMap("Ground", throwIfNotFound: true);
        m_Ground_Move = m_Ground.FindAction("Move", throwIfNotFound: true);
        m_Ground_Camera = m_Ground.FindAction("Camera", throwIfNotFound: true);
        m_Ground_Roll = m_Ground.FindAction("Roll", throwIfNotFound: true);
        m_Ground_CursorPosition = m_Ground.FindAction("CursorPosition", throwIfNotFound: true);
        m_Ground_CursorDelta = m_Ground.FindAction("CursorDelta", throwIfNotFound: true);
        m_Ground_Hotbar1 = m_Ground.FindAction("Hotbar1", throwIfNotFound: true);
        m_Ground_Hotbar2 = m_Ground.FindAction("Hotbar2", throwIfNotFound: true);
        m_Ground_Hotbar3 = m_Ground.FindAction("Hotbar3", throwIfNotFound: true);
        m_Ground_Hotbar4 = m_Ground.FindAction("Hotbar4", throwIfNotFound: true);
        m_Ground_Hotbar5 = m_Ground.FindAction("Hotbar5", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Ground
    private readonly InputActionMap m_Ground;
    private IGroundActions m_GroundActionsCallbackInterface;
    private readonly InputAction m_Ground_Move;
    private readonly InputAction m_Ground_Camera;
    private readonly InputAction m_Ground_Roll;
    private readonly InputAction m_Ground_CursorPosition;
    private readonly InputAction m_Ground_CursorDelta;
    private readonly InputAction m_Ground_Hotbar1;
    private readonly InputAction m_Ground_Hotbar2;
    private readonly InputAction m_Ground_Hotbar3;
    private readonly InputAction m_Ground_Hotbar4;
    private readonly InputAction m_Ground_Hotbar5;
    public struct GroundActions
    {
        private @PlayerControls m_Wrapper;
        public GroundActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Ground_Move;
        public InputAction @Camera => m_Wrapper.m_Ground_Camera;
        public InputAction @Roll => m_Wrapper.m_Ground_Roll;
        public InputAction @CursorPosition => m_Wrapper.m_Ground_CursorPosition;
        public InputAction @CursorDelta => m_Wrapper.m_Ground_CursorDelta;
        public InputAction @Hotbar1 => m_Wrapper.m_Ground_Hotbar1;
        public InputAction @Hotbar2 => m_Wrapper.m_Ground_Hotbar2;
        public InputAction @Hotbar3 => m_Wrapper.m_Ground_Hotbar3;
        public InputAction @Hotbar4 => m_Wrapper.m_Ground_Hotbar4;
        public InputAction @Hotbar5 => m_Wrapper.m_Ground_Hotbar5;
        public InputActionMap Get() { return m_Wrapper.m_Ground; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GroundActions set) { return set.Get(); }
        public void SetCallbacks(IGroundActions instance)
        {
            if (m_Wrapper.m_GroundActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnMove;
                @Camera.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnCamera;
                @Roll.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnRoll;
                @CursorPosition.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnCursorPosition;
                @CursorPosition.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnCursorPosition;
                @CursorPosition.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnCursorPosition;
                @CursorDelta.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnCursorDelta;
                @CursorDelta.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnCursorDelta;
                @CursorDelta.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnCursorDelta;
                @Hotbar1.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar1;
                @Hotbar1.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar1;
                @Hotbar1.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar1;
                @Hotbar2.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar2;
                @Hotbar2.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar2;
                @Hotbar2.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar2;
                @Hotbar3.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar3;
                @Hotbar3.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar3;
                @Hotbar3.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar3;
                @Hotbar4.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar4;
                @Hotbar4.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar4;
                @Hotbar4.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar4;
                @Hotbar5.started -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar5;
                @Hotbar5.performed -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar5;
                @Hotbar5.canceled -= m_Wrapper.m_GroundActionsCallbackInterface.OnHotbar5;
            }
            m_Wrapper.m_GroundActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @CursorPosition.started += instance.OnCursorPosition;
                @CursorPosition.performed += instance.OnCursorPosition;
                @CursorPosition.canceled += instance.OnCursorPosition;
                @CursorDelta.started += instance.OnCursorDelta;
                @CursorDelta.performed += instance.OnCursorDelta;
                @CursorDelta.canceled += instance.OnCursorDelta;
                @Hotbar1.started += instance.OnHotbar1;
                @Hotbar1.performed += instance.OnHotbar1;
                @Hotbar1.canceled += instance.OnHotbar1;
                @Hotbar2.started += instance.OnHotbar2;
                @Hotbar2.performed += instance.OnHotbar2;
                @Hotbar2.canceled += instance.OnHotbar2;
                @Hotbar3.started += instance.OnHotbar3;
                @Hotbar3.performed += instance.OnHotbar3;
                @Hotbar3.canceled += instance.OnHotbar3;
                @Hotbar4.started += instance.OnHotbar4;
                @Hotbar4.performed += instance.OnHotbar4;
                @Hotbar4.canceled += instance.OnHotbar4;
                @Hotbar5.started += instance.OnHotbar5;
                @Hotbar5.performed += instance.OnHotbar5;
                @Hotbar5.canceled += instance.OnHotbar5;
            }
        }
    }
    public GroundActions @Ground => new GroundActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_KeyboardAndMouseSchemeIndex = -1;
    public InputControlScheme KeyboardAndMouseScheme
    {
        get
        {
            if (m_KeyboardAndMouseSchemeIndex == -1) m_KeyboardAndMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard And Mouse");
            return asset.controlSchemes[m_KeyboardAndMouseSchemeIndex];
        }
    }
    public interface IGroundActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnCursorPosition(InputAction.CallbackContext context);
        void OnCursorDelta(InputAction.CallbackContext context);
        void OnHotbar1(InputAction.CallbackContext context);
        void OnHotbar2(InputAction.CallbackContext context);
        void OnHotbar3(InputAction.CallbackContext context);
        void OnHotbar4(InputAction.CallbackContext context);
        void OnHotbar5(InputAction.CallbackContext context);
    }
}
