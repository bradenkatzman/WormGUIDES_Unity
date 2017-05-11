using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardAttachmentType {

	public enum AttachmentType {Cell, Static};

	private AttachmentType attachmentType;

	public BillboardAttachmentType(AttachmentType at) {
		this.attachmentType = at;
	}

	public AttachmentType getAttachmentType() {
		return this.attachmentType;
	}

	public void setAttachmentType(AttachmentType at) {
		this.attachmentType = at;
	}
}
