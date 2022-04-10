using UnityEngine;
using UnityEngine.UI;

public class BilboardBubble : MonoBehaviour
{
    public Text dbg_text;
    // should be image

    public void SetFamily(int family)
    {
        dbg_text.text = family + "";
    }

    private void LateUpdate()
    {
        transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
    }
}
