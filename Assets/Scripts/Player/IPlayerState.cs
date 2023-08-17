using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerState
{
    void StartState();
    void EndState();
    void Move(Vector2 i_move);
    void Jump(InputAction.CallbackContext ctx);
    void Grapple(InputAction.CallbackContext ctx);
}