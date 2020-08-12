using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    // Fade the color from red to green

    // The Scripts sits on CuSO4, so we render the other 3 major components:
    // FeSO4, Iron Nail, Copper Nail
    public Renderer FeSO4, CopperNail, IronNail;

    // To render animations for the nails and corks
    public Animator Nail1, Nail2, Cork1, Cork2;
    
    // All the various colors required
    Color startSolColor, endSolColor, startNailColor, endNailColor, copyStartSolColor, copyStartNailColor;
    
    Material[] mats;
    
    // Duration of reaction
    float duration = 20.0f;
    
    // The duration is scaled down from 0 to 1. 
    //Hence keeping a value of t = 2.0f will not start the reaction unless tapped.
    float t = 2.0f;

    // Flag to signal if one is in the middle of the reaction
    bool playing;
    
   
    void Start()
    {
        // Animation/Reaction not occuring in the beginning
        playing = false;

        // Iron Nail will finally have 2 materials: Copper and Iron. Accessing the Copper Part to change its color.
        mats = IronNail.materials;
        
        // Starting Color of CuSO4
        startSolColor = GetComponent<Renderer>().material.color;
        
        // To keep a copy of Starting Color of CuSO4
        copyStartSolColor = startSolColor;

        // End Color of CuSO4 will be that of FeSO4 solution
        endSolColor = FeSO4.material.color;
        
        // Starting Color of Iron Nail
        startNailColor = mats[0].color;

        // To keep a copy of starting color of Iron Nail
        copyStartNailColor = startNailColor;

        // End Color of 'a part of' the Iron Nail will be that of the Copper Nail
        endNailColor = CopperNail.material.color;
    }
    // Update is called once per frame
    void Update()
    {
        // If 't' has reached 1, it signifies end of reaction.
        // We can tap again to start from the beginning
        if((int)t == 1) {

            // Reaction has come to a stop
            playing = false;
            startSolColor = copyStartSolColor;
            startNailColor = copyStartNailColor;

            // Triggers to stop animation
            Cork1.SetTrigger("End");
            Cork2.SetTrigger("End");
            Nail2.SetTrigger("End"); 
            Nail1.SetTrigger("End");
            t = 2.0f; // Any value greater than 1 will prevent reaction from taking place unless tapped
        }

        // If 't' is less than 1, it gradually moves towards 1,
        // and the color of both CuSO4 and Iron Nail changes gradually with time
        if (t < 1 && playing) {
           
           //Debug.Log(t);
           t += Time.deltaTime / duration;

           // Change solution color 
           GetComponent<Renderer>().material.color = Color.Lerp(startSolColor, endSolColor, t);
           
            // Change Nail color
           mats[0].color = Color.Lerp(startNailColor, endNailColor, t);
           IronNail.materials = mats; 
        }

        // When a user taps, there are two situations:
        //1. The Reaction is not completed, in which case the user sees the progress of the reaction.
        //2. The Reaction has ended/not yet begun.
        if ( (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)) {
            
            // To Check Progress
            if (playing)
                {
                    playing = false;
                    startSolColor = GetComponent<Renderer>().material.color;
                    startNailColor = mats[0].color;
                    Cork1.SetTrigger("End");
                    Cork2.SetTrigger("End");
                    Nail2.SetTrigger("End");
                    Nail1.SetTrigger("End");
                    t = 2.0f;
                }
            // To Start from beginning
                else
                {
                    playing = true;
                    Nail2.SetTrigger("Start");
                    Nail1.SetTrigger("Start");
                    Cork2.SetTrigger("Start");
                    Cork1.SetTrigger("Start");
                    t = -0.1f; // To allow for some delay due to cork + nail animation
                }
            } 
    }
}
