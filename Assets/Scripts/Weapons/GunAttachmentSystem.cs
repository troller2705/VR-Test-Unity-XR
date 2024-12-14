using UnityEngine;

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
        if (attachmentComponent == null) return false;

        // Iterate through attachment points
        foreach (var point in attachmentPoints)
        {
            // Check for a matching type and an empty slot
            if (point.allowedType == attachmentComponent.attachmentType && point.currentAttachment == null)
            {
                // Snap the attachment
                attachment.transform.SetParent(point.mountPoint);
                attachment.transform.localPosition = Vector3.zero;
                attachment.transform.localRotation = Quaternion.identity;

                // Assign the current attachment to the point
                point.currentAttachment = attachment;

                // Disable Rigidbody for proper snapping
                Rigidbody rb = attachment.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                return true;
            }
        }

        // Return false if no valid point was found
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
                break;
            }
        }
    }
}
