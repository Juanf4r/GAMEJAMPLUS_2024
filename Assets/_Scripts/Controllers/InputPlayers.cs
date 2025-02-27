//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/_Scripts/Controllers/InputPlayers.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputPlayers: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputPlayers()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputPlayers"",
    ""maps"": [
        {
            ""name"": ""Players"",
            ""id"": ""37dc45ee-ce0a-4ce2-bc72-abf15673b5a8"",
            ""actions"": [
                {
                    ""name"": ""Movement1"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3e03e613-b67d-4d06-81ea-93670aebaf96"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Movement2"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0dfae7b5-4068-4535-b8cf-e2a9d6c5dda9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Punch1"",
                    ""type"": ""Button"",
                    ""id"": ""cde54d32-6990-480f-8f52-c4438e011b67"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Punch2"",
                    ""type"": ""Button"",
                    ""id"": ""8097b1a1-6760-42ec-8ad9-103f47142c7e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PowerUp1"",
                    ""type"": ""Button"",
                    ""id"": ""02aa6f82-8c0a-452c-8c54-60ed2951379d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PowerUp2"",
                    ""type"": ""Button"",
                    ""id"": ""58bc782b-24cb-4a81-80b4-2d2ec7a7c3ba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""1ccecdf1-6596-4f87-8f49-9c7c8cfb4101"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""2d873458-2ea6-499d-a1c4-04f373c3a548"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement1"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c6e24646-83ab-4499-99be-d3f6776d59b9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""eb61aca0-1f3b-4f76-b14e-895db228aa26"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c6ad424f-4e33-4da2-864a-921b5ce5ffd9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1485473c-e155-4192-8962-1013572fc73d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d401abfc-be80-4370-a1d2-f4ef41de8c36"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Punch1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""ARROWS"",
                    ""id"": ""8e4649b1-527d-4d89-a906-ff510e2b386e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement2"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b9038bbe-0f83-4004-8e56-e613d833a354"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bac5b1b5-6fcc-4fbb-ad96-8968a7c43e7e"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d4231dc0-6e84-4c8d-b058-e014f09c7e43"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d69f830c-45bc-4c0d-b79b-c1efc1c64e02"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ae18195d-555a-42dd-b22e-1c5a1f165023"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Punch2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41a0941b-9848-4cf3-a1cc-2d33d0d24af2"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PowerUp1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""453af183-3368-4c99-a034-a940fce0c123"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PowerUp2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2bb83939-dc98-4b2d-873a-7f66e77eb7fc"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Players
        m_Players = asset.FindActionMap("Players", throwIfNotFound: true);
        m_Players_Movement1 = m_Players.FindAction("Movement1", throwIfNotFound: true);
        m_Players_Movement2 = m_Players.FindAction("Movement2", throwIfNotFound: true);
        m_Players_Punch1 = m_Players.FindAction("Punch1", throwIfNotFound: true);
        m_Players_Punch2 = m_Players.FindAction("Punch2", throwIfNotFound: true);
        m_Players_PowerUp1 = m_Players.FindAction("PowerUp1", throwIfNotFound: true);
        m_Players_PowerUp2 = m_Players.FindAction("PowerUp2", throwIfNotFound: true);
        m_Players_Pause = m_Players.FindAction("Pause", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Players
    private readonly InputActionMap m_Players;
    private List<IPlayersActions> m_PlayersActionsCallbackInterfaces = new List<IPlayersActions>();
    private readonly InputAction m_Players_Movement1;
    private readonly InputAction m_Players_Movement2;
    private readonly InputAction m_Players_Punch1;
    private readonly InputAction m_Players_Punch2;
    private readonly InputAction m_Players_PowerUp1;
    private readonly InputAction m_Players_PowerUp2;
    private readonly InputAction m_Players_Pause;
    public struct PlayersActions
    {
        private @InputPlayers m_Wrapper;
        public PlayersActions(@InputPlayers wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement1 => m_Wrapper.m_Players_Movement1;
        public InputAction @Movement2 => m_Wrapper.m_Players_Movement2;
        public InputAction @Punch1 => m_Wrapper.m_Players_Punch1;
        public InputAction @Punch2 => m_Wrapper.m_Players_Punch2;
        public InputAction @PowerUp1 => m_Wrapper.m_Players_PowerUp1;
        public InputAction @PowerUp2 => m_Wrapper.m_Players_PowerUp2;
        public InputAction @Pause => m_Wrapper.m_Players_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Players; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayersActions set) { return set.Get(); }
        public void AddCallbacks(IPlayersActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayersActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayersActionsCallbackInterfaces.Add(instance);
            @Movement1.started += instance.OnMovement1;
            @Movement1.performed += instance.OnMovement1;
            @Movement1.canceled += instance.OnMovement1;
            @Movement2.started += instance.OnMovement2;
            @Movement2.performed += instance.OnMovement2;
            @Movement2.canceled += instance.OnMovement2;
            @Punch1.started += instance.OnPunch1;
            @Punch1.performed += instance.OnPunch1;
            @Punch1.canceled += instance.OnPunch1;
            @Punch2.started += instance.OnPunch2;
            @Punch2.performed += instance.OnPunch2;
            @Punch2.canceled += instance.OnPunch2;
            @PowerUp1.started += instance.OnPowerUp1;
            @PowerUp1.performed += instance.OnPowerUp1;
            @PowerUp1.canceled += instance.OnPowerUp1;
            @PowerUp2.started += instance.OnPowerUp2;
            @PowerUp2.performed += instance.OnPowerUp2;
            @PowerUp2.canceled += instance.OnPowerUp2;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
        }

        private void UnregisterCallbacks(IPlayersActions instance)
        {
            @Movement1.started -= instance.OnMovement1;
            @Movement1.performed -= instance.OnMovement1;
            @Movement1.canceled -= instance.OnMovement1;
            @Movement2.started -= instance.OnMovement2;
            @Movement2.performed -= instance.OnMovement2;
            @Movement2.canceled -= instance.OnMovement2;
            @Punch1.started -= instance.OnPunch1;
            @Punch1.performed -= instance.OnPunch1;
            @Punch1.canceled -= instance.OnPunch1;
            @Punch2.started -= instance.OnPunch2;
            @Punch2.performed -= instance.OnPunch2;
            @Punch2.canceled -= instance.OnPunch2;
            @PowerUp1.started -= instance.OnPowerUp1;
            @PowerUp1.performed -= instance.OnPowerUp1;
            @PowerUp1.canceled -= instance.OnPowerUp1;
            @PowerUp2.started -= instance.OnPowerUp2;
            @PowerUp2.performed -= instance.OnPowerUp2;
            @PowerUp2.canceled -= instance.OnPowerUp2;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
        }

        public void RemoveCallbacks(IPlayersActions instance)
        {
            if (m_Wrapper.m_PlayersActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayersActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayersActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayersActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayersActions @Players => new PlayersActions(this);
    public interface IPlayersActions
    {
        void OnMovement1(InputAction.CallbackContext context);
        void OnMovement2(InputAction.CallbackContext context);
        void OnPunch1(InputAction.CallbackContext context);
        void OnPunch2(InputAction.CallbackContext context);
        void OnPowerUp1(InputAction.CallbackContext context);
        void OnPowerUp2(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
