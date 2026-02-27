using System.Collections;
using System.Collections.Generic;
using Core.Systems;
using Gameplay.Minigames.ShotGrab.Core;
using Gameplay.Minigames.ShotGrab.Data;
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

        [Header("ShotGrab Rules")]
        [Tooltip("Cantidad de tragos que el jugador debe agarrar antes de pasar al Skillbar")]
        [SerializeField] private int shotsPerRound = 5;

        [Header("Transition")]
        [SerializeField] private float delayBeforeSkillbar = 1.5f;

        [Header("Systems")]
        [SerializeField] private LifeSystem lifeSystem;
        [SerializeField] private LifeUI lifeUI;
        [SerializeField] private ScoreSystem scoreSystem;

        private GameState currentState;

        private readonly List<ShotData> grabbedShots = new();
        private int pendingSkillbars;

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

        private void EnterLobby()
        {
            currentState = GameState.Lobby;

            lifeUI.Hide();
            lifeSystem.ResetLives();
            scoreSystem.ResetScore();

            lobby.Show();

            shotGrabGame.gameObject.SetActive(false);
            skillbarGame.gameObject.SetActive(false);
        }

        private void HandlePlayPressed()
        {
            if (currentState != GameState.Lobby)
                return;

            lobby.Hide();
            lifeUI.Show();

            EnterShotGrab();
        }

        private void EnterShotGrab()
        {
            currentState = GameState.ShotGrab;

            grabbedShots.Clear();

            shotGrabGame.gameObject.SetActive(true);
            skillbarGame.gameObject.SetActive(false);

            shotGrabGame.StartGame();
        }

        private void HandleShotGrabbed(ShotData data)
        {
            if (currentState != GameState.ShotGrab)
                return;

            grabbedShots.Add(data);

            if (grabbedShots.Count >= shotsPerRound)
            {
                StartCoroutine(TransitionToSkillbar());
            }
        }

        private IEnumerator TransitionToSkillbar()
        {
            currentState = GameState.Transition;

            shotGrabGame.StopGame();
            yield return new WaitForSeconds(delayBeforeSkillbar);

            pendingSkillbars = grabbedShots.Count;
            EnterSkillbar();
        }

        private void EnterSkillbar()
        {
            currentState = GameState.Skillbar;

            shotGrabGame.gameObject.SetActive(false);
            skillbarGame.gameObject.SetActive(true);

            skillbarGame.StartGame();
        }

        private void HandleSkillbarFinished(SkillbarResult result)
        {
            if (currentState != GameState.Skillbar)
                return;

            ApplySkillbarResult(result);

            pendingSkillbars--;

            if (lifeSystem.CurrentLives <= 0)
                return;

            if (pendingSkillbars > 0)
            {
                skillbarGame.StartGame();
                return;
            }

            grabbedShots.Clear();
            EnterShotGrab();
        }

        private void ApplySkillbarResult(SkillbarResult result)
        {
            switch (result)
            {
                case SkillbarResult.Perfect:
                    scoreSystem.AddPoints(2);
                    break;

                case SkillbarResult.Normal:
                    scoreSystem.AddPoints(1);
                    break;

                case SkillbarResult.Fail:
                    lifeSystem.LoseLife(1);
                    break;
            }

            Debug.Log($"[GameFlow] Score actual: {scoreSystem.CurrentScore}", this);
        }

        private void HandleGameOver()
        {
            Debug.Log("[GameFlow] GAME OVER", this);

            StopAllCoroutines();
            shotGrabGame.StopGame();

            EnterLobby();
        }
        
        private void OnValidate()
        {
            if (shotsPerRound < 1)
                shotsPerRound = 1;
        }
    }
}