using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GunAttachmentSystem : MonoBehaviour
{
    [System.Serializable]
    public class AttachmentPoint
    {
        public string pointName;
        public Transform mountPoint;
        public Attachment.AttachmentType allowedType;
        public GameObject currentAttachment;
    }

    public AttachmentPoint[] attachmentPoints;

    public bool Attach(GameObject attachment)
    {
        // Get the Attachment component
        Attachment attachmentComponent = attachment.GetComponent<Attachment>();
        if (attachmentComponent == null)
        {
            Debug.Log("No Attachment script found");
            return false;
        }

        // Iterate through attachment points
        foreach (var point in attachmentPoints)
        {
            // Check for a matching type and an empty slot
            if (point.allowedType == attachmentComponent.attachmentType && point.currentAttachment == null)
            {
                // Disable Rigidbody
                Rigidbody rb = attachment.GetComponent<Rigidbody>();
                BoxCollider collider = attachment.GetComponent<BoxCollider>();
                XRGrabInteractable grabInteractable = attachment.GetComponent<XRGrabInteractable>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                    rb.WakeUp();
                    Debug.Log("Rigidbody Set");
                }

                // Disable Interactable Components
                if (collider != null)
                {
                    collider.enabled = false;
                    Debug.Log("Collider disabled");
                }
                
                if (grabInteractable != null)
                {
                    grabInteractable.interactionLayerMask = LayerMask.GetMask("None"); // Prevent further grabs
                    Debug.Log("GrabInteractable updated");
                }
                Debug.Log("Interactables Set");

                // Snap the attachment
                attachment.transform.SetParent(point.mountPoint);
                Debug.Log($"Parent Set to: {attachment.transform.parent.name}");
                attachment.transform.localPosition = Vector3.zero;
                attachment.transform.localRotation = Quaternion.identity;
                Debug.Log("Transforms Set");

                // Assign the current attachment to the point
                point.currentAttachment = attachment;
                Debug.Log("Attachment Set");

                return true;
            }
        }

        // Return false if no valid point was found
        Debug.LogWarning("No valid attachment point found");
        return false;
    }

    public void Detach(GameObject attachment)
    {
        foreach (var point in attachmentPoints)
        {
            if (point.currentAttachment == attachment)
            {
                attachment.transform.SetParent(null);
                point.currentAttachment = null;
                Debug.Log("Attachment Detached");
                break;
            }
        }
    }
}
