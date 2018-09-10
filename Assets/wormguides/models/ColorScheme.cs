using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorScheme {
	private int firstIDX = 0;
	private int lastIDX = 3;

    private List<List<string>> currentColorScheme;

    private string[] colorURLS = new string[] {"http://scene.wormguides.org/wormguides/testurlscript?/set/saavl-n$@+#ffff0000/sibd-n$@+#ffe6ccff/avg-n$@+#ffffff00/rip-n$@+#ff99cc99/rih-n$@+#ff99cc99/smdd-n$@+#ff4d66cc/aiy-n$@+#ffff9900/rig-n$@+#fff90557/pvt-n$@+#ffffffff/dva-n$@+#ffffb366/ala-n$@+#ff01c501/aim-n$@+#ffffff66/aial-n$@+#ff8066cc/afd-n$@+#ff4de6e2/rim-n$@+#ffb399ff/avd-n$@+#ffff9980/CEH-37=Amphid=Multicellular=Structure-H+#ff994d66/rmdd-n$@+#ff664db3/rmg-n$@+#ffffffff/ada-n$@+#ffffffff/aiz-n$@+#ffffffff/bdu-n$@+#ffffffff/smbd-n$@+#ffffffff/pha-n$@+#ffffffff/hsn-n$@+#ffffffff/phb-n$@+#ffffffff/Embryo=Outline-M+#4a00f2ff/Hypoderm-M+#033381aa/view/time=360/rX=28.0/rY=-4.0/rZ=0.0/tX=-14.0/tY=18.0/scale=2.75/dim=0.3/browser/",
    "http://scene.wormguides.org/wormguides/testurlscript?/set/saavl-n$@>+#ffff0000/sibd-n$@>+#ffe6ccff/avg-n$@>+#ffffff00/rip-n$@>+#ff99cc99/rih-n$@>+#ff99cc99/smdd-n$@>+#ff4d66cc/aiy-n$@>+#ff98ff53/rig-n$@>+#fff90557/pvt-n$@>+#ffffffff/dva-n$@>+#ff8343f9/ala-n$@>+#ff01c501/aim-n$@>+#ffffff66/aial-n$@>+#ff8066cc/afd-n$@>+#ff4de6e2/rim-n$@>+#ffb399ff/avd-n$@>+#ffff9980/Embryo=Outline-M+#4500f2ff/CEH-37=Amphid=Multicellular=Structure-H+#ff994d66/rmdd-n$@>+#ff664db3/LIM-4=Multicellular=Structure-H+#ffffb156/rmg-n$@+#ffffffff/ada-n$@+#ffffffff/aiz-n$@+#ffffffff/bdu-n$@+#ffffffff/smbd-n$@+#ffffffff/pha-n$@+#ffffffff/hsn-n$@+#ffffffff/phb-n$@+#ffffffff/Pharynx-M+#80fffefe/P0-s<$+#00ffffff/Hypoderm-M+#00ffffff/view/time=360/rX=13.0/rY=3.0/rZ=0.0/tX=-1.5/tY=12.5/scale=2.75/dim=0.3/browser/",
    "http://scene.wormguides.org/wormguides/testurlscript?/set/sheath-d$+#ffffffff/socket-d$+#ffe64d4d/sensory-d$+#ffffff66/interneuron-d$+#ff334db3/motor-d$+#ff33cc46/view/time=327/rX=-7.375/rY=-5.125/rZ=0.0/tX=0.0/tY=0.0/scale=2.25/dim=0.36/Android/",
    "http://scene.wormguides.org/wormguides/testurlscript?/set/E-s<$+#ff13ff03/MS-s<$+#ff05ffd6/D-s<$+#fffd00ff/C-s<$+#ffffb3b3/P4-s<$+#ffffff4d/ABal-s<$+#ffff6666/ABar-s<$+#ffcc3333/ABpl-s<$+#ff4b6efc/ABpr-s<$+#ff0034ff/view/time=360/rX=-9.0/rY=-24.0/rZ=0.0/tX=-14.0/tY=18.0/scale=2.75/dim=0.25/Android/",
    "http://scene.wormguides.org/wormguides/testurlscript?/set/muscle-d$+#ff33cc00/intestinal-d$+#ffffccff/marginal-d$%3E+#ffcc33cc/vulva-d$+#ff9999ff/interneuron-d$+#ffff3300/motor-d$+#ff9966cc/sensory-d$+#ffff66cc/pore-d$+#ffecf78f/duct-d$+#ffcc9966/gland-d$+#ff9999cc/excretory-d$+#ffcccc99/hypoderm-d$+#ffdcc3ac/seam-d$+#ffcc6633/socket-d$+#ffff9999/sheath-d$+#ff339999/valve-d$+#ff996633/arcade-d$+#ffcccc00/epithelium-d$+#ff996699/rectal-d$+#ff996666/gland-d$+#ffccccff/head-d$+#ffff6633/view/time=327/rX=-7.375/rY=-5.125/rZ=0.0/tX=0.0/tY=0.0/scale=2.25/dim=0.36/Android/"};

    private string[] colorSchemeNames = new string[] {"Tour of the Tract and Simplified Single Cell Shape Model", "Pharynx Development and the Nerve Ring",
    "Neuronal Cell Positions", "Lineage/Spatial Relationships", "Tissue Types in Worm Atlas Color Scheme"};

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