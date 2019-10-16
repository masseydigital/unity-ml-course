/* Dna.cs
 * 
 * Brief:           This determines the genes and the behavior for a person GameObject
 * Author:          Drew Massey
 * Date Created:    10/15/2019
 */ 

using UnityEngine;

public class Dna : MonoBehaviour
{
    #region Variables
    // Gene for color
    public float r;
    public float g;
    public float b;
    public float timeToDie = 0;

    private bool dead = false;
    private SpriteRenderer sRenderer;
    private Collider2D sCollider;
    #endregion Variables

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        sCollider = GetComponent<Collider2D>();

        sRenderer.color = new Color(r, g, b);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Occurs when the collider is clicked on
    private void OnMouseDown()
    {
        dead = true;
        timeToDie = PopulationManager.elapsed;
        Debug.Log($"Dead at: {timeToDie}");
        sRenderer.enabled = false;
        sCollider.enabled = false;
    }
    #endregion Unity Methods
}
