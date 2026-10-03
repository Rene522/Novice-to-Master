using UnityEngine;

public class Journal : MonoBehaviour
{
    public Page[] pages;
    public int currentPage;
    public string status = "None";

    public Camera JournalCam;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //On click, if the mouse is on the left of the book select the left page, else select the right
            if (JournalCam.ScreenToWorldPoint(Input.mousePosition).x < 0)
            {
                SetPageTurning(currentPage, true);
                status = "Left";
            }
            else
            {
                SetPageTurning(currentPage+1, true);
                status = "Right";
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //Makes both pages return to their resting spot/full size
            SetPageTurning(currentPage, false);
            SetPageTurning(currentPage+1, false);

            if (status == "Right" && JournalCam.ScreenToWorldPoint(Input.mousePosition).x < 0)
            {
                //If the player has dragged a page past the half way point, adjust the current page
                UpdateFlip(false);
            }
            else if (status == "Left" && JournalCam.ScreenToWorldPoint(Input.mousePosition).x > 0)
            {
                //i.e. dragged the right page to the left or left page to the right
                UpdateFlip(true);
            }

            status = "None";
        }
    }


    public void UpdateFlip(bool leftTurned)
    {
        //left turned is the left page going to the right,current page -1
        //current page -1 to current page + 2 need to be active

        if (leftTurned)
        {
            if (currentPage >= 0)
            {
                currentPage--;
            }
            //Deactivates and Activates pages based on what direction the pages were turned
            SetPageActive(currentPage + 3, false);
            SetPageActive(currentPage - 1, true);

            UpdateSortOrders();
        }
        else
        {
            if (currentPage <= pages.Length)
            {
                currentPage++;
            }
            //And again for right :>
            SetPageActive(currentPage + 2, true);
            SetPageActive(currentPage - 2, false);

            UpdateSortOrders();
        }
    }

    //Following pages are mostly to make sure there's not constant error messages from out of bounds arrays,
    //they basically all check if position is within bounds + exists, and then does a specific action
    public void SetPageActive(int number, bool isActive)
    {
        print(number + " " + isActive);
        if (number >= 0 && number < pages.Length)
        {
            if (pages[number] != null)
            {
                pages[number].gameObject.SetActive(isActive);
            }
        }
    }

    public void SetPageTurning(int number, bool isTurning)
    {
        if (number >= 0 && number < pages.Length)
        {
            if (pages[number] != null)
            {
                pages[number].isTurning = isTurning;
            }
        }
    }

    public void SetPageBehind(int number, bool isBehind)
    {
        if (number >= 0 && number < pages.Length)
        {
            if (pages[number] != null)
            {
                pages[number].isBehind = isBehind;
            }
        }
    }

    public void UpdateSortOrders()
    {
        //clunky but it updates the "IsBehind" variable which makes sure the pages are rendered in the right order
        SetPageBehind(currentPage + 2, true);
        SetPageBehind(currentPage + 1, false);
        SetPageBehind(currentPage, false);
        SetPageBehind(currentPage - 1, true);
    }
}
