using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SulstonLineage {
    private static string P0 = "P0";
    private static string AB = "AB";
    private static string P1 = "P1";
    private static string P2 = "P2";
    private static string EMS = "EMS";
    private static string E = "E";
    private static string MS = "MS";
    private static string C = "C";
    private static string P3 = "P3";
    private static string D = "D";
    private static string P4 = "P4";
    private static string Z3 = "Z3";
    private static string Z2 = "Z2";
    private static List<string> specialCasesAsStringArray = new List<string>{P0, AB, P1, EMS, E, MS, C, P3, D, P4, Z3, Z2};

    // instead of building a traversable tree data structure to query the ancestors and descendants of these special cases
    // these exhaustive lists have the ancestry information for all of the special cases (no native tree structure in C#)
    private static List<string> P0_ancestors = new List<string> { };
    private static List<string> P0_descendants = new List<string> { AB, P1, EMS, E, MS, P2, C, P3, D, P4, Z2, Z3};

    private static List<string> AB_ancestors = new List<string> { P0};
    private static List<string> AB_descendants = new List<string> { };

    private static List<string> P1_ancestors = AB_ancestors;
    private static List<string> P1_descendants = new List<string> { EMS, E, MS, P2, C, P3, D, P4, Z3, Z2};

    private static List<string> P2_ancestors = new List<string> { P1, P0};
    private static List<string> P2_descendants = new List<string> { C, P3, D, P4};

    private static List<string> EMS_ancestors = P2_ancestors;
    private static List<string> EMS_descendants = new List<string> { E, MS};

    private static List<string> E_ancestors = new List<string> { EMS, P1, P0};
    private static List<string> E_descendants = new List<string> { };

    private static List<string> MS_ancestors = E_ancestors;
    private static List<string> MS_descendants = new List<string> { };

    private static List<string> C_ancestors = new List<string> { P2, P1, P0};
    private static List<string> C_descendants = new List<string> { };

    private static List<string> P3_ancestors = C_ancestors;
    private static List<string> P3_descendants = new List<string> { D, P4, Z3, Z2};

    private static List<string> D_ancestors = new List<string> { P3, P2, P1, P0};
    private static List<string> D_descendants = new List<string> { };

    private static List<string> P4_ancestors = D_ancestors;
    private static List<string> P4_descendants = new List<string> { Z3, Z2};

    private static List<string> Z3_ancestors = new List<string> { P4, P3, P2, P1, P0};
    private static List<string> Z3_descendants = new List<string> { };

    private static List<string> Z2_ancestors = Z3_ancestors;
    private static List<string> Z2_descendants = new List<string> { };

    public static List<string> getSpecialCases()
    {
        return specialCasesAsStringArray;
    }

    public static List<string> getAncestorsOfSpecialCase(string specialCase)
    {
        if (specialCase == null)
        {
            return null;
        }

        if (specialCase.ToUpper().Equals(P0))
        {
            return P0_ancestors;
        } else if (specialCase.ToUpper().Equals(AB))
        {
            return AB_ancestors;
        } else if (specialCase.ToUpper().Equals(P1))
        {
            return P1_ancestors;
        } else if(specialCase.ToUpper().Equals(P2)) {
            return P2_ancestors;
        } else if(specialCase.ToUpper().Equals(EMS))
        {
            return EMS_ancestors;
        } else if (specialCase.ToUpper().Equals(E))
        {
            return E_ancestors;
        } else if (specialCase.ToUpper().Equals(MS))
        {
            return MS_ancestors;
        } else if (specialCase.ToUpper().Equals(C))
        {
            return C_ancestors;
        } else if (specialCase.ToUpper().Equals(P3))
        {
            return P3_ancestors;
        } else if(specialCase.ToUpper().Equals(D))
        {
            return D_ancestors;
        } else if(specialCase.ToUpper().Equals(P4))
        {
            return P4_ancestors;
        } else if(specialCase.ToUpper().Equals(Z3))
        {
            return Z3_ancestors;
        } else if(specialCase.ToUpper().Equals(Z2))
        {
            return Z2_ancestors;
        }

        return null;
    }

    public static List<string> getDescendantsOfSpecialCase(string specialCase)
    {
        if (specialCase == null)
        {
            return null;
        }

        if (specialCase.ToUpper().Equals(P0))
        {
            return P0_descendants;
        }
        else if (specialCase.ToUpper().Equals(AB))
        {
            return AB_descendants;
        }
        else if (specialCase.ToUpper().Equals(P1))
        {
            return P1_descendants;
        }
        else if (specialCase.ToUpper().Equals(P2)) {
            return P2_descendants;
        }
        else if (specialCase.ToUpper().Equals(EMS))
        {
            return EMS_descendants;
        }
        else if (specialCase.ToUpper().Equals(E))
        {
            return E_descendants;
        }
        else if (specialCase.ToUpper().Equals(MS))
        {
            return MS_descendants;
        }
        else if (specialCase.ToUpper().Equals(C))
        {
            return C_descendants;
        }
        else if (specialCase.ToUpper().Equals(P3))
        {
            return P3_descendants;
        }
        else if (specialCase.ToUpper().Equals(D))
        {
            return D_descendants;
        }
        else if (specialCase.ToUpper().Equals(P4))
        {
            return P4_descendants;
        }
        else if (specialCase.ToUpper().Equals(Z3))
        {
            return Z3_descendants;
        }
        else if (specialCase.ToUpper().Equals(Z2))
        {
            return Z2_descendants;
        }

        return null;
    }
}
