using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int RevealedCards { get; set; }
    public int TargetPairs { get; set; }
    public int FoundPairs { get; set; }

    public CardBehavior card1;
    public CardBehavior card2;
    public GameObject CardHolder;

    public void HideCards()
    {
        card1.Hide();
        card2.Hide();

        RevealedCards = 0;

        card1 = null;
        card2 = null;
    }

    public bool RevealCard(CardBehavior c)
    {
        if(RevealedCards >= 2)
        {
            return false;
        }

        if (card1 == null)
        {
            card1 = c;
        }
        else
        {
            card2 = c;
        }

        RevealedCards++;

        if (RevealedCards == 2)
        {
            if (card1.transform.Find("card-face").gameObject.GetComponent<Renderer>().material.name ==
            card2.transform.Find("card-face").gameObject.GetComponent<Renderer>().material.name)
            {
                FoundPairs++;
                RevealedCards = 0;
                card1 = null;
                card2 = null;

                if(FoundPairs == TargetPairs)
                {
                    Win();
                }
            }
            else
            {
                Invoke("HideCards", 1);
            }
        }

        return true;
    }

    public void Win()
    {
        WinScreen.SetActive(true);
        CardHolder.SetActive(false);

        this.WinScreenText.SetText($"Congratulations! It took you {(int)elapsedTimeFloat} seconds to finish!");
    }

    [SerializeField]
    public TextMeshProUGUI WinScreenText;
    [SerializeField]
    public TextMeshProUGUI elapsedTimeText;
    [SerializeField]
    public GameObject WinScreen;
    [SerializeField]
    public float elapsedTimeFloat = 0;

    [SerializeField]
    public List<Material> Materials = new List<Material>();
    public List<int> SelectedMaterials = new List<int>();


    // Start is called before the first frame update
    void Start()
    {
        WinScreen.SetActive(false);

        RevealedCards = 0;
        FoundPairs = 0;
        
        // Fetch all cards in game
        List<GameObject> Cards = new List<GameObject>();
        Cards.AddRange(GameObject.FindGameObjectsWithTag("Card"));
        TargetPairs = Cards.Count / 2;

        while (Cards.Count != 0)
        {
            // Fetch random index from materials that isn't already selected
            int matIdx;
            while (true)
            {
                matIdx = Random.Range(0, Materials.Count);
                if (SelectedMaterials.Contains(matIdx)) continue;

                SelectedMaterials.Add(matIdx);
                break;
            }

            int card1idx = Random.Range(0, Cards.Count);
            Cards[card1idx].transform.Find("card-face").gameObject.GetComponent<Renderer>().material = Materials[matIdx];
            Cards.Remove(Cards[card1idx]);

            int card2idx = Random.Range(0, Cards.Count);
            Cards[card2idx].transform.Find("card-face").gameObject.GetComponent<Renderer>().material = Materials[matIdx];
            Cards.Remove(Cards[card2idx]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTimeFloat += Time.deltaTime;
        elapsedTimeText.SetText($"{(int)elapsedTimeFloat}");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
