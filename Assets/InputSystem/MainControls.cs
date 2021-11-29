// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem/MainControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MainControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MainControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MainControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""e643b30b-e29c-4654-9c5f-d9f8ae4812a8"",
            ""actions"": [
                {
                    ""name"": ""UpArm"",
                    ""type"": ""Button"",
                    ""id"": ""fcab80f8-4ef8-436a-b9ac-47bdb9f0940f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold,Tap""
                },
                {
                    ""name"": ""RightArm"",
                    ""type"": ""Button"",
                    ""id"": ""f5a59a6b-d516-487f-9f63-03abca681aa4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold,Tap""
                },
                {
                    ""name"": ""DownArm"",
                    ""type"": ""Button"",
                    ""id"": ""1f4c73e5-a746-46f9-8637-62cf654bf789"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold,Tap""
                },
                {
                    ""name"": ""LeftArm"",
                    ""type"": ""Button"",
                    ""id"": ""c155569e-ed1f-4db0-b151-a5ddfb2bbdb6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold,Tap""
                },
                {
                    ""name"": ""Start"",
                    ""type"": ""Button"",
                    ""id"": ""5bb21308-0bf5-47e7-aee5-4e34a0f6c259"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""3f80a269-a7d4-4850-a9d9-a10c48f2dc5a"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""HoldTrigger"",
                    ""type"": ""Button"",
                    ""id"": ""3a74d111-eb37-4a90-aeca-886a30e1b80f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""NextCharacter"",
                    ""type"": ""Button"",
                    ""id"": ""118f1003-80c9-499b-acd7-2f74c7a0cec8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PreviousCharacter"",
                    ""type"": ""Button"",
                    ""id"": ""3598f70b-141f-49d5-90d6-16454bb80950"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cf8247a7-6c30-436e-806b-a9daaa968ffb"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""DownArm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4073af91-45cc-4274-8ff9-7eacfdb3817b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""DownArm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""27f703f3-7d8a-423a-91b4-a857189b3f3c"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""LeftArm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cd87fc36-2e8d-4fa0-ac83-22fd0acc20dd"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""LeftArm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3789a3e-62f8-471b-807a-6fd484c7982a"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""UpArm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e49cd02-69d6-49aa-a8da-c902021d3a27"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""UpArm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8ccc506-8157-4a74-ab97-27ba9ef64671"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""RightArm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f54feb0-79ec-40be-848b-d31ba8423af8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RightArm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1059e33c-9c87-42e7-9eea-f26bc6a54023"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01046c7b-2db7-44fe-a85e-9c30dfe75bc1"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dad54fca-bd90-4a98-9b4f-e2c0bba85f30"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""6c6ca18d-9b4c-4193-909a-351047442834"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""5f6f656b-2ef8-418e-b965-ebe4e5f969db"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""2f6cc838-577f-484b-b3e3-9a54906fabce"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""534f0700-2780-4009-981d-c034a7c19d3a"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""HoldTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a255f327-3013-42fa-bdae-6b073d5cdf1f"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""HoldTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""acff1cbf-f817-454e-a50f-3bf7e1f87232"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""HoldTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d780514e-faa2-4a9a-a265-725f36a7641e"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""NextCharacter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""efe60712-ab48-42bc-8cd3-dbaefa84b42b"",
                    ""path"": ""<Keyboard>/semicolon"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""NextCharacter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5994975f-455c-4845-b11d-994674075f86"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PreviousCharacter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20585c1c-a394-44cc-afb9-d83a1d52f06a"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PreviousCharacter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""44b164b6-b4ef-4384-8ca0-f81b67c6724c"",
            ""actions"": [
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""d98e8f0c-a8f4-4d6e-b850-f6f13a195f6d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""db964beb-1635-4caf-9408-abe3c5d62938"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""aadcdc37-9e6d-4ce8-9811-bde53f46496b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""6a1b0798-a88e-4a9f-bbc3-c6bc2c691022"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Validate"",
                    ""type"": ""Button"",
                    ""id"": ""32eded2b-1eca-45ff-a008-a4c21e0384a9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""54ac8609-4940-47a1-af8f-a19db90a8bb3"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""59f1a83d-eddd-42e8-8d56-c1de80bdf4b1"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af8bb740-b601-4a63-bd0c-c65cfee9ba9c"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8211c8b7-2de0-47f8-a30e-049058ebfa87"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""159f3631-bb70-4051-a3e0-c1420a7be044"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ba2118e-e1a0-4ec5-bb54-a837d875becd"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b1c91d3f-c4ff-4f7e-9fab-d01bf3cc6b2c"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9baa75b5-275b-4268-8fe2-7557a250e2db"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90b40887-e9eb-4a13-acb9-dd21b8a6d298"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Validate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dbda67e9-3e8d-48d6-9409-3f4ee5da67a0"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Validate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""405d2632-f6fe-4baf-a1a6-6b6f77417ae2"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Validate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc70c86c-8e40-4720-9d58-d077b6a244b3"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Validate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_UpArm = m_Gameplay.FindAction("UpArm", throwIfNotFound: true);
        m_Gameplay_RightArm = m_Gameplay.FindAction("RightArm", throwIfNotFound: true);
        m_Gameplay_DownArm = m_Gameplay.FindAction("DownArm", throwIfNotFound: true);
        m_Gameplay_LeftArm = m_Gameplay.FindAction("LeftArm", throwIfNotFound: true);
        m_Gameplay_Start = m_Gameplay.FindAction("Start", throwIfNotFound: true);
        m_Gameplay_Rotate = m_Gameplay.FindAction("Rotate", throwIfNotFound: true);
        m_Gameplay_HoldTrigger = m_Gameplay.FindAction("HoldTrigger", throwIfNotFound: true);
        m_Gameplay_NextCharacter = m_Gameplay.FindAction("NextCharacter", throwIfNotFound: true);
        m_Gameplay_PreviousCharacter = m_Gameplay.FindAction("PreviousCharacter", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Up = m_Menu.FindAction("Up", throwIfNotFound: true);
        m_Menu_Right = m_Menu.FindAction("Right", throwIfNotFound: true);
        m_Menu_Down = m_Menu.FindAction("Down", throwIfNotFound: true);
        m_Menu_Left = m_Menu.FindAction("Left", throwIfNotFound: true);
        m_Menu_Validate = m_Menu.FindAction("Validate", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_UpArm;
    private readonly InputAction m_Gameplay_RightArm;
    private readonly InputAction m_Gameplay_DownArm;
    private readonly InputAction m_Gameplay_LeftArm;
    private readonly InputAction m_Gameplay_Start;
    private readonly InputAction m_Gameplay_Rotate;
    private readonly InputAction m_Gameplay_HoldTrigger;
    private readonly InputAction m_Gameplay_NextCharacter;
    private readonly InputAction m_Gameplay_PreviousCharacter;
    public struct GameplayActions
    {
        private @MainControls m_Wrapper;
        public GameplayActions(@MainControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @UpArm => m_Wrapper.m_Gameplay_UpArm;
        public InputAction @RightArm => m_Wrapper.m_Gameplay_RightArm;
        public InputAction @DownArm => m_Wrapper.m_Gameplay_DownArm;
        public InputAction @LeftArm => m_Wrapper.m_Gameplay_LeftArm;
        public InputAction @Start => m_Wrapper.m_Gameplay_Start;
        public InputAction @Rotate => m_Wrapper.m_Gameplay_Rotate;
        public InputAction @HoldTrigger => m_Wrapper.m_Gameplay_HoldTrigger;
        public InputAction @NextCharacter => m_Wrapper.m_Gameplay_NextCharacter;
        public InputAction @PreviousCharacter => m_Wrapper.m_Gameplay_PreviousCharacter;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @UpArm.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUpArm;
                @UpArm.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUpArm;
                @UpArm.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUpArm;
                @RightArm.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightArm;
                @RightArm.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightArm;
                @RightArm.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightArm;
                @DownArm.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDownArm;
                @DownArm.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDownArm;
                @DownArm.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDownArm;
                @LeftArm.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftArm;
                @LeftArm.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftArm;
                @LeftArm.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftArm;
                @Start.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @Rotate.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRotate;
                @HoldTrigger.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHoldTrigger;
                @HoldTrigger.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHoldTrigger;
                @HoldTrigger.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHoldTrigger;
                @NextCharacter.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNextCharacter;
                @NextCharacter.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNextCharacter;
                @NextCharacter.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNextCharacter;
                @PreviousCharacter.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPreviousCharacter;
                @PreviousCharacter.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPreviousCharacter;
                @PreviousCharacter.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPreviousCharacter;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @UpArm.started += instance.OnUpArm;
                @UpArm.performed += instance.OnUpArm;
                @UpArm.canceled += instance.OnUpArm;
                @RightArm.started += instance.OnRightArm;
                @RightArm.performed += instance.OnRightArm;
                @RightArm.canceled += instance.OnRightArm;
                @DownArm.started += instance.OnDownArm;
                @DownArm.performed += instance.OnDownArm;
                @DownArm.canceled += instance.OnDownArm;
                @LeftArm.started += instance.OnLeftArm;
                @LeftArm.performed += instance.OnLeftArm;
                @LeftArm.canceled += instance.OnLeftArm;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @HoldTrigger.started += instance.OnHoldTrigger;
                @HoldTrigger.performed += instance.OnHoldTrigger;
                @HoldTrigger.canceled += instance.OnHoldTrigger;
                @NextCharacter.started += instance.OnNextCharacter;
                @NextCharacter.performed += instance.OnNextCharacter;
                @NextCharacter.canceled += instance.OnNextCharacter;
                @PreviousCharacter.started += instance.OnPreviousCharacter;
                @PreviousCharacter.performed += instance.OnPreviousCharacter;
                @PreviousCharacter.canceled += instance.OnPreviousCharacter;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Up;
    private readonly InputAction m_Menu_Right;
    private readonly InputAction m_Menu_Down;
    private readonly InputAction m_Menu_Left;
    private readonly InputAction m_Menu_Validate;
    public struct MenuActions
    {
        private @MainControls m_Wrapper;
        public MenuActions(@MainControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Up => m_Wrapper.m_Menu_Up;
        public InputAction @Right => m_Wrapper.m_Menu_Right;
        public InputAction @Down => m_Wrapper.m_Menu_Down;
        public InputAction @Left => m_Wrapper.m_Menu_Left;
        public InputAction @Validate => m_Wrapper.m_Menu_Validate;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Up.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnUp;
                @Up.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnUp;
                @Up.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnUp;
                @Right.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnRight;
                @Right.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnRight;
                @Right.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnRight;
                @Down.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnDown;
                @Down.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnDown;
                @Down.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnDown;
                @Left.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeft;
                @Left.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeft;
                @Left.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeft;
                @Validate.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnValidate;
                @Validate.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnValidate;
                @Validate.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnValidate;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Up.started += instance.OnUp;
                @Up.performed += instance.OnUp;
                @Up.canceled += instance.OnUp;
                @Right.started += instance.OnRight;
                @Right.performed += instance.OnRight;
                @Right.canceled += instance.OnRight;
                @Down.started += instance.OnDown;
                @Down.performed += instance.OnDown;
                @Down.canceled += instance.OnDown;
                @Left.started += instance.OnLeft;
                @Left.performed += instance.OnLeft;
                @Left.canceled += instance.OnLeft;
                @Validate.started += instance.OnValidate;
                @Validate.performed += instance.OnValidate;
                @Validate.canceled += instance.OnValidate;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnUpArm(InputAction.CallbackContext context);
        void OnRightArm(InputAction.CallbackContext context);
        void OnDownArm(InputAction.CallbackContext context);
        void OnLeftArm(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnHoldTrigger(InputAction.CallbackContext context);
        void OnNextCharacter(InputAction.CallbackContext context);
        void OnPreviousCharacter(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnUp(InputAction.CallbackContext context);
        void OnRight(InputAction.CallbackContext context);
        void OnDown(InputAction.CallbackContext context);
        void OnLeft(InputAction.CallbackContext context);
        void OnValidate(InputAction.CallbackContext context);
    }
}
