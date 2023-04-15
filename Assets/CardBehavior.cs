using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    // Reference to game manager for whatever reason
    public GameManager gm;

    /// Animator for rotating cards
    [SerializeField]
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnMouseOver()
    {
        // If mouse is over card and is clicked, reveal card if it can be revealed (no other two cards are revealed)
        if (Input.GetMouseButtonDown(0))
        {
            if (!animator.GetBool("IsRevealed") && gm.RevealCard(this))
            {
                animator.SetBool("IsRevealed", true);
            }
        }
    }

    public void Hide()
    {
        animator.SetBool("IsRevealed", false);
    }
}
