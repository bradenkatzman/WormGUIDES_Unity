using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard {

	private string billboardText;
	private BillboardAttachmentType.AttachmentType attachmentType;
	private string attachmentCell;
	private float[] xyzLocation;
	private int startTime;
	private int endTime;

	private static string CELL = "Cell";
	private static string STATIC_ = "Static";
	private static int X_IDX = 0;
	private static int Y_IDX = 1;
	private static int Z_IDX = 2;

	public Billboard(
		string bt,
		string attachmentTypeStr,
		string attachmentCell_,
		string xyzLocation_,
		string st,
		string et) {

		if (bt != null) {
			this.billboardText = bt;
		} else {
			this.billboardText = "";
		}

		if (attachmentTypeStr != null) {
			if (attachmentTypeStr.ToLower ().Equals (CELL.ToLower ())) {
				this.attachmentType = BillboardAttachmentType.AttachmentType.Cell;
				this.attachmentCell = attachmentCell_;
			} else if (attachmentTypeStr.ToLower ().Equals (STATIC_.ToLower ())) {
				this.attachmentType = BillboardAttachmentType.AttachmentType.Static;
				this.xyzLocation = new float[3];
				string[] positions = xyzLocation_.Split (' ');
				if (positions.Length == 3) {
					this.xyzLocation [X_IDX] = float.Parse(positions [X_IDX]);
					this.xyzLocation [Y_IDX] = float.Parse(positions [Y_IDX]);
					this.xyzLocation [Z_IDX] = float.Parse(positions [Z_IDX]);
				}
				this.startTime = int.Parse (st);
				this.endTime = int.Parse (et);
			}
		}
	}

	public void setAttachmentCell(string cell) {
		if (cell != null) {
			this.attachmentCell = cell;
		}
	}

	public void setXYZLocation(float x, float y, float z) {
		if (attachmentType.Equals (BillboardAttachmentType.AttachmentType.Static)) {
			if (xyzLocation == null) {
				xyzLocation = new float[3];
			}

			xyzLocation [X_IDX] = x;
			xyzLocation [Y_IDX] = y;
			xyzLocation [Z_IDX] = z;
		}
	}

	public void setStartTime(int st) {
		if (attachmentType.Equals (BillboardAttachmentType.AttachmentType.Static)) {
			this.startTime = st;
		}
	}

	public void setEndTime(int et) {
		if (attachmentType.Equals (BillboardAttachmentType.AttachmentType.Static)) {
			this.endTime = et;
		}
	}

	public string getBillboardText() {
		return this.billboardText;
	}

	public BillboardAttachmentType.AttachmentType getAttachmentType() {
		return this.attachmentType;
	}

	public string getAttachmentCell() {
		if (attachmentType.Equals (BillboardAttachmentType.AttachmentType.Cell)) {
			return this.attachmentCell;
		}

		return "";
	}

	public float[] getXYZLocation() {
		if (attachmentType.Equals (BillboardAttachmentType.AttachmentType.Static)) {
			return this.xyzLocation;
		}

		return new float[]{ -1.0f, -1.0f, -1.0f };
	}

	public int getStartTime() {
		if (attachmentType.Equals(BillboardAttachmentType.AttachmentType.Static)) {
			return this.startTime;
		}

		return -1;
	}

	public int getEndTime() {
		if (attachmentType.Equals (BillboardAttachmentType.AttachmentType.Static)) {
			return this.endTime;
		}

		return -1;
	}
}