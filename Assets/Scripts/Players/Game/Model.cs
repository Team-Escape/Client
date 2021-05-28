using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Game
{
    public class Model : MonoBehaviour
    {
        private void Awake()
        {
            if (GetComponent<SpriteRenderer>().material == null)
                GetComponent<SpriteRenderer>().material = material;
            PlayerState = new PlayerState();
            PlayerState = PlayerState.Waiting;
            GroundState = new GroundState();
            FrontState = new FrontState();
            JumpState = new JumpState();
            TeamID = -1;
            MaxHealth = 3;
        }

        public int TeamID { get; set; }

        public Material material = null;

        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        /// <summary>
        /// State
        /// </summary>
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        public PlayerState PlayerState { get; set; }
        public GroundState GroundState { get; set; }
        public FrontState FrontState { get; set; }
        // public MoveState MoveState { get; set; }
        public JumpState JumpState { get; set; }

        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        /// <summary>
        /// Attritubes
        /// </summary>
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        [SerializeField] int currentHealth = 3;
        public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
        [SerializeField] int maxHealth = 3;
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        [SerializeField] float endurance = 3f;
        public float Endurance { get { return endurance; } set { endurance = value; } }
        [SerializeField] float walkSpeed = 15f;
        public float WalkSpeed { get { return walkSpeed; } }
        [SerializeField] float jumpTime = 0.3f;
        public float JumpTime { get { return jumpTime; } }
        [SerializeField] float jumpForce = 15f;
        public float JumpForce { get { return jumpForce; } }
        [SerializeField] Vector2 wallJumpForce = new Vector2(10, 20);
        public Vector2 WallJumpForce { get { return wallJumpForce; } }
        [SerializeField] float wallJumpTime = 0.2f;//按下跳躍時，持續施力的時間
        public float WallJumpTime { get { return wallJumpTime; } }
        [SerializeField] float rebornDuration = 3f;
        public float RebornDuration { get { return rebornDuration; } }
        [SerializeField] float characterSize = 1f;
        public float CharacterSize { get { return characterSize; } set { characterSize = value; } }
        [SerializeField] float dashPower = 25f;
        public float DashPower { get { return dashPower; } set { dashPower = value; } }


        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        /// <summary>
        /// Check Stuffs
        /// </summary>
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        [SerializeField] Transform[] frontCheck = null;
        public Transform[] GetFrontCheck { get { return frontCheck; } }
        [SerializeField] Transform[] groundCheck = null;
        public Transform[] GetGroundCheck { get { return groundCheck; } }

        [SerializeField] float groundCheckDistance = 0.4f;
        public float GroundCheckDistance { get { return groundCheckDistance; } }
        [SerializeField] float frontCheckDistance = 0.1f;
        public float FrontCheckDistance { get { return frontCheckDistance; } }

        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        /// <summary>
        /// Ground/Wall Layer
        /// </summary>
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        [SerializeField] LayerMask whatIsGround = 0;
        public LayerMask GetGroundLayer { get { return whatIsGround; } }
        [SerializeField] LayerMask whatIsIceGround = 0;
        public LayerMask GetIceGroundLayer { get { return whatIsIceGround; } }
        [SerializeField] LayerMask whatIsSlimeGround = 0;
        public LayerMask GetSlimeGroundLayer { get { return whatIsSlimeGround; } }
        [SerializeField] LayerMask whatIsBox = 0;
        public LayerMask GetBoxLayer { get { return whatIsBox; } }


        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        /// <summary>
        /// Gains
        /// </summary>
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        public IEnumerator ItemSpeedUp(float value, float sec)
        {
            itemSpeedGain = value;
            yield return new WaitForSeconds(sec);
            itemSpeedGain = 1;
        }
        public IEnumerator ItemJumpUp(float value, float sec)
        {
            itemJumpGain = value;
            yield return new WaitForSeconds(sec);
            itemJumpGain = 1;
        }
        [SerializeField] float itemSpeedGain = 1;
        public float ItemSpeedGain { get { return itemSpeedGain; } set { itemSpeedGain = value; } }
        public float AddItemSpeedGain { set { itemSpeedGain += value; } }
        [SerializeField] float itemJumpGain = 1;
        public float ItemJumpGain { get { return itemJumpGain; } set { itemJumpGain = value; } }
        public float AddItemJumpGain { set { itemJumpGain += value; } }
        [SerializeField] float groundSpeedGain = 1;
        public float GroundSpeedGain { get { return groundSpeedGain; } set { groundSpeedGain = value; } }
        [SerializeField] float groundJumpGain = 1;
        public float GroundJumpGain { get { return groundJumpGain; } set { groundJumpGain = value; } }
        [SerializeField] float wallSlideGain = 1;
        public float WallSlideGain { get { return wallSlideGain; } set { wallSlideGain = value; } }
        [SerializeField] float wallSpeedGain = 1;
        public float WallSpeedGain { get { return wallSpeedGain; } set { wallSpeedGain = value; } }
        [SerializeField] float wallJumpGain = 1;
        public float WallJumpGain { get { return wallJumpGain; } set { wallJumpGain = value; } }
        [SerializeField] float slideSpeedGain = 1;
        public float SlideSpeedGain { get { return slideSpeedGain; } set { slideSpeedGain = value; } }



        public IEnumerator SpeedUpSec(float value, float sec)
        {
            speedGain = value;
            yield return new WaitForSeconds(sec);
            speedGain = 1;
        }
        public IEnumerator JumpUpSec(float value, float sec)
        {
            jumpGain = value;
            yield return new WaitForSeconds(sec);
            jumpGain = 1;
        }
        [SerializeField] float speedGain = 1;
        public float SpeedGain { get { return speedGain; } set { speedGain = value; } }
        public float AddSpeedGain { set { speedGain += value; } }
        [SerializeField] float jumpGain = 1;
        public float JumpGain { get { return jumpGain; } set { jumpGain = value; } }
        public float AddJumpGain { set { jumpGain += value; } }


        public float StateSpeedGain { get { return stateSpeedGain; } set { stateSpeedGain = value; } }
        [SerializeField] float stateSpeedGain = 1;
        public float StateJumpGain { get { return stateJumpGain; } set { stateJumpGain = value; } }
        [SerializeField] float stateJumpGain = 1;

        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        /// <summary>
        /// Private variables
        /// </summary>
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        private bool isFronted = false;
        public bool Fronted
        {
            get { return isFronted; }
            set { isFronted = value; }
        }

        private bool isIceFronted = false;
        public bool IceFronted
        {
            get { return isIceFronted; }
            set { isIceFronted = value; }
        }

        private bool isSlimeFronted = false;
        public bool SlimeFronted
        {
            get { return isSlimeFronted; }
            set { isSlimeFronted = value; }
        }

        private bool isGrounded = false;
        public bool Grounded
        {
            get { return isGrounded; }
            set { isGrounded = value; }
        }

        private bool isIceGrounded = false;
        public bool IceGrounded
        {
            get { return isIceGrounded; }
            set { isIceGrounded = value; }
        }

        private bool isSlimeGrounded = false;
        public bool SlimeGrounded
        {
            get { return isSlimeGrounded; }
            set { isSlimeGrounded = value; }
        }

        private bool isTouchingFront = false;
        public bool TouchFront
        {
            get { return isTouchingFront; }
            set { isTouchingFront = value; }
        }

        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        /// <summary>
        /// 開場道具
        /// </summary>
        ///█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        private bool isGetStartItem = false;
        public bool IsGetStartItem
        {
            get { return isGetStartItem; }
            set { isGetStartItem = value; }
        }

        private bool iceSkate = false;
        public bool IceSkate
        {
            get { return iceSkate; }
            set { iceSkate = value; }
        }

        private bool slimeShoe = false;
        public bool SlimeShoe
        {
            get { return slimeShoe; }
            set { slimeShoe = value; }
        }

        private bool shield = false;
        public bool Shield
        {
            get { return shield; }
            set { shield = value; }
        }

        private bool shielding = false;
        public bool Shielding
        {
            get { return shielding; }
            set { shielding = value; }
        }

        private bool energyDrink = false;
        public bool EnergyDrink
        {
            get { return energyDrink; }
            set { energyDrink = value; }
        }

        private bool crucifixion = false;
        public bool Crucifixion
        {
            get { return crucifixion; }
            set { crucifixion = value; }
        }

        private bool armor = false;
        public bool Armor
        {
            get { return armor; }
            set { armor = value; }
        }

        private bool lightnessShoe = false;
        public bool LightnessShoe
        {
            get { return lightnessShoe; }
            set { lightnessShoe = value; }
        }

        private bool rocketShoe = false;
        public bool RocketShoe
        {
            get { return rocketShoe; }
            set { rocketShoe = value; }
        }

        private bool developerObsession = false;
        public bool DeveloperObsession
        {
            get { return developerObsession; }
            set { developerObsession = value; }
        }

        private bool immortal = false;
        public bool Immortal
        {
            get { return immortal; }
            set { immortal = value; }
        }


        private bool balloon;
        public bool Balloon
        {
            get { return balloon; }
            set { balloon = value; }
        }

        private bool trophy = false;
        public bool Trophy
        {
            get { return trophy; }
            set { trophy = value; }
        }

        private bool detector = false;
        public bool Detector
        {
            get { return detector; }
            set { detector = value; }
        }

        private bool rocketJump = false;
        public bool RocketJump
        {
            get { return rocketJump; }
            set { rocketJump = value; }
        }
    }
}