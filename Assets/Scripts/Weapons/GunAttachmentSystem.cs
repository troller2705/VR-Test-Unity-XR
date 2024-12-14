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
        Attachment attachmentComponent = attachment.GetComponent<Attachment>();
        if (attachmentComponent == null) return false;

        foreach (var point in attachmentPoints)
        {
            if (point.allowedType == attachmentComponent.attachmentType && point.currentAttachment == null)
            {
                // Snap the attachment
                attachment.transform.SetParent(point.mountPoint);
                attachment.transform.localPosition = Vector3.zero;
                attachment.transform.localRotation = Quaternion.identity;

                point.currentAttachment = attachment;
                return true;
            }
        }

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
