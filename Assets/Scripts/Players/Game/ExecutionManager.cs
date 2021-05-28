using System.Collections;
using UnityEngine;
namespace PlayerSpace.Game
{
    public class ExecutionManager : MonoBehaviour
    {
        Animator animator;
        Model model;
        PlayerCharacter player;
        Control playerControl;
        bool isExecuting = false;
        [SerializeField] bool testMode = false;
        [SerializeField] int testState = 0;
        [SerializeField] Transform weapon;
        private void Awake()
        {
            player = GetComponent<PlayerCharacter>();
            model = GetComponent<Model>();
            animator = GetComponent<Animator>();
            playerControl = GetComponent<Control>();

        }
        public int GetState()
        {
            return testState;
        }
        public void DoExecution()
        {
            if (model.PlayerState == PlayerState.Hunter || (testMode && testState == 0))
                StartCoroutine(Executions());
        }
        public void Executed()
        {
            if (model.PlayerState == PlayerState.Dead || (testMode && testState == 1))
            {

                int health = model.CurrentHealth;
                if (health <= 0)
                {
                    animator.SetTrigger("execution");

                    playerControl.Mutate();//player.GetCaught();
                }
            }
        }
        public bool IsExecuting()
        {
            return isExecuting;
        }
        public IEnumerator Executions()
        {
            Collider2D[] list = new Collider2D[10];
            ContactFilter2D contact = new ContactFilter2D();
            contact.SetLayerMask(LayerMask.NameToLayer("Default"));
            animator.SetTrigger("execution");

            yield return new WaitForSecondsRealtime(0.5f);
            list = Physics2D.OverlapCircleAll(weapon.position, 1.5f);
            foreach (var item in list)
            {
                if (item.gameObject.name == "Body")
                {
                    item.GetComponent<ExecutionManager>().Executed();
                }
            }

        }
    }
}