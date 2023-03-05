//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Scripts/SimulationController.inputactions
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

public partial class @SimulationController : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @SimulationController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""SimulationController"",
    ""maps"": [
        {
            ""name"": ""Simulation"",
            ""id"": ""9bed5fc7-d640-4da6-b003-3bf0753be588"",
            ""actions"": [
                {
                    ""name"": ""Initial_Conditions"",
                    ""type"": ""Button"",
                    ""id"": ""d19f9741-255e-4fa8-b530-4cfa7daeb6b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Simulation_Step"",
                    ""type"": ""Button"",
                    ""id"": ""5c27573a-23e6-45eb-a1d6-919431c64d3f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move_One_Agent"",
                    ""type"": ""Button"",
                    ""id"": ""e5700d07-d47f-46c6-9776-16e9655f2528"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""dc1d7e63-9d05-45d7-97e8-db85a1eca25f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Initial_Conditions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e9719267-30d8-49d7-8077-25fc53b8ecdc"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Simulation_Step"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4443b888-6342-45ac-9846-29b9b7b04a88"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move_One_Agent"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
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
        // Simulation
        m_Simulation = asset.FindActionMap("Simulation", throwIfNotFound: true);
        m_Simulation_Initial_Conditions = m_Simulation.FindAction("Initial_Conditions", throwIfNotFound: true);
        m_Simulation_Simulation_Step = m_Simulation.FindAction("Simulation_Step", throwIfNotFound: true);
        m_Simulation_Move_One_Agent = m_Simulation.FindAction("Move_One_Agent", throwIfNotFound: true);
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

    // Simulation
    private readonly InputActionMap m_Simulation;
    private ISimulationActions m_SimulationActionsCallbackInterface;
    private readonly InputAction m_Simulation_Initial_Conditions;
    private readonly InputAction m_Simulation_Simulation_Step;
    private readonly InputAction m_Simulation_Move_One_Agent;
    public struct SimulationActions
    {
        private @SimulationController m_Wrapper;
        public SimulationActions(@SimulationController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Initial_Conditions => m_Wrapper.m_Simulation_Initial_Conditions;
        public InputAction @Simulation_Step => m_Wrapper.m_Simulation_Simulation_Step;
        public InputAction @Move_One_Agent => m_Wrapper.m_Simulation_Move_One_Agent;
        public InputActionMap Get() { return m_Wrapper.m_Simulation; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SimulationActions set) { return set.Get(); }
        public void SetCallbacks(ISimulationActions instance)
        {
            if (m_Wrapper.m_SimulationActionsCallbackInterface != null)
            {
                @Initial_Conditions.started -= m_Wrapper.m_SimulationActionsCallbackInterface.OnInitial_Conditions;
                @Initial_Conditions.performed -= m_Wrapper.m_SimulationActionsCallbackInterface.OnInitial_Conditions;
                @Initial_Conditions.canceled -= m_Wrapper.m_SimulationActionsCallbackInterface.OnInitial_Conditions;
                @Simulation_Step.started -= m_Wrapper.m_SimulationActionsCallbackInterface.OnSimulation_Step;
                @Simulation_Step.performed -= m_Wrapper.m_SimulationActionsCallbackInterface.OnSimulation_Step;
                @Simulation_Step.canceled -= m_Wrapper.m_SimulationActionsCallbackInterface.OnSimulation_Step;
                @Move_One_Agent.started -= m_Wrapper.m_SimulationActionsCallbackInterface.OnMove_One_Agent;
                @Move_One_Agent.performed -= m_Wrapper.m_SimulationActionsCallbackInterface.OnMove_One_Agent;
                @Move_One_Agent.canceled -= m_Wrapper.m_SimulationActionsCallbackInterface.OnMove_One_Agent;
            }
            m_Wrapper.m_SimulationActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Initial_Conditions.started += instance.OnInitial_Conditions;
                @Initial_Conditions.performed += instance.OnInitial_Conditions;
                @Initial_Conditions.canceled += instance.OnInitial_Conditions;
                @Simulation_Step.started += instance.OnSimulation_Step;
                @Simulation_Step.performed += instance.OnSimulation_Step;
                @Simulation_Step.canceled += instance.OnSimulation_Step;
                @Move_One_Agent.started += instance.OnMove_One_Agent;
                @Move_One_Agent.performed += instance.OnMove_One_Agent;
                @Move_One_Agent.canceled += instance.OnMove_One_Agent;
            }
        }
    }
    public SimulationActions @Simulation => new SimulationActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface ISimulationActions
    {
        void OnInitial_Conditions(InputAction.CallbackContext context);
        void OnSimulation_Step(InputAction.CallbackContext context);
        void OnMove_One_Agent(InputAction.CallbackContext context);
    }
}
