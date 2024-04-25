using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement{

    public enum Direction { UP, DOWN, LEFT, RIGHT};

    public class PlayerMovement : MonoBehaviour
    {
        public float speed;
        private Vector2 direction, currentPosition;
        private Animator animator;
        public static Direction lastDirection;
        private SpriteRenderer m_renderer;
        private Rigidbody2D m_rigidbody;

#region Unity Functions

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            m_renderer = GetComponent<SpriteRenderer>();
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {

            direction = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
            m_rigidbody.velocity = direction*speed;

            UpdateAnim();
        }

        void Update(){
            if(Input.GetKey("space")){
                Debug.Log("direction: " + direction);
                Debug.Log(direction.x - currentPosition.x);
            }
        }
#endregion

#region Private Functions

        private void UpdateAnim(){

            float distanceX = direction.x - currentPosition.x;
            float distanceY = direction.y - currentPosition.y;

            if(distanceX > 0){ //is going to the right
                animator.Play("Player_WalkLeft");
                m_renderer.flipX = true;
                lastDirection = Direction.RIGHT;
            }
            else if (distanceX < 0){//is going to the left
                animator.Play("Player_WalkLeft");
                m_renderer.flipX = false;
                lastDirection = Direction.LEFT;
            }
            else{
                if(distanceY > 0){//is going up
                    animator.Play("Player_WalkBack");
                    lastDirection = Direction.UP;
                }
                else if (distanceY < 0){// is going down
                    animator.Play("Player_WalkForward");
                    lastDirection = Direction.DOWN;
                }
            }

            

            if(distanceX == 0 && distanceY == 0){//is not moving
                switch(lastDirection){
                    case Direction.LEFT:
                        animator.Play("player_IdleLeft");
                        m_renderer.flipX = false;
                    break;

                    case Direction.RIGHT:
                        animator.Play("player_IdleLeft");
                        m_renderer.flipX = true;
                    break;

                    case Direction.UP:
                        animator.Play("Player_Idle_Back");
                    break;

                    case Direction.DOWN:
                        animator.Play("Player_Idle_anim");
                    break;

                    default:
                        animator.Play("Player_Idle_anim");
                    break;
                }
            }

        }

#endregion

    }
}
