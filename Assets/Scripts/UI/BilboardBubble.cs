using UnityEngine;
using UnityEngine.UI;

public class BilboardBubble : MonoBehaviour
{
    public Text dbg_text;
    // should be image

    public GameObject[] box_family_images;

    public void SetFamily(int family)
    {
        //dbg_text.text = family + "";

        for (int i = 0; i < box_family_images.Length; i++)
        {
            if (i == family)
            {
                box_family_images[i].gameObject.SetActive(true);
            }
            else
            {
                box_family_images[i].gameObject.SetActive(false);
            }
        }

    }

    private void LateUpdate()
    {
        transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
    }
}
