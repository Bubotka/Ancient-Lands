using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Codebase.Infrastructure.Services.Input
{
    public interface IInputService : IInputActionCollection2, IDisposable, IService
    {
        InputActionAsset asset { get; }
        InputBinding? bindingMask { get; set; }
        ReadOnlyArray<InputDevice>? devices { get; set; }
        ReadOnlyArray<InputControlScheme> controlSchemes { get; }
        IEnumerable<InputBinding> bindings { get; }
        InputService.PlayerActions @Player { get; }
        void Dispose();
        bool Contains(InputAction action);
        IEnumerator<InputAction> GetEnumerator();
        void Enable();
        void Disable();
        InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false);
        int FindBinding(InputBinding bindingMask, out InputAction action);
    }
}