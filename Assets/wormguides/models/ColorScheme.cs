using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorScheme {
	private int firstIDX = 0;
	private int lastIDX = 3;

	public enum CS {TourTract_NerveRing, LineageSpatialRelationships, NeuronalCellPositions, TissueTypes};

	private CS cs;

	public ColorScheme(CS cs_) {
		this.cs = cs_;
	}

	public CS getColorScheme() {
		return this.cs;
	}

	public void setColorScheme(CS cs_) {
		this.cs = cs_;
	}

	public void setColorScheme(int cs_IDX) {
		if (cs_IDX >= firstIDX && cs_IDX <= lastIDX) {
			this.cs = (CS)cs_IDX;
		}
	}
}