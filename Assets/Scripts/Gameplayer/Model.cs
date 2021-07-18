using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class Model : MonoBehaviour
    {
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();

            SetVariables();
        }

        private void SetVariables()
        {
            CurrentPlayerState = new PlayerState();
            CurrentFrontState = new FrontState();
            CurrentGroundState = new GroundState();

            selfTransform = transform;
            distToGround = GetComponent<Collider2D>().bounds.extents.y;
        }

        [HideInInspector]
        public Rigidbody2D rb;
        [HideInInspector]
        public Animator anim;

        #region Enum
        public PlayerState CurrentPlayerState { get; set; }
        public FrontState CurrentFrontState { get; set; }
        public GroundState CurrentGroundState { get; set; }
        #endregion

        #region Transform
        public Transform selfTransform;
        public float characterSize = 1f;
        #endregion

        #region Movement
        public float distToGround = 0;
        public float distToGroundOffset = 0;
        public float distToWall = 0;
        public float distToWallOffset = 0;
        public float moveSpeed = 10f;
        public float jumpTime = 0.3f;
        public float jumpForce = 15f;
        public Vector2 wallJumpForce = new Vector2(12, 20);
        public float wallJumpTime = 0.2f;
        #endregion

        #region Layer
        public LayerMask whatIsGround;
        public LayerMask whatIsIceGround;
        public LayerMask whatIsSlimeGround;
        public LayerMask whatIsBox;
        #endregion

        #region Gains
        public float speedGain = 1f;
        public float jumpGain = 1f;
        public float playerStateSpeedGain = 1f;
        public float playerStateJumpGain = 1f;
        public float itemSpeedGain = 1f;
        public float itemJumpGain = 1f;
        public float groundSpeedGain = 1f;
        public float groundJumpGain = 1f;
        public float wallSlideGain = 1f;
        public float wallSpeedGain = 1f;
        public float wallJumpGain = 1f;
        public float slideSpeedGain = 1f;
        #endregion

        #region StartItemRelated
        public bool hasGotStartItem = false;
        public bool isShielding = false;
        #endregion

        #region StartItems
        /// <summary>
        /// Ignore Ice material ground when moving
        /// </summary>
        public bool iceSkate = false;
        /// <summary>
        /// Ignore slime material ground when moving
        /// </summary>
        public bool slimeShoe = false;
        /// <summary>
        /// Jump higher than usual
        /// </summary>
        public bool swiftnessBoot = false;
        /// <summary>
        /// Able to jump again in the air
        /// </summary>
        public bool rocketShoe = false;
        /// <summary>
        /// Avoid being attacked once then CD for secs.
        /// </summary>
        public bool shield = false;
        /// <summary>
        /// More endurance on running
        /// </summary>
        public bool energyDrink = false;
        /// <summary>
        /// Extra Life, escaping from execuation once
        /// </summary>
        public bool extraLife = false;
        /// <summary>
        /// Higher hp, lower speed
        /// </summary>
        public bool armor = false;
        /// <summary>
        /// Inspector is able to escape and get score
        /// </summary>
        public bool inspectorsChance = false;
        /// <summary>
        /// After revival, get higher speed for secs
        /// </summary>
        public bool deathWithStronger = false;
        /// <summary>
        /// Get more score when escape
        /// </summary>
        public bool extralScore = false;
        /// <summary>
        /// Nothing helpful but just a balloon
        /// </summary>
        public bool balloon = false;
        #endregion
    }

}