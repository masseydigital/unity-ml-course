using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class Brain : MonoBehaviour
{
    #region Variables
    public int dnaLength = 1;
    public float timeAlive;
    public Dna dna;

    private ThirdPersonCharacter m_Character;
    private Vector3 m_Move;
    private bool m_Jump;
    private bool alive = true;
    #endregion Variables

    #region Untiy Methods
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read DNA
        float h = 0;
        float v = 0;
        bool crouch = false;
        if (dna.GetGene(0) == 0) v = 1;
        else if (dna.GetGene(0) == 1) v = -1;
        else if (dna.GetGene(0) == 2) h = -1;
        else if (dna.GetGene(0) == 3) h = 1;
        else if (dna.GetGene(0) == 4) m_Jump = true;
        else if (dna.GetGene(0) == 5) crouch = true;

        m_Move = v * Vector3.forward + h * Vector3.right;
        m_Character.Move(m_Move, crouch, m_Jump);
        m_Jump = false;
        if (alive) timeAlive += Time.deltaTime;
    }

    // When a collision starts
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "dead")
        {
            alive = false;
        }
    }
    #endregion Unity Methods

    #region Methods
    /// <summary>
    /// Initialize DNA
    /// 0 forward
    /// 1 back
    /// 2 left
    /// 3 right
    /// 4 jump 
    /// 5 crouch
    /// </summary>
    public void Init()
    {
        dna = new Dna(dnaLength, 6);
        m_Character = GetComponent<ThirdPersonCharacter>();
        timeAlive = 0;
        alive = true;
    }
    #endregion Methods
}
