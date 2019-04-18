    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class SelectTopping : MonoBehaviour
    {

        GameplayManager gameplayManager;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ChangeImageColor);
            gameplayManager = GameObject.FindObjectOfType<GameplayManager>();

        }

        public void ChangeImageColor()
        {
            if (gameplayManager.toppingLimit < 3)
            {
            GetComponent<Image>().color = new Color(205f / 255f, 255f / 255f, 173f / 255f);

                string toppingName = GetComponent<Image>().sprite.name;

                gameplayManager.updatePizzaName(toppingName);

            }
        }


        private Color RandomColor()
        {
            return new Color(
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f)
                );
        }
    }
