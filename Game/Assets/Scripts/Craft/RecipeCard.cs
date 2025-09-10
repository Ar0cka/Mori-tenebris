using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace
{
    public class RecipeCard : MonoBehaviour
    {
        [Header("Card info")]
        [SerializeField] private RecipesConfig recipe;
        [SerializeField] private Button recipeButton;

        [Header("Card UI")]
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI title;
        
        [Inject] private PanelController _panelController;
        
        private void Awake()
        {
            recipeButton.onClick.AddListener(OpenPanel);

            image.sprite = recipe.GetResultItemData().iconItem;
            title.text = recipe.GetResultItemData().nameItem;
        }

        private void OpenPanel()
        {
            _panelController?.OpenPanel(recipe);
        }
    }
}