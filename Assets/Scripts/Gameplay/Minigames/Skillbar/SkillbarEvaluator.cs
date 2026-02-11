using UnityEngine;

namespace Gameplay.Minigames.Skillbar
{
    public class SkillbarEvaluator : MonoBehaviour
    {
        [Header("Perfect Zone")]
        [SerializeField] private RectTransform perfectZone;

        [Header("Difficulty")]
        [SerializeField] private float easyWidth = 200f;
        [SerializeField] private float hardWidth = 80f;

        [Tooltip("Score necesario para llegar a la dificultad máxima")]
        [SerializeField] private int scoreForMaxDifficulty = 20;

        public SkillbarResult Evaluate(float selectorX)
        {
            float min =
                perfectZone.anchoredPosition.x
                - perfectZone.rect.width / 2f;

            float max =
                perfectZone.anchoredPosition.x
                + perfectZone.rect.width / 2f;

            if (selectorX >= min && selectorX <= max)
                return SkillbarResult.Perfect;

            return SkillbarResult.Fail;
        }

        public void SetupPerfectZone(int playerScore)
        {
            float t = Mathf.Clamp01(
                (float)playerScore / scoreForMaxDifficulty
            );

            float width = Mathf.Lerp(easyWidth, hardWidth, t);

            perfectZone.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                width
            );
        }
    }
}