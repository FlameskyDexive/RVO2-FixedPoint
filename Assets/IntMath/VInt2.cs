using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Two Dimensional Integer Coordinate Pair */
public struct VInt2 : System.IEquatable<VInt2>
{
    public int x;
    public int y;
    public static VInt2 zero { get { return new VInt2(0, 0); } }
    public static VInt2 forward { get { return new VInt2(0, 0x3e8); } }

    public VInt2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int sqrMagnitude
    {
        get
        {
            return ((this.x * this.x) + (this.y * this.y));
        }
    }

    public long sqrMagnitudeLong
    {
        get
        {
            return (long)x * (long)x + (long)y * (long)y;
        }
    }
    public int magnitude
    {
        get
        {
            long x = this.x;
            long y = this.y;
            return IntMath.Sqrt((x * x) + (y * y));
        }
    }

    public static VInt2 operator +(VInt2 a, VInt2 b)
    {
        return new VInt2(a.x + b.x, a.y + b.y);
    }

    public static VInt2 operator -(VInt2 a, VInt2 b)
    {
        return new VInt2(a.x - b.x, a.y - b.y);
    }

    public static bool operator ==(VInt2 a, VInt2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(VInt2 a, VInt2 b)
    {
        return a.x != b.x || a.y != b.y;
    }
    public static VInt2 Lerp(VInt2 a, VInt2 b, VFactor f)
    {
        return new VInt2((int)IntMath.Divide((long)(b.x - a.x) * f.nom, f.den) + a.x, (int)IntMath.Divide((long)(b.y - a.y) * f.nom, f.den) + a.y);
    }

    public static VInt2 Lerp(VInt2 a, VInt2 b, int factorNom, int factorDen)
    {
        return new VInt2(IntMath.Divide((b.x - a.x) * factorNom, factorDen) + a.x, IntMath.Divide((b.y - a.y) * factorNom, factorDen) + a.y);
    }

    public static int Dot(VInt2 a, VInt2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    /** Dot product of the two coordinates */
    public static long DotLong(VInt2 a, VInt2 b)
    {
        return (long)a.x * (long)b.x + (long)a.y * (long)b.y;
    }

    /** Dot product of the two coordinates */
    public static long DotLong(ref VInt2 a, ref VInt2 b)
    {
        return (long)a.x * (long)b.x + (long)a.y * (long)b.y;
    }

    public static long DetLong(ref VInt2 a, ref VInt2 b)
    {
        return ((a.x * b.y) - (a.y * b.x));
    }

    public static long DetLong(VInt2 a, VInt2 b)
    {
        return ((a.x * b.y) - (a.y * b.x));
    }

    public static VInt2 operator *(VInt2 lhs, int rhs)
    {
        lhs.x *= rhs;
        lhs.y *= rhs;
        return lhs;
    }

    public static VInt2 operator /(VInt2 lhs, int rhs)
    {
        lhs.x = lhs.x / rhs;
        lhs.y = lhs.y / rhs;
        return lhs;
    }

    public static implicit operator Vector2(VInt2 ob)
    {
        return new Vector2((float)ob.x * 0.001f, (float)ob.y * 0.001f);
    }

    public static explicit operator VInt3(VInt2 ob)
    {
        return new VInt3(ob.x, 0, ob.y);
    }

    public static implicit operator VInt2(Vector2 ob)
    {
        return new VInt2((int)(ob.x *1000), (int)(ob.y *1000));
    }

    //GG
    /*public static implicit operator VInt2(VFactor a, VFactor b)
    {
        return new VInt2((int)(a.nom * 1000 / a.den), (int)(b.nom * 1000 / b.den));
    }*/

    public override bool Equals(System.Object o)
    {
        if (o == null) return false;
        var rhs = (VInt2)o;

        return x == rhs.x && y == rhs.y;
    }

    public static VInt2 operator -(VInt2 lhs)
    {
        lhs.x = -lhs.x;
        lhs.y = -lhs.y;
        return lhs;
    }

    #region IEquatable implementation

    public bool Equals(VInt2 other)
    {
        return x == other.x && y == other.y;
    }

    #endregion

    public override int GetHashCode()
    {
        return x * 49157 + y * 98317;
    }

    /** Matrices for rotation.
     * Each group of 4 elements is a 2x2 matrix.
     * The XZ position is multiplied by this.
     * So
     * \code
     * //A rotation by 90 degrees clockwise, second matrix in the array
     * (5,2) * ((0, 1), (-1, 0)) = (2,-5)
     * \endcode
     */
    private static readonly int[] Rotations = {
            1, 0,  //Identity matrix
			0, 1,

            0, 1,
            -1, 0,

            -1, 0,
            0, -1,

            0, -1,
            1, 0
        };

    /** Returns a new VInt2 rotated 90*r degrees around the origin.
     * \deprecated Deprecated becuase it is not used by any part of the A* Pathfinding Project
     */
    [System.Obsolete("Deprecated becuase it is not used by any part of the A* Pathfinding Project")]
    public static VInt2 Rotate(VInt2 v, int r)
    {
        r = r % 4;
        return new VInt2(v.x * Rotations[r * 4 + 0] + v.y * Rotations[r * 4 + 1], v.x * Rotations[r * 4 + 2] + v.y * Rotations[r * 4 + 3]);
    }

    /// <summary>
    /// 取得点旋转对应角度后的值
    /// </summary>
    /// <param name="degree"></param>
    /// <returns></returns>
    public VInt2 Rotate(int degree)
    {
        VInt2 num;
        VFactor factor;
        VFactor factor2;
        IntMath.sincos(out factor, out factor2, (long)(0x7ab8 * degree), 0x1b7740L);
        long num2 = factor2.nom * factor.den;
        long num3 = factor2.den * factor.nom;
        long b = factor2.den * factor.den;
        num.x = (int)IntMath.Divide((long)((x * num2) + (y * num3)), b);
        num.y = (int)IntMath.Divide((long)((-x * num3) + (y * num2)), b);
        return num;
    }

    public static VInt2 Min(VInt2 a, VInt2 b)
    {
        return new VInt2(System.Math.Min(a.x, b.x), System.Math.Min(a.y, b.y));
    }

    public static VInt2 Max(VInt2 a, VInt2 b)
    {
        return new VInt2(System.Math.Max(a.x, b.x), System.Math.Max(a.y, b.y));
    }

    public static VInt2 FromInt3XZ(VInt3 o)
    {
        return new VInt2(o.x, o.z);
    }

    public static VInt3 ToInt3XZ(VInt2 o)
    {
        return new VInt3(o.x, 0, o.y);
    }

    public static VInt2 ClampMagnitude(VInt2 v, int maxLength)
    {
        long sqrMagnitudeLong = v.sqrMagnitudeLong;
        long num2 = maxLength;
        if (sqrMagnitudeLong > (num2 * num2))
        {
            long b = IntMath.Sqrt(sqrMagnitudeLong);
            int x = (int)IntMath.Divide((long)(v.x * maxLength), b);
            return new VInt2(x, (int)IntMath.Divide((long)(v.x * maxLength), b));
        }
        return v;
    }

    public Vector2 vec2
    {
        get { return new Vector2(x * 0.001f, y * 0.001f); }
    }

    public void Normalize()
    {
        long num = this.x * 100;
        long num2 = this.y * 100;
        long a = (num * num) + (num2 * num2);
        if (a != 0)
        {
            long b = IntMath.Sqrt(a);
            this.x = (int)IntMath.Divide((long)(num * 0x3e8L), b);
            this.y = (int)IntMath.Divide((long)(num2 * 0x3e8L), b);
        }
    }

    public VInt2 normalized
    {
        get
        {
            VInt2 num = new VInt2(this.x, this.y);
            num.Normalize();
            return num;
        }
    }


    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }

#if UNITY_EDITOR
    public static VInt2 EditorGUIVInt2Field(GUIContent content, VInt2 vint2)
    {
        UnityEditor.EditorGUIUtility.labelWidth = 10;
        content.text += "(VInt2)";
        UnityEditor.EditorGUILayout.LabelField(content);
        GUILayout.BeginHorizontal();
        vint2.x = UnityEditor.EditorGUILayout.IntField("x", vint2.x);
        vint2.y = UnityEditor.EditorGUILayout.IntField("y", vint2.y);
        GUILayout.EndHorizontal();
        UnityEditor.EditorGUIUtility.labelWidth = 0;
        return vint2;
    }
#endif
}
