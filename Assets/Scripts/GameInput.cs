using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions PlayerInputActions;

    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerDash;
    public event Action<bool> OnBlockChanged;
    public event EventHandler OnPlayerHeal;
    public event EventHandler OnGroundSlam;
    public event EventHandler OnInteract;
    public event EventHandler OnPause;
    public event EventHandler OnSkillPanel;

    private void Awake()
    {
        Instance = this;

        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Enable();

        PlayerInputActions.Combat.Attack.started += PlayerAttack_started;
        PlayerInputActions.Player.Dash.started += PlayerDash_performed;
        PlayerInputActions.Combat.Block.performed += ctx => OnBlockChanged?.Invoke(true);
        PlayerInputActions.Combat.Block.canceled += ctx => OnBlockChanged?.Invoke(false);
        PlayerInputActions.Player.Heal.started += PlayerHeal_performed;
        PlayerInputActions.Combat.GroundSlam.started += GroundSlam_started;
        PlayerInputActions.Player.Interact.started += Interact_started;
        PlayerInputActions.Pause.Pause.started += Pause_started;
        PlayerInputActions.Player.SkillPanel.started += SkillPanel_started;
    }

    private void SkillPanel_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSkillPanel?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    private void GroundSlam_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnGroundSlam?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerDash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerDash?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerAttack_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerHeal_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerHeal?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = PlayerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public void DisableMovement()
    {
        PlayerInputActions.Disable();
    }

    public void DisableGameplay()
    {
        PlayerInputActions.Combat.Disable();
        PlayerInputActions.Player.Move.Disable();
        PlayerInputActions.Player.Dash.Disable();
        PlayerInputActions.Player.Heal.Disable();
    }

    public void EnableGameplay()
    {
        PlayerInputActions.Combat.Enable();
        PlayerInputActions.Player.Enable();
    }

    private void OnDestroy()
    {
        PlayerInputActions.Combat.Attack.started -= PlayerAttack_started;
        PlayerInputActions.Player.Dash.started -= PlayerDash_performed;
        PlayerInputActions.Player.Heal.started -= PlayerHeal_performed;
        PlayerInputActions.Combat.GroundSlam.started -= GroundSlam_started;
        PlayerInputActions.Player.Interact.started -= Interact_started;
        PlayerInputActions.Pause.Pause.started -= Pause_started;
        PlayerInputActions.Player.SkillPanel.started -= SkillPanel_started;
    }
}
