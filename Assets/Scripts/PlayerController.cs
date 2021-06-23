using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");

        transform.eulerAngles += new Vector3(0, x, 0);

        var head = transform.Find("Head");
        if (head != null) {
            head.transform.eulerAngles += new Vector3(-y, 0, 0);
        }
    }
}
