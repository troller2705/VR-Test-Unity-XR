using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Attachment : MonoBehaviour
{
    public enum AttachmentType { Scope, Barrel, Grip }
    public AttachmentType attachmentType;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collision detected with {other.name}");
        GunAttachmentSystem gun = other.GetComponentInParent<GunAttachmentSystem>();
        if (gun != null && !gameObject.GetComponent<XRGrabInteractable>().isSelected)
        {
            bool attached = gun.Attach(gameObject);
            if (attached)
            {
                Debug.Log($"{gameObject.name} successfully attached.");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} could not attach.");
            }
        }
    }
}
