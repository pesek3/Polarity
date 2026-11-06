using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{   
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();

        if (mask.value == 0)
        {
            mask = LayerMask.GetMask("Interactable");
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty); //vycisti text na UI

        Ray ray = new Ray(cam.transform.position, cam.transform.forward); //vytvori ray z pozice kamery a smerem dopredu
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red); //vykresli ray v editoru    
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask)) //vykresli raycast a ulozi informace o tom co raycast zasahl do hitinfo
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null) //pokud raycast zasahl do objektu s komponentou Interactable
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage); //vypise do konzole zpravu z komponenty Interactable
                if (inputManager.OnFoot.Interact.triggered) //pokud je stisknuto interakce
                {
                    interactable.BaseInteract(); //vola BaseInteract() metodu z komponenty Interactable
                }
            }
        }
    }
}
  