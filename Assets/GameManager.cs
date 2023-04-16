using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Main Logic Items
    /// Currently revealed cards (for finding pairs)
    public int RevealedCards { get; set; }
    /// Amount of target pairs to find to win the game
    public int TargetPairs { get; set; }
    /// Currently found pairs in game
    public int FoundPairs { get; set; }

    public int NumberOfTurns { get; set; }
    #endregion

    #region Points System
    /// Total points the player has aquired
    public float TotalPoints { get; set; }

    #endregion

    #region Cards
    [Header("Cards")]
    public CardBehavior card1;
    public CardBehavior card2;
    public GameObject CardHolder;

    #endregion

    #region UI
    [Header("UI Stuff")]
    [SerializeField]
    public TextMeshProUGUI WinScreenText; // For writing congrats
    [SerializeField]
    public TextMeshProUGUI elapsedTimeTMP; // For increasing and writing elapsed time in UI
    [SerializeField]
    public TextMeshProUGUI NumberOfTurnsTMP;
    [SerializeField]
    public GameObject WinScreen; // where to place all win stuff
    [SerializeField]
    public float elapsedTimeFloat = 0; // How much time has passed since restart
    [SerializeField]
    public GameObject GameRunningUIItems;

    #endregion

    #region Materials
    [Header("Materials")]
    [SerializeField]
    public List<Material> Materials = new List<Material>(); // All card faces
    private List<int> SelectedMaterials = new List<int>(); // All selected card faces (used in cards)

    #endregion

    #region All Functions

    // Start is called before the first frame update
    void Start()
    {
        // Hide winscreen on start
        WinScreen.SetActive(false);

        // Housekeeping, initialize items
        RevealedCards = 0;
        FoundPairs = 0;
        TotalPoints = 0;
        
        // Fetch all cards in game
        List<GameObject> Cards = new List<GameObject>();
        Cards.AddRange(GameObject.FindGameObjectsWithTag("Card"));

        // Initialize how many is needed to win - in case of reducing number of cards for debugging
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

            // Fetch random card from available cards
            int card1idx = Random.Range(0, Cards.Count);
            // Set its card-face components material to the random material selected from Materials
            Cards[card1idx].transform.Find("card-face").gameObject.GetComponent<Renderer>().material = Materials[matIdx];
            // Remove card from collection, as to not fetch it again
            Cards.Remove(Cards[card1idx]);

            // Repeat procedure - makes two random cards have same face
            int card2idx = Random.Range(0, Cards.Count);
            Cards[card2idx].transform.Find("card-face").gameObject.GetComponent<Renderer>().material = Materials[matIdx];
            Cards.Remove(Cards[card2idx]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Increase elapsed time if all target pairs found
        if (this.TargetPairs > this.FoundPairs)
        {
            elapsedTimeFloat += Time.deltaTime;
            elapsedTimeTMP.SetText($"{(int)elapsedTimeFloat}");
        }
    }
    
    /// <summary>
    /// Calls hide on all cardbehavior references, sets them to null and resets number of revealed cards
    /// </summary>
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
        // Check if two or more cards are already revealed, fail at revealing
        if (RevealedCards >= 2)
        {
            return false;
        }

        // Check which card is not revealed and set it
        if (card1 == null)
        {
            card1 = c;
        }
        else
        {
            // Increase number of turns when second card is revealed
            NumberOfTurns++;
            NumberOfTurnsTMP.SetText($"{NumberOfTurns}");
            card2 = c;
        }

        // Increase number of revealed cards
        RevealedCards++;

        // If two cards are revealed - process if faces are the same
        if (RevealedCards == 2)
        {
            // Check material names for revealed cards
            if (card1.transform.Find("card-face").gameObject.GetComponent<Renderer>().material.name ==
            card2.transform.Find("card-face").gameObject.GetComponent<Renderer>().material.name)
            {
                // Increase number of pairs, reset other items
                FoundPairs++;
                RevealedCards = 0;
                card1 = null;
                card2 = null;

                // Check if found pairs is the same as target, win if true
                if (FoundPairs == TargetPairs)
                {
                    Invoke("Win", 1);
                    //Win();
                }
            }
            else
            {
                // Invoke - call a procedure after enough time has passed (1sec) without blocking game
                Invoke("HideCards", 1);
            }
        }

        return true;
    }

    public void Win()
    {
        // Show the win screen and hide all cards by hiding the card holder
        WinScreen.SetActive(true);
        CardHolder.SetActive(false);
        GameRunningUIItems.SetActive(false);

        // Write congradulatory text
        this.WinScreenText.SetText($"Congratulations! It took you {(int)elapsedTimeFloat} seconds to finish!");
    }


    public void Restart()
    {
        // Reloading current scene effectively resets game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion
}
