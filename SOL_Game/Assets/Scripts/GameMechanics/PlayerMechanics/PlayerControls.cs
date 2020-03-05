// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/GameMechanics/PlayerMechanics/PlayerControls.inputactions'

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
            ""name"": ""Gameplay"",
            ""id"": ""e71873ba-ed06-41bb-8779-ec7a8595f065"",
            ""actions"": [
                {
                    ""name"": ""SwordAttack"",
                    ""type"": ""Button"",
                    ""id"": ""cc73b755-0e61-41c6-80a3-f0ac4d18b43b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HammerAttack"",
                    ""type"": ""Button"",
                    ""id"": ""f01d7439-2bef-4092-a63e-2efd19e7977e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BlasterAttack"",
                    ""type"": ""Button"",
                    ""id"": ""73972ce3-a721-414f-bceb-e509f00d6c9c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShieldDefense"",
                    ""type"": ""Button"",
                    ""id"": ""db12c0bb-7d37-4e23-abe5-cf718e115604"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""123d9d0d-70aa-45ab-bce8-2c944b8a29d9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5e0a26e7-3437-4973-8364-50492575c008"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwordAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca45ae29-f94a-4ed0-958c-8b40ccecec87"",
                    ""path"": ""<HID::USB Gamepad >/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwordAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85207b15-ec2e-471e-bcb5-28b8919018a8"",
                    ""path"": ""<Joystick>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HammerAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""150f87b6-420e-48c8-b8a2-e63b08c224e9"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HammerAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c5f4269-2637-4af7-8bb4-d218eef5f8b2"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BlasterAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""97444f23-de13-46b2-aee0-69705c03c026"",
                    ""path"": ""<HID::USB Gamepad >/button4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BlasterAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc9d1253-2a09-462f-b13b-eb78d2d60675"",
                    ""path"": ""<HID::USB Gamepad >/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShieldDefense"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6f1b05cb-c372-48d3-8d07-4003447a6a60"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShieldDefense"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""wasdMovement"",
                    ""id"": ""aa31dcfe-aab1-404f-834a-24e233c0cfb2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""49805f3d-2cf2-458e-8c08-7c4838d61dc8"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b762cac6-30f1-4465-b7dc-031ede03e0cf"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ebb3f412-046a-4d8a-a441-e5029f6023ac"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""190cc1b8-6931-4691-8cbf-771217c0c3e2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ControlerDPad"",
                    ""id"": ""00a48cd8-f57f-47e8-9a6c-68fe008baa10"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fa92e3b1-8b57-44a3-b864-0d08b844e40f"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9f6a5644-87ea-4642-a758-bec7b77382dd"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b9375814-17a7-4314-be43-23d00dd7f5a4"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bffcf479-3e05-46ae-aa09-0b7c63829942"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_SwordAttack = m_Gameplay.FindAction("SwordAttack", throwIfNotFound: true);
        m_Gameplay_HammerAttack = m_Gameplay.FindAction("HammerAttack", throwIfNotFound: true);
        m_Gameplay_BlasterAttack = m_Gameplay.FindAction("BlasterAttack", throwIfNotFound: true);
        m_Gameplay_ShieldDefense = m_Gameplay.FindAction("ShieldDefense", throwIfNotFound: true);
        m_Gameplay_Movement = m_Gameplay.FindAction("Movement", throwIfNotFound: true);
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
    private readonly InputAction m_Gameplay_SwordAttack;
    private readonly InputAction m_Gameplay_HammerAttack;
    private readonly InputAction m_Gameplay_BlasterAttack;
    private readonly InputAction m_Gameplay_ShieldDefense;
    private readonly InputAction m_Gameplay_Movement;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @SwordAttack => m_Wrapper.m_Gameplay_SwordAttack;
        public InputAction @HammerAttack => m_Wrapper.m_Gameplay_HammerAttack;
        public InputAction @BlasterAttack => m_Wrapper.m_Gameplay_BlasterAttack;
        public InputAction @ShieldDefense => m_Wrapper.m_Gameplay_ShieldDefense;
        public InputAction @Movement => m_Wrapper.m_Gameplay_Movement;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @SwordAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwordAttack;
                @SwordAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwordAttack;
                @SwordAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwordAttack;
                @HammerAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHammerAttack;
                @HammerAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHammerAttack;
                @HammerAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHammerAttack;
                @BlasterAttack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlasterAttack;
                @BlasterAttack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlasterAttack;
                @BlasterAttack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlasterAttack;
                @ShieldDefense.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShieldDefense;
                @ShieldDefense.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShieldDefense;
                @ShieldDefense.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShieldDefense;
                @Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SwordAttack.started += instance.OnSwordAttack;
                @SwordAttack.performed += instance.OnSwordAttack;
                @SwordAttack.canceled += instance.OnSwordAttack;
                @HammerAttack.started += instance.OnHammerAttack;
                @HammerAttack.performed += instance.OnHammerAttack;
                @HammerAttack.canceled += instance.OnHammerAttack;
                @BlasterAttack.started += instance.OnBlasterAttack;
                @BlasterAttack.performed += instance.OnBlasterAttack;
                @BlasterAttack.canceled += instance.OnBlasterAttack;
                @ShieldDefense.started += instance.OnShieldDefense;
                @ShieldDefense.performed += instance.OnShieldDefense;
                @ShieldDefense.canceled += instance.OnShieldDefense;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnSwordAttack(InputAction.CallbackContext context);
        void OnHammerAttack(InputAction.CallbackContext context);
        void OnBlasterAttack(InputAction.CallbackContext context);
        void OnShieldDefense(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
    }
}
