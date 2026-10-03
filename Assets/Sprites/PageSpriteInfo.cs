using UnityEngine;

public class PageSpriteInfo : MonoBehaviour
{
    //I cannot find the option to create a text file
    //SO im being lazy and making it a script </3


    //For importing new page assets, they should be imported with the spine on the page aligned to the right
    //Or thinking about it as one of 2 pages on a book, it would be the left page
    //In the case that you're actually designing the page that would be on the right, it needs to be flipped/mirrored
    //Unless you can find a way to do so, i think you gotta do this out of engine, sorry :c

    //The pivot of the sprite must be set to bottom right (thinking about it bottom might not be required? but RIGHT alignment is a must)

    //For adding them to the PAGE script, go to the Page Sprite list
    //Element 0 (or the first item in the list) is the RIGHT hand page (the one thats weird and mirrored)
    //Element 1 (or the second item in the list) is the LEFT hand page (the one thats NOT flipped)

    //Important to get these the right way around otherwise you'll end up with 2 unreadable, inverted pages!
    //Also good to note that adding sprites to this list does nothing, dont press the + and do NOT remove an element, that will break a lot of things!!!


    //If theres any issues feel free to contact me :> 

    //Theres also a fair chance that the sizing will be horribly off, i hate trying to size things in unity its always wrong ToT
}
