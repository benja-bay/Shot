using Core.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LifeUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LifeSystem lifeSystem;
        [SerializeField] private Transform container;

        [Header("Prefabs")]
        [SerializeField] private Image lifeImagePrefab;

        [Header("Sprites")]
        [SerializeField] private Sprite healthyLiver; // 2 pts
        [SerializeField] private Sprite damagedLiver; // 1 pt

        private void Awake()
        {
            lifeSystem.OnLivesChanged += UpdateUI;
            gameObject.SetActive(false); // oculto al inicio
        }

        private void OnDestroy()
        {
            lifeSystem.OnLivesChanged -= UpdateUI;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            UpdateUI(lifeSystem.CurrentLives);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdateUI(int lifePoints)
        {
            foreach (Transform child in container)
                Destroy(child.gameObject);

            int points = lifePoints;

            while (points >= 2)
            {
                CreateLiver(healthyLiver);
                points -= 2;
            }

            if (points == 1)
                CreateLiver(damagedLiver);
        }

        private void CreateLiver(Sprite sprite)
        {
            Image img = Instantiate(lifeImagePrefab, container);
            img.sprite = sprite;
        }
    }
}