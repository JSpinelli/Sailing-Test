// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerControls.inputactions'

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
            ""name"": ""Boat"",
            ""id"": ""69d45496-64c5-4f9b-99ea-736b16435621"",
            ""actions"": [
                {
                    ""name"": ""Left Trigger"",
                    ""type"": ""Button"",
                    ""id"": ""030f139d-fca1-47d2-8895-aea6408b10c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Trigger"",
                    ""type"": ""Button"",
                    ""id"": ""82a23630-d86b-4542-beba-2d7d8470cde2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left Stick"",
                    ""type"": ""Value"",
                    ""id"": ""552a1291-4636-4f01-bc5c-c67f9348f599"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Stick"",
                    ""type"": ""Value"",
                    ""id"": ""686b5b8d-bbcc-40eb-8db2-5fa0f28936d7"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Button South"",
                    ""type"": ""Button"",
                    ""id"": ""54a54790-1a6f-4d67-ac18-4cceac1c2dad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Button East"",
                    ""type"": ""Button"",
                    ""id"": ""e6c36b7e-67d4-434c-9a88-91845345db02"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Button North"",
                    ""type"": ""Button"",
                    ""id"": ""46759ebc-627b-4452-936d-2a4c1640aa7b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""b699428b-c925-4574-8b90-337736ed84a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Looking Glass"",
                    ""type"": ""Button"",
                    ""id"": ""f29bbf60-e78a-400c-a101-cff8fcff9b53"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8d869983-60d6-453b-b6a3-334ae01cf33a"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Trigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0bf9c33c-9e87-455f-941f-388fb7bf3edf"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Trigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""776a3d2c-6155-444e-90ab-0ce3abb0fb1e"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""139dbbc1-2b08-434f-9bc6-32ed99438809"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0696d999-7be9-42d8-b23b-84171dd4f453"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button South"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ad0becd-9544-45de-a9f3-25a4a9112883"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button East"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b9f40e9-19bd-4c69-a0f9-6d632f22b7d7"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button North"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d7c6cb1-cf97-4278-8f41-f50dda485737"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9104f0c-d48d-49cb-9fe7-d4e7b94b3b23"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Looking Glass"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Boat
        m_Boat = asset.FindActionMap("Boat", throwIfNotFound: true);
        m_Boat_LeftTrigger = m_Boat.FindAction("Left Trigger", throwIfNotFound: true);
        m_Boat_RightTrigger = m_Boat.FindAction("Right Trigger", throwIfNotFound: true);
        m_Boat_LeftStick = m_Boat.FindAction("Left Stick", throwIfNotFound: true);
        m_Boat_RightStick = m_Boat.FindAction("Right Stick", throwIfNotFound: true);
        m_Boat_ButtonSouth = m_Boat.FindAction("Button South", throwIfNotFound: true);
        m_Boat_ButtonEast = m_Boat.FindAction("Button East", throwIfNotFound: true);
        m_Boat_ButtonNorth = m_Boat.FindAction("Button North", throwIfNotFound: true);
        m_Boat_Interact = m_Boat.FindAction("Interact", throwIfNotFound: true);
        m_Boat_LookingGlass = m_Boat.FindAction("Looking Glass", throwIfNotFound: true);
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

    // Boat
    private readonly InputActionMap m_Boat;
    private IBoatActions m_BoatActionsCallbackInterface;
    private readonly InputAction m_Boat_LeftTrigger;
    private readonly InputAction m_Boat_RightTrigger;
    private readonly InputAction m_Boat_LeftStick;
    private readonly InputAction m_Boat_RightStick;
    private readonly InputAction m_Boat_ButtonSouth;
    private readonly InputAction m_Boat_ButtonEast;
    private readonly InputAction m_Boat_ButtonNorth;
    private readonly InputAction m_Boat_Interact;
    private readonly InputAction m_Boat_LookingGlass;
    public struct BoatActions
    {
        private @PlayerControls m_Wrapper;
        public BoatActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftTrigger => m_Wrapper.m_Boat_LeftTrigger;
        public InputAction @RightTrigger => m_Wrapper.m_Boat_RightTrigger;
        public InputAction @LeftStick => m_Wrapper.m_Boat_LeftStick;
        public InputAction @RightStick => m_Wrapper.m_Boat_RightStick;
        public InputAction @ButtonSouth => m_Wrapper.m_Boat_ButtonSouth;
        public InputAction @ButtonEast => m_Wrapper.m_Boat_ButtonEast;
        public InputAction @ButtonNorth => m_Wrapper.m_Boat_ButtonNorth;
        public InputAction @Interact => m_Wrapper.m_Boat_Interact;
        public InputAction @LookingGlass => m_Wrapper.m_Boat_LookingGlass;
        public InputActionMap Get() { return m_Wrapper.m_Boat; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BoatActions set) { return set.Get(); }
        public void SetCallbacks(IBoatActions instance)
        {
            if (m_Wrapper.m_BoatActionsCallbackInterface != null)
            {
                @LeftTrigger.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnLeftTrigger;
                @LeftTrigger.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnLeftTrigger;
                @LeftTrigger.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnLeftTrigger;
                @RightTrigger.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnRightTrigger;
                @RightTrigger.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnRightTrigger;
                @RightTrigger.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnRightTrigger;
                @LeftStick.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnLeftStick;
                @LeftStick.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnLeftStick;
                @LeftStick.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnLeftStick;
                @RightStick.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnRightStick;
                @RightStick.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnRightStick;
                @RightStick.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnRightStick;
                @ButtonSouth.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnButtonSouth;
                @ButtonSouth.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnButtonSouth;
                @ButtonSouth.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnButtonSouth;
                @ButtonEast.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnButtonEast;
                @ButtonEast.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnButtonEast;
                @ButtonEast.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnButtonEast;
                @ButtonNorth.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnButtonNorth;
                @ButtonNorth.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnButtonNorth;
                @ButtonNorth.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnButtonNorth;
                @Interact.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnInteract;
                @LookingGlass.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnLookingGlass;
                @LookingGlass.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnLookingGlass;
                @LookingGlass.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnLookingGlass;
            }
            m_Wrapper.m_BoatActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftTrigger.started += instance.OnLeftTrigger;
                @LeftTrigger.performed += instance.OnLeftTrigger;
                @LeftTrigger.canceled += instance.OnLeftTrigger;
                @RightTrigger.started += instance.OnRightTrigger;
                @RightTrigger.performed += instance.OnRightTrigger;
                @RightTrigger.canceled += instance.OnRightTrigger;
                @LeftStick.started += instance.OnLeftStick;
                @LeftStick.performed += instance.OnLeftStick;
                @LeftStick.canceled += instance.OnLeftStick;
                @RightStick.started += instance.OnRightStick;
                @RightStick.performed += instance.OnRightStick;
                @RightStick.canceled += instance.OnRightStick;
                @ButtonSouth.started += instance.OnButtonSouth;
                @ButtonSouth.performed += instance.OnButtonSouth;
                @ButtonSouth.canceled += instance.OnButtonSouth;
                @ButtonEast.started += instance.OnButtonEast;
                @ButtonEast.performed += instance.OnButtonEast;
                @ButtonEast.canceled += instance.OnButtonEast;
                @ButtonNorth.started += instance.OnButtonNorth;
                @ButtonNorth.performed += instance.OnButtonNorth;
                @ButtonNorth.canceled += instance.OnButtonNorth;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @LookingGlass.started += instance.OnLookingGlass;
                @LookingGlass.performed += instance.OnLookingGlass;
                @LookingGlass.canceled += instance.OnLookingGlass;
            }
        }
    }
    public BoatActions @Boat => new BoatActions(this);
    public interface IBoatActions
    {
        void OnLeftTrigger(InputAction.CallbackContext context);
        void OnRightTrigger(InputAction.CallbackContext context);
        void OnLeftStick(InputAction.CallbackContext context);
        void OnRightStick(InputAction.CallbackContext context);
        void OnButtonSouth(InputAction.CallbackContext context);
        void OnButtonEast(InputAction.CallbackContext context);
        void OnButtonNorth(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnLookingGlass(InputAction.CallbackContext context);
    }
}
