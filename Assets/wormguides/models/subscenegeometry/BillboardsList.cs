using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class BillboardsList {
	private static string BILLBOARDS_CONFIG_FILE_PATH = "billboards_file/BillboardsConfig";


	private static int NUM_CSV_FIElDS = 6;
	private static int BILLBOARD_TEXT_IDX = 0;
	private static int ATTACHMENT_TYPE_IDX = 1;
	private static int ATTACHMENT_CELL = 2;
	private static int XYZ_LOCATION_IDX = 3;
	private static int START_TIME_IDX = 4;
	private static int END_TIME_IDX = 5;

	public static float NOSE_TIP_OFFSET_X = 4;
	public static float NOSE_TIP_OFFSET_Y = 18;
	public static float NOSE_TIP_OFFSET_Z = -28;


	private static int DEFAULT_FONT_SIZE = 50;
	private static float X_OFFSET = 5;
	private static float Y_OFFSET = -15;
	private static float Z_OFFSET = 0;

	private List<Billboard> billboardsList;

	private static string MiscGeoPathStr = "Miscellaneous_Geometry/";

	public BillboardsList() {
		billboardsList = new List<Billboard> ();
		buildListFromConfig ();
	}

	private void buildListFromConfig() {
		TextAsset file = Resources.Load (BILLBOARDS_CONFIG_FILE_PATH) as TextAsset;
		if (file != null) {
			string filestream = file.text;
			string[] fLines = Regex.Split (filestream, "\n|\r|\r\n");

			string line;
			string billboardText;
			string attachmentType;
			string attachmentCell;
			string xyzLocation;
			string startTime;
			string endTime;

			for (int i = 1; i < fLines.Length; i++) {
				line = fLines [i];

				string[] tokens = line.Split (',');

				if (tokens.Length == NUM_CSV_FIElDS) {
					billboardText = tokens [BILLBOARD_TEXT_IDX];
					attachmentType = tokens [ATTACHMENT_TYPE_IDX];
					attachmentCell = tokens [ATTACHMENT_CELL];
					xyzLocation = tokens [XYZ_LOCATION_IDX];
					startTime = tokens [START_TIME_IDX];
					endTime = tokens [END_TIME_IDX];

					Billboard b = new Billboard (
						billboardText,
						attachmentType,
						attachmentCell,
						xyzLocation,
						startTime,
						endTime);

					addBillboard (b);
				}
			}
		}
	}

	public void addBillboard(Billboard b) {
		if (b != null) {
			billboardsList.Add (b);
		}
	}

	public List<Billboard> getBillboardsAtTime(int time, List<string> cellNames) {
		List<Billboard> billboardsAtTime = new List<Billboard> ();

		foreach (Billboard b in billboardsList) {
			if (b.getAttachmentType ().Equals (BillboardAttachmentType.AttachmentType.Static)) {
				if ((int)b.getStartTime () <= time && (int)b.getEndTime () >= time) {
					billboardsAtTime.Add (b);
				}
			} else if (b.getAttachmentType ().Equals (BillboardAttachmentType.AttachmentType.Cell)) {
				if (cellNames.IndexOf (b.getAttachmentCell()) != -1) {
					billboardsAtTime.Add (b);
				}
			}
		}

		return billboardsAtTime;
	}

	public int getDefaultFontSize() {
		return DEFAULT_FONT_SIZE;
	}

	public float getXOffset() {
		return X_OFFSET;
	}

	public float getYOffset() {
		return Y_OFFSET;
	}

	public float getZOffset() {
		return Z_OFFSET;
	}

	public string getMiscGeoPathStr() {
		return MiscGeoPathStr;
	}
}