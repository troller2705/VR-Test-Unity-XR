using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : MonoBehaviour
{
    public enum AttachmentType { Scope, Barrel, Grip }
    public AttachmentType attachmentType;

    private void OnTriggerEnter(Collider other)
    {
        GunAttachmentSystem gun = other.GetComponentInParent<GunAttachmentSystem>();
        if (gun != null && gun.Attach(gameObject))
        {
            // Successfully attached
            Destroy(this); // Remove the grabbable component
        }
    }

}
