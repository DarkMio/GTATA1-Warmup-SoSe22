using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    /// <summary>
    /// Controls the movement of the Character
    /// </summary>
    public class RunCharacterController : MonoBehaviour
    {
        public Transform Transform => character;
        public SpriteRenderer CharacterSprite => characterSprite;
        public Animator animator;
        /// <summary>
        /// Since the Character controller takes responsibility for triggering Input events, it also emits an
        /// event when it does so
        /// </summary>
        public Action onJump;
        
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpDuration;
        /// <summary>
        /// Unity handles Arrays and Lists in the inspector correctly (but not Maps, Dictionaries or other Collections)
        /// </summary>
        [SerializeField] private KeyCode[] jumpKeys;
        /// <summary>
        /// We don't require anything else from the Character than its transform
        /// </summary>
        [SerializeField] private Transform character;
        [SerializeField] private SpriteRenderer characterSprite;
        [SerializeField] private AnimationCurve jumpPosition;
        //added after
        public AudioSource audioSrc;
        int totalCharacter=1;
        public int currentCharacterIndex;
        public GameObject[] charList;
        public GameObject characters;
        public GameObject currentChar;
        public Button previousChar;
        public Button nextChar;

        private bool canJump = true;

        /// <summary>
        /// Update is a Unity runtime function called *every rendered* frame before Rendering happens
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>

        void Start(){
            totalCharacter=characters.transform.childCount; //get the number of chars avaible
            charList= new GameObject[totalCharacter]; //setting the length
            
            for (int i=0; i<totalCharacter; i++)
            {
                charList[i]=characters.transform.GetChild(i).gameObject;
                charList[i].SetActive(false);
            }
            charList[0].SetActive(true);
            currentChar=charList[0];
            currentCharacterIndex=0;
            animator=charList[0].GetComponent<Animator>();//just charList[0] wont work because the context is in gameobject 
            character=charList[0].transform;
            characterSprite=charList[0].GetComponent<SpriteRenderer>();

            Button prevBtn = previousChar.GetComponent<Button>();
            Button nextBtn = nextChar.GetComponent<Button>();
            prevBtn.onClick.AddListener(PrevClicked);
            nextBtn.onClick.AddListener(NextClicked);
        }
        private void Update()
        {
            //Debug.Log("total char"+totalCharacter);
            if (!canJump)
            {
                return;
            }
            // here the input event counts - if there is any button pressed that were defined as jump keys, trigger a jump
            if (jumpKeys.Any(x => Input.GetKeyDown(x)))
            {   // first we disable the jump, then start the Coroutine that handles the jump and invoke the event
                canJump = false;
                audioSrc.Play();
                StartCoroutine(JumpRoutine());
                onJump?.Invoke();
               animator.SetBool("isjumping", true);
               //animator.SetBool("isjumping", false);
            }
        }
       
        void PrevClicked()
    {
        
        Debug.Log("You have clicked the PREV!");
        Debug.Log("BEFORE"+currentCharacterIndex);
        charList[currentCharacterIndex].SetActive(false);
        if(currentCharacterIndex==0){
            currentCharacterIndex=totalCharacter-1;
        }else{
        currentCharacterIndex -=1;
        }
        Debug.Log("AFTER"+currentCharacterIndex);
        charList[currentCharacterIndex].SetActive(true);
        animator=charList[currentCharacterIndex].GetComponent<Animator>();//just charList[0] wont work because the context is in gameobject 
            character=charList[currentCharacterIndex].transform;
            characterSprite=charList[currentCharacterIndex].GetComponent<SpriteRenderer>();
    }
        void NextClicked()
    {
        Debug.Log("You have clicked the NEXT!");
        Debug.Log("BEFORE"+currentCharacterIndex);
        charList[currentCharacterIndex].SetActive(false);
        if(currentCharacterIndex==2){
            currentCharacterIndex=totalCharacter-totalCharacter;
        }else{
        currentCharacterIndex +=1;
        }
        Debug.Log("AFTER"+currentCharacterIndex);
        charList[currentCharacterIndex].SetActive(true);
        animator=charList[currentCharacterIndex].GetComponent<Animator>();//just charList[0] wont work because the context is in gameobject 
            character=charList[currentCharacterIndex].transform;
            characterSprite=charList[currentCharacterIndex].GetComponent<SpriteRenderer>();
    }
        /// <summary>
        /// OnDrawGizmosSelected is a Unity editor function called when the attached GameObject is selected and used to
        /// display debugging information in the Scene view
        /// see: https://docs.unity3d.com/Manual/ExecutionOrder.html
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            var upScale = transform.lossyScale;
            upScale.Scale(transform.up);
            Gizmos.DrawLine(transform.position, Vector3.up * jumpHeight * upScale.magnitude);
        }

        /// <summary>
        /// Handles the jump of a character
        /// 
        /// To be used in an Coroutine, this function is a generator (return IEnumerator) and has special syntactic
        /// sugar with "yield return"
        /// </summary>
        private IEnumerator JumpRoutine()
        {
            // the time this coroutine runs
            var totalTime = 0f;
            // low position is assumed to be a (0, 0, 0)
            var highPosition = character.up * jumpHeight;
            while (totalTime < jumpDuration)
            {
                totalTime += Time.deltaTime;
                // what's the normalized time [0...1] this coroutine runs at
                var sampleTime = totalTime / jumpDuration;
                // Lerp is a Linear Interpolation between a...b based on a value between 0...1
                character.localPosition = Vector3.Lerp(Vector3.zero, highPosition, jumpPosition.Evaluate(sampleTime));
                // we enable jumping again after we're almost done to remove some "stuck" behaviour when landing down
                if (sampleTime > 0.95f)
                {
                    canJump = true;
                    animator.SetBool("isjumping", false);
                }
                
                // yield return null waits a single frame
                yield return null;
            }
        }
    }
}