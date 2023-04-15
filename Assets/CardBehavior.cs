using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    public GameManager gm;

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
