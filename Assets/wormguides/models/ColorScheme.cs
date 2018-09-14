using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorScheme {
	private int firstIDX = 0;
	private int lastIDX = 3;

    private List<List<string>> currentColorScheme;

    private string[] colorURLS = new string[] {"http://scene.wormguides.org/wormguides/testurlscript?/set/saavl-n$@+#ffff0000/sibd-n$@+#ffe6ccff/avg-n$@+#ffffff00/rip-n$@+#ff99cc99/rih-n$@+#ff99cc99/smdd-n$@+#ff4d66cc/aiy-n$@+#ffff9900/rig-n$@+#fff90557/pvt-n$@+#ffffffff/dva-n$@+#ffffb366/ala-n$@+#ff01c501/aim-n$@+#ffffff66/aial-n$@+#ff8066cc/afd-n$@+#ff4de6e2/rim-n$@+#ffb399ff/avd-n$@+#ffff9980/CEH-37=Amphid=Multicellular=Structure-H+#ff994d66/rmdd-n$@+#ff664db3/rmg-n$@+#ffffffff/ada-n$@+#ffffffff/aiz-n$@+#ffffffff/bdu-n$@+#ffffffff/smbd-n$@+#ffffffff/pha-n$@+#ffffffff/hsn-n$@+#ffffffff/phb-n$@+#ffffffff/Embryo=Outline-M+#4b00f2ff/Hypoderm-M+#043381aa/view/time=360/rX=0.0/rY=0.0/rZ=0.0/tX=-14.0/tY=18.0/scale=2.75/dim=0.3/Android/",
    "http://scene.wormguides.org/wormguides/testurlscript?/set/E-s<$+#ff13ff03/MS-s<$+#ff05ffd6/D-s<$+#fffd00ff/C-s<$+#ffffb3b3/P4-s<$+#ffffff4d/ABal-s<$+#ffff6666/ABar-s<$+#ffcc3333/ABpl-s<$+#ff4b6efc/ABpr-s<$+#ff0034ff/view/time=294/rX=0.0/rY=0.0/rZ=0.0/tX=-14.0/tY=18.0/scale=2.75/dim=0.3/Android/"};

    private string[] colorSchemeNames = new string[] {"Tour of the Tract and Simplified Single Cell Shape Model", 
     "Lineage/Spatial Relationships"};

    public enum CS {TourTract_NerveRing, LineageSpatialRelationships, NeuronalCellPositions, TissueTypes};

	private CS cs;

	public ColorScheme(int idx) {
        currentColorScheme = UrlParser.parseUrlRules(colorURLS[idx]);
	}

	public CS getColorScheme() {
		return this.cs;
	}

	public void setColorScheme(int idx) {
        currentColorScheme = UrlParser.parseUrlRules(colorURLS[idx]);
	}

    public List<List<string>> getCurrentRulesList()
    {
        return this.currentColorScheme;
    }
}