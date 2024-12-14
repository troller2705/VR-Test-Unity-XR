using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class Attachment : MonoBehaviour
{
    public enum AttachmentType { Scope, Barrel, Grip }
    public AttachmentType attachmentType;
    public Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        GunAttachmentSystem gun = other.GetComponentInParent<GunAttachmentSystem>();
        if (gun != null)
        {
            Debug.Log($"Attachment {gameObject.name} entered snapping zone.");
            gun.Attach(gameObject);
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

}
