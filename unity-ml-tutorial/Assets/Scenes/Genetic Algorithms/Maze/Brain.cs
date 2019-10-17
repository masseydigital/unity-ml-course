using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class Brain : MonoBehaviour
    {
        public Dna dna;
        public GameObject eyes;
        public float distTravelled = 0;

        private int dnaLength = 2;
        private bool seeWall = true;
        private Vector3 startPosition;
        private bool alive = true;

        #region Unity Methods
        private void OnCollisionEnter(Collision collision)
        {
           if(collision.gameObject.tag == "dead")
           {
                distTravelled = 0;
                alive = false;
           }
        }

        // Update is called once per frame
        void Update()
        {
            if (!alive) return;

            seeWall = false;
            RaycastHit hit;
            Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 0.5f, Color.red);
            if (Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.forward, out hit, 0.5f))
            {
                if (hit.collider.gameObject.tag == "wall")
                {
                    seeWall = true;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!alive) return;

            // read dna
            float h = 0;
            float v = dna.GetGene(0);

            if (seeWall)
            {
                h = dna.GetGene(1);
            }

            this.transform.Translate(0, 0, v * 0.001f);
            this.transform.Rotate(0, h, 0);
            distTravelled = Vector3.Distance(startPosition, this.transform.position);
        }
        #endregion Unity Methods

        #region Methods
        // initialize dna
        // 0 forward
        // 1 angle turn
        public void Init()
        {
            dna = new Dna(dnaLength, 360);
            startPosition = this.transform.position;
        }
        #endregion Methods
    }
}


