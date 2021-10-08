// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

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
                    ""name"": ""Grab Left"",
                    ""type"": ""Button"",
                    ""id"": ""030f139d-fca1-47d2-8895-aea6408b10c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Grab Right"",
                    ""type"": ""Button"",
                    ""id"": ""82a23630-d86b-4542-beba-2d7d8470cde2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rope Handling"",
                    ""type"": ""Value"",
                    ""id"": ""552a1291-4636-4f01-bc5c-c67f9348f599"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tiller"",
                    ""type"": ""Value"",
                    ""id"": ""686b5b8d-bbcc-40eb-8db2-5fa0f28936d7"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Grab Main Sail"",
                    ""type"": ""Button"",
                    ""id"": ""54a54790-1a6f-4d67-ac18-4cceac1c2dad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Release"",
                    ""type"": ""Button"",
                    ""id"": ""e6c36b7e-67d4-434c-9a88-91845345db02"",
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
                    ""action"": ""Grab Left"",
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
                    ""action"": ""Grab Right"",
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
                    ""action"": ""Rope Handling"",
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
                    ""action"": ""Tiller"",
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
                    ""action"": ""Grab Main Sail"",
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
                    ""action"": ""Release"",
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
        m_Boat_GrabLeft = m_Boat.FindAction("Grab Left", throwIfNotFound: true);
        m_Boat_GrabRight = m_Boat.FindAction("Grab Right", throwIfNotFound: true);
        m_Boat_RopeHandling = m_Boat.FindAction("Rope Handling", throwIfNotFound: true);
        m_Boat_Tiller = m_Boat.FindAction("Tiller", throwIfNotFound: true);
        m_Boat_GrabMainSail = m_Boat.FindAction("Grab Main Sail", throwIfNotFound: true);
        m_Boat_Release = m_Boat.FindAction("Release", throwIfNotFound: true);
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
    private readonly InputAction m_Boat_GrabLeft;
    private readonly InputAction m_Boat_GrabRight;
    private readonly InputAction m_Boat_RopeHandling;
    private readonly InputAction m_Boat_Tiller;
    private readonly InputAction m_Boat_GrabMainSail;
    private readonly InputAction m_Boat_Release;
    public struct BoatActions
    {
        private @PlayerControls m_Wrapper;
        public BoatActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @GrabLeft => m_Wrapper.m_Boat_GrabLeft;
        public InputAction @GrabRight => m_Wrapper.m_Boat_GrabRight;
        public InputAction @RopeHandling => m_Wrapper.m_Boat_RopeHandling;
        public InputAction @Tiller => m_Wrapper.m_Boat_Tiller;
        public InputAction @GrabMainSail => m_Wrapper.m_Boat_GrabMainSail;
        public InputAction @Release => m_Wrapper.m_Boat_Release;
        public InputActionMap Get() { return m_Wrapper.m_Boat; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BoatActions set) { return set.Get(); }
        public void SetCallbacks(IBoatActions instance)
        {
            if (m_Wrapper.m_BoatActionsCallbackInterface != null)
            {
                @GrabLeft.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnGrabLeft;
                @GrabLeft.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnGrabLeft;
                @GrabLeft.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnGrabLeft;
                @GrabRight.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnGrabRight;
                @GrabRight.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnGrabRight;
                @GrabRight.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnGrabRight;
                @RopeHandling.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnRopeHandling;
                @RopeHandling.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnRopeHandling;
                @RopeHandling.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnRopeHandling;
                @Tiller.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnTiller;
                @Tiller.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnTiller;
                @Tiller.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnTiller;
                @GrabMainSail.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnGrabMainSail;
                @GrabMainSail.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnGrabMainSail;
                @GrabMainSail.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnGrabMainSail;
                @Release.started -= m_Wrapper.m_BoatActionsCallbackInterface.OnRelease;
                @Release.performed -= m_Wrapper.m_BoatActionsCallbackInterface.OnRelease;
                @Release.canceled -= m_Wrapper.m_BoatActionsCallbackInterface.OnRelease;
            }
            m_Wrapper.m_BoatActionsCallbackInterface = instance;
            if (instance != null)
            {
                @GrabLeft.started += instance.OnGrabLeft;
                @GrabLeft.performed += instance.OnGrabLeft;
                @GrabLeft.canceled += instance.OnGrabLeft;
                @GrabRight.started += instance.OnGrabRight;
                @GrabRight.performed += instance.OnGrabRight;
                @GrabRight.canceled += instance.OnGrabRight;
                @RopeHandling.started += instance.OnRopeHandling;
                @RopeHandling.performed += instance.OnRopeHandling;
                @RopeHandling.canceled += instance.OnRopeHandling;
                @Tiller.started += instance.OnTiller;
                @Tiller.performed += instance.OnTiller;
                @Tiller.canceled += instance.OnTiller;
                @GrabMainSail.started += instance.OnGrabMainSail;
                @GrabMainSail.performed += instance.OnGrabMainSail;
                @GrabMainSail.canceled += instance.OnGrabMainSail;
                @Release.started += instance.OnRelease;
                @Release.performed += instance.OnRelease;
                @Release.canceled += instance.OnRelease;
            }
        }
    }
    public BoatActions @Boat => new BoatActions(this);
    public interface IBoatActions
    {
        void OnGrabLeft(InputAction.CallbackContext context);
        void OnGrabRight(InputAction.CallbackContext context);
        void OnRopeHandling(InputAction.CallbackContext context);
        void OnTiller(InputAction.CallbackContext context);
        void OnGrabMainSail(InputAction.CallbackContext context);
        void OnRelease(InputAction.CallbackContext context);
    }
}
