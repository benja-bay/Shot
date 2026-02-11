using System.Collections;
using Core.Systems;
using Gameplay.Minigames.ShotGrab.Core;
using Gameplay.Minigames.Skillbar;
using UI;
using UnityEngine;

namespace Core.GameFlow
{
    public class GameFlowController : MonoBehaviour
    {
        private enum GameState
        {
            Lobby,
            ShotGrab,
            Transition,
            Skillbar
        }

        [Header("Lobby")]
        [SerializeField] private LobbyController lobby;

        [Header("Minigames")]
        [SerializeField] private ShotGrabGame shotGrabGame;
        [SerializeField] private SkillbarGame skillbarGame;

        [Header("Transition")]
        [SerializeField] private float delayBeforeSkillbar = 1.5f;

        [Header("Systems")]
        [SerializeField] private LifeSystem lifeSystem;

        private GameState currentState;
        private int currentScore;

        private void Awake()
        {
            shotGrabGame.OnShotGrabbed += HandleShotGrabbed;
            skillbarGame.OnSkillbarFinished += HandleSkillbarFinished;
            lobby.OnPlayPressed += HandlePlayPressed;
            lifeSystem.OnNoLivesLeft += HandleGameOver;
        }

        private void Start()
        {
            EnterLobby();
        }

        private void OnDestroy()
        {
            shotGrabGame.OnShotGrabbed -= HandleShotGrabbed;
            skillbarGame.OnSkillbarFinished -= HandleSkillbarFinished;
            lobby.OnPlayPressed -= HandlePlayPressed;
            lifeSystem.OnNoLivesLeft -= HandleGameOver;
        }

        /* =========================
         * LOBBY
         * ========================= */
        private void EnterLobby()
        {
            lifeSystem.ResetLives();

            currentState = GameState.Lobby;
            currentScore = 0;

            lobby.Show();

            shotGrabGame.gameObject.SetActive(false);
            skillbarGame.gameObject.SetActive(false);
        }

        private void HandlePlayPressed()
        {
            if (currentState != GameState.Lobby)
                return;

            lobby.Hide();
            EnterShotGrab();
        }

        /* =========================
         * SHOT GRAB
         * ========================= */
        private void EnterShotGrab()
        {
            currentState = GameState.ShotGrab;

            lobby.Hide();

            shotGrabGame.gameObject.SetActive(true);
            skillbarGame.gameObject.SetActive(false);

            shotGrabGame.StartGame();
        }

        private void HandleShotGrabbed()
        {
            if (currentState != GameState.ShotGrab)
                return;

            StartCoroutine(TransitionToSkillbar());
        }

        /* =========================
         * TRANSITION
         * ========================= */
        private IEnumerator TransitionToSkillbar()
        {
            currentState = GameState.Transition;

            shotGrabGame.StopGame();
            yield return new WaitForSeconds(delayBeforeSkillbar);

            EnterSkillbar();
        }

        /* =========================
         * SKILLBAR
         * ========================= */
        private void EnterSkillbar()
        {
            currentState = GameState.Skillbar;

            shotGrabGame.gameObject.SetActive(false);
            skillbarGame.gameObject.SetActive(true);

            skillbarGame.StartGame(currentScore);
        }

        private void HandleSkillbarFinished(SkillbarResult result)
        {
            ApplySkillbarResult(result);

            if (lifeSystem.CurrentLives <= 0)
                return;

            EnterShotGrab();
        }

        private void ApplySkillbarResult(SkillbarResult result)
        {
            switch (result)
            {
                case SkillbarResult.Perfect:
                    currentScore += 2;
                    break;

                case SkillbarResult.Normal:
                    currentScore += 1;
                    break;

                case SkillbarResult.Fail:
                    lifeSystem.LoseLife(1);
                    break;
            }

            Debug.Log($"[GameFlow] Score actual: {currentScore}", this);
        }

        private void HandleGameOver()
        {
            Debug.Log("[GameFlow] GAME OVER", this);

            StopAllCoroutines();
            shotGrabGame.StopGame();

            EnterLobby();
        }
    }
}
