using Gameplay.Minigames.ShotGrab.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Minigames.ShotGrab.Input
{
    public class LaneInputController : MonoBehaviour
    {
        [SerializeField] private HandController hand;
        [SerializeField] private float dragSensitivity = 0.01f;

        private Vector2 lastPosition;
        private bool isDragging;

        private void Update()
        {
            // Si hay touch activo, usar drag táctil
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.isPressed)
            {
                HandleTouch();
            }
            else
            {
                // Si no, usar mouse follow
                HandleMouseFollow();
            }
        }

        private void HandleTouch()
        {
            var touch = Touchscreen.current.primaryTouch;

            Vector2 pos = touch.position.ReadValue();

            if (!isDragging)
            {
                lastPosition = pos;
                isDragging = true;
            }

            float deltaY = (pos.y - lastPosition.y) * dragSensitivity;
            hand.MoveVertical(deltaY);

            lastPosition = pos;
        }

        private void HandleMouseFollow()
        {
            if (Mouse.current == null) return;

            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            float deltaY = worldPos.y - hand.transform.position.y;
            hand.MoveVertical(deltaY);
        }
    }
}