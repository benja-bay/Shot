using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Core.Systems;

namespace Gameplay.Minigames.Skillbar
{
    public class SkillbarGame : MonoBehaviour
    {
        public event Action<SkillbarResult> OnSkillbarFinished;

        [Header("References")]
        [SerializeField] private SkillbarSelector selector;
        [SerializeField] private SkillbarEvaluator evaluator;
        [SerializeField] private ScoreSystem scoreSystem;

        private bool isActive;

        public void StartGame()
        {
            gameObject.SetActive(true);
            isActive = true;

            int playerScore = scoreSystem.CurrentScore;

            selector.Setup(playerScore);
            evaluator.SetupPerfectZone(playerScore);

            selector.StartMoving();
        }

        private void Update()
        {
            if (!isActive) return;

            if (IsInputPressed())
            {
                FinishSkillbar();
            }
        }

        private bool IsInputPressed()
        {
            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
                return true;

            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
                return true;

            return false;
        }

        private void FinishSkillbar()
        {
            isActive = false;

            selector.StopMoving();

            SkillbarResult result =
                evaluator.Evaluate(selector.CurrentX);

            OnSkillbarFinished?.Invoke(result);
        }
    }
}