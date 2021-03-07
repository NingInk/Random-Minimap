using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public RandomRoom rr;
    public GameObject cam;
    CharacterController cc;
    [Range(0f, 1f)]
    public float speed = 1;
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        GetComponent<Renderer>().material.color = Color.black;
    }

    void Update()
    {
        float hh = Input.GetAxis("Horizontal");
        float vv = Input.GetAxis("Vertical");

        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<Renderer>().enabled)
            {

                if (!rr.prePos.ContainsKey(hit.collider.transform.position + Vector3.forward))
                {
                    if (((hit.collider.transform.position + Vector3.forward).z - transform.position.z) < 0.6f)
                    {
                        if (vv > 0)
                        {
                            vv = 0;
                        }
                    }
                }
                if (!rr.prePos.ContainsKey(hit.collider.transform.position - Vector3.forward))
                {
                    if ((transform.position.z - (hit.collider.transform.position - Vector3.forward).z) < 0.6f)
                    {
                        if (vv < 0)
                        {
                            vv = 0;
                        }
                    }
                }
                if (!rr.prePos.ContainsKey(hit.collider.transform.position + Vector3.right))
                {

                    if (((hit.collider.transform.position + Vector3.right).x - transform.position.x) < 0.6f)
                    {
                        if (hh > 0)
                        {
                            hh = 0;
                        }
                    }

                }

                if (!rr.prePos.ContainsKey(hit.collider.transform.position - Vector3.right))
                {
                    if ((transform.position.x - (hit.collider.transform.position - Vector3.right).x) < 0.6f)
                    {
                        if (hh < 0)
                        {
                            hh = 0;
                        }
                    }
                }

            }
        }
        

        Vector3 move = new Vector3(hh, 0, vv);
        cc.Move(move * speed);
    }
    private void LateUpdate()
    {
        cam.transform.position = (transform.position + (Vector3.up * 10));
    }
}
