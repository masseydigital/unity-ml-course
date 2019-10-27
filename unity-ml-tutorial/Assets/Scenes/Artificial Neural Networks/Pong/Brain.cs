using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong
{
    public class Brain : MonoBehaviour
    {
        public GameObject paddle;
        public GameObject ball;
        Rigidbody2D brb;
        float yvel;
        float paddleMinY = 8.7f;
        float paddleMaxY = 17.5f;
        float paddleMaxSpeed = 15;
        public float numSaved = 0;
        public float numMissed = 0;

        ANN ann;

        private void Awake()
        {
            brb = ball.GetComponent<Rigidbody2D>();
        }

        // Start is called before the first frame update
        void Start()
        {
            ann = new ANN(6, 1, 1, 4, 0.11);
        }

        // Update is called once per frame
        void Update()
        {
            // Keep the paddle on the court
            float posy = Mathf.Clamp(paddle.transform.position.y + (yvel * Time.deltaTime * paddleMaxSpeed),
                paddleMinY, paddleMaxY);

            paddle.transform.position = new Vector3(paddle.transform.position.x, posy, paddle.transform.position.z);

            List<double> output = new List<double>();
            int layerMask = 1 << 10;
            RaycastHit2D hit = Physics2D.Raycast(ball.transform.position, brb.velocity, 1000, layerMask);

            // make sure we are hitting something
            if (hit.collider != null)
            {
                if(hit.collider.gameObject.tag == "tops") // learn the reflection off the top
                {
                    Vector3 reflection = Vector3.Reflect(brb.velocity, hit.normal);
                    hit = Physics2D.Raycast(hit.point, reflection, 1000, layerMask);
                }

                // checking for hit again since we are performing another raycast (the ball goes straight to the backwall)
                if (hit.collider != null && hit.collider.gameObject.tag == "backwall")
                {
                    float dy = (hit.point.y - paddle.transform.position.y);

                    output = Run(ball.transform.position.x,
                        ball.transform.position.y,
                        brb.velocity.x, brb.velocity.y,
                        paddle.transform.position.x,
                        paddle.transform.position.y,
                        dy, true);

                    yvel = (float)output[0];
                }
            }
            else
            {
                yvel = 0;
            }
        }

        List<double> Run(double bx, double by, double bvx, double bvy, double px, double py, double pv, bool train)
        {
            List<double> inputs = new List<double>();
            List<double> outputs = new List<double>();
            inputs.Add(bx);
            inputs.Add(by);
            inputs.Add(bvx);
            inputs.Add(bvy);
            inputs.Add(px);
            inputs.Add(py);
            outputs.Add(pv);

            if (train)
                return (ann.Train(inputs, outputs));
            else
                return (ann.CalcOutput(inputs, outputs));
        }
    }
}
