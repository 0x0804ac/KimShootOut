using UnityEngine;
using UnityEngine.InputSystem;

//Rebinds the SAME PlayerInput.actions asset used by gameplay, not a disconnected copy.
public sealed class PreferencesBindings : System.IDisposable
{
    private readonly InputActionAsset actions;
    private readonly PreferencesService service;
    private InputActionRebindingExtensions.RebindingOperation operation;
    private InputAction selectedAction;
    private int selectedBindingIndex;
    private string previousOverridePath;
    private bool wasEnabled;

    public event System.Action BindingsChanged;
    public event System.Action<bool> ListeningChanged;
    public event System.Action<string> RebindRejected;

    public PreferencesBindings(InputActionAsset actions, PreferencesService service)
    {
        this.actions = actions ?? throw new System.ArgumentNullException(nameof(actions));
        this.service = service ?? throw new System.ArgumentNullException(nameof(service));
        LoadOverrides();
    }

    public bool IsListening()
    {
        return operation != null;
    }

    public string GetBindingDisplay(string actionName, int bindingIndex)
    {
        if (!TryFindBinding(actionName, bindingIndex, out InputAction action))
        {
            return "—";
        }

        return action.GetBindingDisplayString(bindingIndex,
            InputBinding.DisplayStringOptions.DontIncludeInteractions);
    }

    public bool TryStartRebind(string actionName, int bindingIndex)
    {
        if (operation != null || !TryFindBinding(actionName, bindingIndex, out InputAction action))
        {
            return false;
        }

        InputBinding binding = action.bindings[bindingIndex];
        string path = binding.effectivePath;

        if (binding.isComposite || string.IsNullOrEmpty(path) ||
            !path.StartsWith("<Keyboard>/", System.StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        selectedAction = action;
        selectedBindingIndex = bindingIndex;
        previousOverridePath = binding.overridePath;
        wasEnabled = action.enabled;

        if (wasEnabled)
        {
            action.Disable();
        }

        operation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsHavingToMatchPath("<Keyboard>/*")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(RebindCompleted)
            .OnCancel(RebindCanceled);

        ListeningChanged?.Invoke(true);
        operation.Start();
        return true;
    }

    public void CancelRebind()
    {
        if (operation != null)
        {
            operation.Cancel();
        }
    }

    public void ResetAllBindings()
    {
        CancelRebind();
        actions.RemoveAllBindingOverrides();
        service.SetBindingOverridesJson(string.Empty);
        BindingsChanged?.Invoke();
    }

    public void Dispose()
    {
        CancelRebind();
    }

    private void LoadOverrides()
    {
        string json = service.GetBindingOverridesJson();
        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        try
        {
            actions.LoadBindingOverridesFromJson(json);
        }
        catch (System.Exception exception)
        {
            Debug.LogWarning($"조작 키 설정을 불러오는 중 오류가 발생했습니다: {exception.Message}");
            actions.RemoveAllBindingOverrides();
            service.SetBindingOverridesJson(string.Empty);
        }
    }

    private bool TryFindBinding(string actionName, int bindingIndex, out InputAction action)
    {
        action = actions.FindAction(actionName, false);
        return action != null &&
            bindingIndex >= 0 && bindingIndex < action.bindings.Count;
    }

    private void RebindCompleted(InputActionRebindingExtensions.RebindingOperation completed)
    {
        string candidate = selectedAction.bindings[selectedBindingIndex].effectivePath;
        bool conflict = HasConflict(selectedAction, selectedBindingIndex, candidate);

        if (conflict)
        {
            RestorePreviousBinding();
        }
        else
        {
            service.SetBindingOverridesJson(actions.SaveBindingOverridesAsJson());
        }

        CleanupRebind();
        BindingsChanged?.Invoke();

        if (conflict)
        {
            RebindRejected?.Invoke("다른 동작에 이미 사용 중인 키입니다. / Key already in use.");
        }
    }

    private void RebindCanceled(InputActionRebindingExtensions.RebindingOperation canceled)
    {
        CleanupRebind();
        BindingsChanged?.Invoke();
    }

    private void CleanupRebind()
    {
        operation?.Dispose();
        operation = null;

        if (selectedAction != null && wasEnabled)
        {
            selectedAction.Enable();
        }

        selectedAction = null;
        previousOverridePath = null;
        ListeningChanged?.Invoke(false);
    }

    private void RestorePreviousBinding()
    {
        if (previousOverridePath == null)
        {
            selectedAction.RemoveBindingOverride(selectedBindingIndex);
        }
        else
        {
            selectedAction.ApplyBindingOverride(selectedBindingIndex, previousOverridePath);
        }
    }

    private bool HasConflict(InputAction action, int bindingIndex, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        //Within the same action map; intentional duplicates across maps are allowed.
        foreach (InputAction otherAction in action.actionMap.actions)
        {
            for (int i = 0; i < otherAction.bindings.Count; i++)
            {
                if (otherAction == action && i == bindingIndex)
                {
                    continue;
                }

                InputBinding other = otherAction.bindings[i];
                if (!other.isComposite && string.Equals(other.effectivePath, path,
                    System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
