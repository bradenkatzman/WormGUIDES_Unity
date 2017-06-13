using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulesLists {
	/*
	 * Cells or keywords with associated color rules
	 */ 
	// 1. Tract Tour, Nerve Ring
	private string[] TractTour_NerveRing_rule_cells = new string[]{
		"ABplpaaaaaa", "ABplppaaaaa", "ABprppaaaaa", "ABprpappaap", "ABprpapppap", "ABalpapaaaa", "ABarappaaaa", "ABprpappaaa",
		"ABplpapappa", "ABprpapappa", "ABplpapaaaa", "ABprpapaaaa",  "ABplpapaapa", "ABprpapaapa",  "ABplpapaapp", "ABprpapaapp",
		"ABprpaaaaaa", "ABalppappaa", "ABarappppaa", "ABplpaapaaa", "ABprpaapaaa", "unc-86_outgrowth"};

	// 2. Lineage and Spatial Relationships (color only applies to primitive cell shapes
	private string[] LineageSpatialRelationships_rule_cells = new string[] {
		"E", "MS", "D", "C", "P4", "ABal", "ABar", "ABpl", "ABpr", "Z2", "Z3"};

	// 3. Neuronal Cell Positions
	private string[] NeuronalCellPositions_keywords = new string[] {
		"sheath", "socket", "sensory", "interneuron", "motor"};
	// 4. Tissue Types
	private string[] TissueTypes_keywords = new string[] {
		"muscle", "intestinal", "marginal", "vulva", "interneuron", "motor", "sensory", "pore", "duct",
		"gland", "excretory", "hypoderm", "seam", "socket", "sheath", "valve", "arcade", "epithelium", 
		"rectal", "head"};
	// ** end cells/keywords

	/*
	 * Color Schemes
	 */ 
	// 1. Tract Tour, Nerve Ring
	private Material[] TractTour_NerveRing_rule_materials;


	// 2. Lineage and Spatial Relationships
	private Material[] LineageSpatialRelationships_rule_materials;

	// 3. Neuronal Cell Positions
	private Material[] NeuronalCellPositions_rule_materials;

	// 4. Tissue Types
	private Material[] TissueTypes_rule_materials;

	//defaults
	private Material[] DefaultMaterials;
	private int DEFAULT_MATERIAL_IDX = 0;
	// ** end color schemes


	public RulesLists(Material[] ttrn_materials,
		Material[] lsr_materials,
		Material[] ncp_materials,
		Material[] tt_materials,
		Material[] def_materials) {

		this.TractTour_NerveRing_rule_materials = ttrn_materials;
		this.LineageSpatialRelationships_rule_materials = lsr_materials;
		this.NeuronalCellPositions_rule_materials = ncp_materials;
		this.TissueTypes_rule_materials = tt_materials;
		this.DefaultMaterials = def_materials;
	}

	public List<List<Material>> getAllRuleMaterialsForTimePoint(List<string> cellNames) {
		List<Material> TractTour_NerveRing_materials = new List<Material> ();
		List<Material> LineageSpatialRelationships_materials = new List<Material> ();
		List<Material> NeuronalCellPositions_materials = new List<Material> ();
		List<Material> TissueTypes_materials = new List<Material> ();

		for (int i = 0; i < cellNames.Count; i++) {
			bool hasColor;

			/****** TRACT TOUR ******/
			// add color
			hasColor = false;
			for (int k = 0; k < TractTour_NerveRing_rule_cells.Length; k++) {
				if (cellNames[i].ToLower ().Equals (TractTour_NerveRing_rule_cells [k].ToLower ())) {
					// add the color
					hasColor = true;
					TractTour_NerveRing_materials.Add(TractTour_NerveRing_rule_materials [k]);
				}
			}
			if (!hasColor) {
				TractTour_NerveRing_materials.Add(DefaultMaterials [DEFAULT_MATERIAL_IDX]);
			}
			/****** END TRACT TOUR ******/


//			/****** LINEAGE/SPATIAL RELATIONSHIPS ******/
			hasColor = false;
			for (int k = 0; k < LineageSpatialRelationships_rule_cells.Length; k++) {
				if (cellNames[i].ToLower ().StartsWith (LineageSpatialRelationships_rule_cells [k].ToLower ()) && !cellNames[i].Equals ("EMS")) {
					hasColor = true;
					LineageSpatialRelationships_materials.Add(LineageSpatialRelationships_rule_materials [k]);
				}
			}
			if (!hasColor) {
				LineageSpatialRelationships_materials.Add(DefaultMaterials [DEFAULT_MATERIAL_IDX]);
			}
//			/****** END LINEAGE/SPATIAL RELATIONSHIPS ******/

			/****** NEURONAL CELL POSITIONS ******/
//			hasColor = false;
//			for (int k = 0; !hasColor && k < NeuronalCellPositions_keywords.Length; k++) {
//				int descrMatchResults = PartsList.findDescriptionMatch (cellNames[i], NeuronalCellPositions_keywords [k]);
//
//				if (descrMatchResults == 0) { // pure clone, give full color
//					hasColor = true;
//					NeuronalCellPositions_materials.Add(NeuronalCellPositions_rule_materials [k]);
//				} else if (descrMatchResults > 0) { // parent of pure clone, give weighted avg color
//					
//					string weightedHex = WeightedAvgHexcode.computeWeightedAverageHexcode (
//						                     new string[] {ColorUtility.ToHtmlStringRGBA (NeuronalCellPositions_rule_materials [k].color),
//												ColorUtility.ToHtmlStringRGBA (DefaultMaterials [DEFAULT_MATERIAL_IDX].color)},
//						            		new float[] {(1.0f / (float)(descrMatchResults + 1)),
//												(((float)(descrMatchResults)) / ((float)(descrMatchResults + 1)))
//						});
//					Material weightedMaterial = DefaultMaterials [DEFAULT_MATERIAL_IDX];
//					Color weightedColor = new Color ();
//					bool success = ColorUtility.TryParseHtmlString (weightedHex, out weightedColor);
//					if (success) {
//						hasColor = true;
//						weightedMaterial.color = weightedColor;
//						NeuronalCellPositions_materials.Add (weightedMaterial);
//					} else {
//						Debug.Log ("failed with " + weightedHex);
//					}
//				}
//			}
//			if (!hasColor) {
//				NeuronalCellPositions_materials.Add (DefaultMaterials [DEFAULT_MATERIAL_IDX]);
//			} else {
//				Debug.Log ("has neuronal cell position color");
//			}
//			/****** END NEURONAL CELL POSITIONS ******/
////
////
////
////			/****** TISSUE TYPES ******/
//			hasColor = false;
//			for (int k = 0; !hasColor && k < TissueTypes_keywords.Length; k++) {
//				int descrMatchResults = PartsList.findDescriptionMatch (cellNames[i], TissueTypes_keywords [k]);
//				if (descrMatchResults == 0) { // pure clone, give full color
//					hasColor = true;
//					TissueTypes_materials.Add(TissueTypes_rule_materials [k]);
//				} else if (descrMatchResults > 0) { // parent of pure clone, give weighted avg color
//					string weightedHex = WeightedAvgHexcode.computeWeightedAverageHexcode (
//						                     new string[] {ColorUtility.ToHtmlStringRGBA (TissueTypes_rule_materials [k].color),
//							ColorUtility.ToHtmlStringRGBA (DefaultMaterials [DEFAULT_MATERIAL_IDX].color)
//						},
//						                     new float[] {(1.0f / (float)(descrMatchResults + 1)),
//							(((float)(descrMatchResults)) / ((float)(descrMatchResults + 1)))
//						});
//					Material weightedMaterial = DefaultMaterials [DEFAULT_MATERIAL_IDX];
//					Color weightedColor = new Color ();
//					bool success = ColorUtility.TryParseHtmlString (weightedHex, out weightedColor);
//					if (success) {
//						hasColor = true;
//						weightedMaterial.color = weightedColor;
//						TissueTypes_materials.Add (weightedMaterial);
//					} else {
//						Debug.Log ("failed with " + weightedHex);
//					}
//				}
//			}
//			if (!hasColor) {
//				TissueTypes_materials.Add(DefaultMaterials [DEFAULT_MATERIAL_IDX]);
//			}
//			/****** END TISSUE TYPES ******/
		}

		List<List<Material>> allMaterialsLists = new List<List<Material>> ();
		allMaterialsLists.Add (TractTour_NerveRing_materials);
		allMaterialsLists.Add (LineageSpatialRelationships_materials);
		allMaterialsLists.Add (NeuronalCellPositions_materials);
		allMaterialsLists.Add (TissueTypes_materials);

		return allMaterialsLists;
	}
}