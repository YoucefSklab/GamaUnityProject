using System.Collections;
using UnityEngine;
using ummisco.gama.unity.messages;


namespace ummisco.gama.unity.topics
{
    public class MoveTopic : Topic
    {

        public Rigidbody rb;
        public float inverseMoveTime;
        public float moveTime = 10000.1f;
        public MoveTopicMessage topicMessage;

        public MoveTopic(TopicMessage currentMsg, GameObject gameObj) : base(gameObj)
        {

        }

        // Use this for initialization
        public override void Start()
        {
            inverseMoveTime = 1f / moveTime;

        }

        // Update is called once per frame
        public override void Update()
        {

        }

        public void ProcessTopic(object obj)
        {
            SetAllProperties(obj);

            rb = targetGameObject.GetComponent<Rigidbody>();

            Vector3 movement = topicMessage.position.toVector3D();

            SendTopic(movement);

        }

        // The method to call Game Objects methods
        //----------------------------------------
        public void SendTopic(Vector3 movement)
        {

            if (topicMessage.smoothMove)
            {
                smoothMoveToPosition(movement, topicMessage.speed);
            }
            else
            {
                freeMoveToPosition(movement, topicMessage.speed);
            }
        }

        public void freeMoveToPosition(Vector3 position, float speed)
        {
            rb.AddForce(position * speed);
        }

        public void smoothMoveToPosition(Vector3 position, float speed)
        {

            //Store start position to move from, based on objects current transform position.
            Vector3 start = targetGameObject.transform.position;

            // Calculate end position based on the direction parameters passed in when calling Move.
            Vector3 end = start + position;

            StartCoroutine(SmoothMovement(end, speed));
        }

        //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
        protected IEnumerator SmoothMovement(Vector3 end, float speed)
        {
            //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
            //Square magnitude is used instead of magnitude because it's computationally cheaper.
            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            inverseMoveTime = 100f;

            //While that distance is greater than a very small amount (Epsilon, almost 
            while (sqrRemainingDistance > 0.1f)
            {
                //Find a new position proportionally closer to the end, based on the moveTime
                //Vector3 newPostion = Vector3.MoveTowards (rb.position, end, inverseMoveTime * Time.deltaTime);
                Vector3 newPostion = Vector3.MoveTowards(rb.position, end, speed * Time.deltaTime);

                //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
                rb.MovePosition(newPostion);

                //Recalculate the remaining distance after moving.
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;

                //Return and loop until sqrRemainingDistance is close enough to zero to end the function
                yield return null;
            }
            Debug.Log("Position Set! ");
            rb.velocity = Vector3.zero;
            rb.transform.position = end;
            //rb.angularVelocity = Vector3.zero;
            //rb.Sleep ();
        }

        public override void SetAllProperties(object args)
        {
            object[] obj = (object[])args;
            this.topicMessage = (MoveTopicMessage)obj[0];
            this.targetGameObject = (GameObject)obj[1];
        }
    }
}