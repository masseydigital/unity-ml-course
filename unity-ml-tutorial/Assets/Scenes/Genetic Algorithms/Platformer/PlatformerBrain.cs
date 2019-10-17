using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerBrain : MonoBehaviour
{
    public float timeAlive;
    public float timeWalking;
    public Dna dna;
    public GameObject eyes;
    public GameObject humanPrefab;

    private int dnaLength = 2;   // number of decisions to make
    private bool alive = true;
    private bool seeGround = true;
    private GameObject humanInstance;

    #region Unity Methods
    private void OnDestroy()
    {
        Destroy(humanInstance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "dead")
        {
            // If we die, we want to wipe out any info we've collected
            alive = false;
            timeAlive = 0;
            timeWalking = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive) return;

        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red, 10);
        seeGround = false;
        RaycastHit hit;
        if(Physics.Raycast(eyes.transform.position, eyes.transform.forward * 10, out hit))
        {
            if(hit.collider.gameObject.tag == "platform")
            {
                seeGround = true;
            }
        }

        timeAlive = PlatformPopulationManager.elapsed;

        // read Dna
        float turn = 0;
        float move = 0;

        if(seeGround)
        {
            // make v relative to charcter and always move forward
            if (dna.GetGene(0) == 0) { move = 1; timeWalking += 1; }
            else if (dna.GetGene(0) == 1) turn = -90;
            else if (dna.GetGene(0) == 2) turn = 90;
        }
        else
        {
            if (dna.GetGene(1) == 0) { move = 1; timeWalking += 1; }
            else if (dna.GetGene(1) == 1) turn = -90;
            else if (dna.GetGene(1) == 2) turn = 90; 
        }

        this.transform.Translate(0, 0, move * 0.1f);
        this.transform.Rotate(0, turn, 0);
    }
    #endregion Unity Methods

    #region Methods
    // Initialize Dna
    // 0 forward
    // 1 left
    // 2 right
    public void Init()
    {
        dna = new Dna(dnaLength, 3);
        timeAlive = 0;
        alive = true;

        humanInstance = Instantiate(humanPrefab, this.transform.position, this.transform.rotation);
        humanInstance.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = this.transform;
    }
    #endregion Methods
}
