using System;
using System.Runtime.InteropServices;
using UnityEngine;

//namespace Pathfinding {
/** Holds a coordinate in integers */
[Serializable, StructLayout(LayoutKind.Sequential)]
public struct VInt3 : System.IEquatable<VInt3>
{
    public int x;
    public int y;
    public int z;

    //These should be set to the same value (only PrecisionFactor should be 1 divided by Precision)

    /** Precision for the integer coordinates.
     * One world unit is divided into [value] pieces. A value of 1000 would mean millimeter precision, a value of 1 would mean meter precision (assuming 1 world unit = 1 meter).
     * This value affects the maximum coordinates for nodes as well as how large the cost values are for moving between two nodes.
     * A higher value means that you also have to set all penalty values to a higher value to compensate since the normal cost of moving will be higher.
     */
    public const int Precision = 1000;

    /** #Precision as a float */
    public const float FloatPrecision = 1000F;

    /** 1 divided by #Precision */
    public const float PrecisionFactor = 0.001F;

    public static VInt3 zero { get { return new VInt3(); } }
    public static readonly VInt3 one = new VInt3(1000, 1000, 1000);

    public static readonly VInt3 half = new VInt3(500, 500, 500);

    public static readonly VInt3 forward = new VInt3(0, 0, 1000);
    public static readonly VInt3 back = new VInt3(0, 0, -1000);

    public static readonly VInt3 up = new VInt3(0, 1000, 0);

    public static readonly VInt3 right = new VInt3(1000, 0, 0);

    public VInt3(Vector3 position)
    {
        x = (int)System.Math.Round(position.x * FloatPrecision);
        y = (int)System.Math.Round(position.y * FloatPrecision);
        z = (int)System.Math.Round(position.z * FloatPrecision);
    }

    public VInt3(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public Vector3 vec3
    {
        get
        {
            return new Vector3((float)this.x * 0.001f, (float)this.y * 0.001f, (float)this.z * 0.001f);
        }
    }

    public VInt2 xz
    {
        get
        {
            return new VInt2(this.x, this.z);
        }
    }

    public int magnitude2D
    {
        get
        {
            long num = (long)this.x;
            long num2 = (long)this.z;
            return IntMath.Sqrt(num * num + num2 * num2);
        }
    }

    public static bool operator ==(VInt3 lhs, VInt3 rhs)
    {
        return lhs.x == rhs.x &&
               lhs.y == rhs.y &&
               lhs.z == rhs.z;
    }

    public static bool operator !=(VInt3 lhs, VInt3 rhs)
    {
        return lhs.x != rhs.x ||
               lhs.y != rhs.y ||
               lhs.z != rhs.z;
    }

    public static implicit operator VInt3(Vector3 ob)
    {
        VInt3 vInt3 = new VInt3(
            (int)System.Math.Round(ob.x * FloatPrecision),
            (int)System.Math.Round(ob.y * FloatPrecision),
            (int)System.Math.Round(ob.z * FloatPrecision)
        );
        //Debug.Log(vInt3);
        return vInt3;

        /*return new VInt3(
            (int)System.Math.Round(ob.x*FloatPrecision),
            (int)System.Math.Round(ob.y*FloatPrecision),
            (int)System.Math.Round(ob.z*FloatPrecision)
            );*/
    }

    public static implicit operator Vector3(VInt3 ob)
    {
        return new Vector3(ob.x * PrecisionFactor, ob.y * PrecisionFactor, ob.z * PrecisionFactor);
    }

    public static VInt3 operator -(VInt3 lhs, VInt3 rhs)
    {
        lhs.x -= rhs.x;
        lhs.y -= rhs.y;
        lhs.z -= rhs.z;
        return lhs;
    }

    public static VInt3 operator -(VInt3 lhs)
    {
        lhs.x = -lhs.x;
        lhs.y = -lhs.y;
        lhs.z = -lhs.z;
        return lhs;
    }

    public static VInt3 operator +(VInt3 lhs, VInt3 rhs)
    {
        lhs.x += rhs.x;
        lhs.y += rhs.y;
        lhs.z += rhs.z;
        return lhs;
    }

    public static VInt3 operator *(VInt3 lhs, int rhs)
    {
        lhs.x *= rhs;
        lhs.y *= rhs;
        lhs.z *= rhs;

        return lhs;
    }

    public static VInt3 operator *(VInt3 lhs, float rhs)
    {
        lhs.x = (int)System.Math.Round(lhs.x * rhs);
        lhs.y = (int)System.Math.Round(lhs.y * rhs);
        lhs.z = (int)System.Math.Round(lhs.z * rhs);

        return lhs;
    }

    public static VInt3 operator *(VInt3 lhs, double rhs)
    {
        lhs.x = (int)System.Math.Round(lhs.x * rhs);
        lhs.y = (int)System.Math.Round(lhs.y * rhs);
        lhs.z = (int)System.Math.Round(lhs.z * rhs);

        return lhs;
    }

    public static VInt3 operator *(VInt3 lhs, VInt3 rhs)
    {
        lhs.x *= rhs.x;
        lhs.y *= rhs.y;
        lhs.z *= rhs.z;
        return lhs;
    }

    public static VInt3 operator /(VInt3 lhs, float rhs)
    {
        lhs.x = (int)System.Math.Round(lhs.x / rhs);
        lhs.y = (int)System.Math.Round(lhs.y / rhs);
        lhs.z = (int)System.Math.Round(lhs.z / rhs);
        return lhs;
    }

    public int this[int i]
    {
        get
        {
            return i == 0 ? x : (i == 1 ? y : z);
        }
        set
        {
            if (i == 0) x = value;
            else if (i == 1) y = value;
            else z = value;
        }
    }

    /** Angle between the vectors in radians */
    public static float Angle(VInt3 lhs, VInt3 rhs)
    {
        double cos = Dot(lhs, rhs) / ((double)lhs.magnitude * (double)rhs.magnitude);

        cos = cos < -1 ? -1 : (cos > 1 ? 1 : cos);
        return (float)System.Math.Acos(cos);
    }

    public static int Dot(VInt3 lhs, VInt3 rhs)
    {
        return
            lhs.x * rhs.x +
            lhs.y * rhs.y +
            lhs.z * rhs.z;
    }

    public static long DotLong(VInt3 lhs, VInt3 rhs)
    {
        return
            (long)lhs.x * (long)rhs.x +
            (long)lhs.y * (long)rhs.y +
            (long)lhs.z * (long)rhs.z;
    }

    public static long DotLongSafe(VInt3 lhs, VInt3 rhs)
    {
        return
            ((long)lhs.x * (long)rhs.x +
            (long)lhs.y * (long)rhs.y +
            (long)lhs.z * (long)rhs.z) / 10000;
    }


    public VInt3 NormalizeV3()
    {
        VInt3 retVal = this;
        long num = retVal.x * 100;
        long num2 = retVal.y * 100;
        long num3 = retVal.z * 100;
        long a = ((num * num) + (num2 * num2)) + (num3 * num3);
        if (a != 0)
        {
            long b = IntMath.Sqrt(a);
            //if (b)
            //{
            //}
            long num6 = Precision;
            if (Math.Abs(num) == b)
            {
                retVal.x = (int)(num * num6 / b);
            }
            else
            {
                retVal.x = (int)IntMath.Divide((long)(num * num6), b);
            }
            if (Math.Abs(num2) == b)
            {
                retVal.y = (int)(num2 * num6 / b);
            }
            else
            {
                retVal.y = (int)IntMath.Divide((long)(num2 * num6), b);
            }
            if (Math.Abs(num3) == b)
            {
                retVal.z = (int)(num3 * num6 / b);
            }
            else
            {
                retVal.z = (int)IntMath.Divide((long)(num3 * num6), b);
            }
            //retVal.y = (int)IntMath.Divide((long)(num2 * num6), b);
            //retVal.z = (int)IntMath.Divide((long)(num3 * num6), b);
            //retVal *= Precision;
        }
        return retVal;
    }

    public VInt3 normalized
    {
        get
        {
            return NormalizeV3();
            //return ((Vector3)this).normalized;
        }
    }

    /** Normal in 2D space (XZ).
     * Equivalent to Cross(this, VInt3(0,1,0) )
     * except that the Y coordinate is left unchanged with this operation.
     */
    public VInt3 Normal2D()
    {
        return new VInt3(z, y, -x);
    }

    /** Returns the magnitude of the vector. The magnitude is the 'length' of the vector from 0,0,0 to this point. Can be used for distance calculations:
     * \code Debug.Log ("Distance between 3,4,5 and 6,7,8 is: "+(new VInt3(3,4,5) - new VInt3(6,7,8)).magnitude); \endcode
     */
    /*public float magnitude {
        get {
            //It turns out that using doubles is just as fast as using ints with Mathf.Sqrt. And this can also handle larger numbers (possibly with small errors when using huge numbers)!

            double _x = x;
            double _y = y;
            double _z = z;

            return (float)System.Math.Sqrt(_x*_x+_y*_y+_z*_z);
        }
    }*/

    //Good Game
    public int magnitude
    {
        get
        {
            long num = (long)this.x;
            long num2 = (long)this.y;
            long num3 = (long)this.z;
            return IntMath.Sqrt(num * num + num2 * num2 + num3 * num3);
        }
    }

    /** Magnitude used for the cost between two nodes. The default cost between two nodes can be calculated like this:
     * \code int cost = (node1.position-node2.position).costMagnitude; \endcode
     *
     * This is simply the magnitude, rounded to the nearest integer
     */
    public int costMagnitude
    {
        get
        {
            //return (int)System.Math.Round(magnitude);
            //Good Game
            return magnitude;
        }
    }

    /** The magnitude in world units.
     * \deprecated This property is deprecated. Use magnitude or cast to a Vector3
     */
    [System.Obsolete("This property is deprecated. Use magnitude or cast to a Vector3")]
    public float worldMagnitude
    {
        get
        {
            double _x = x;
            double _y = y;
            double _z = z;

            return (float)System.Math.Sqrt(_x * _x + _y * _y + _z * _z) * PrecisionFactor;
        }
    }

    /** The squared magnitude of the vector */
    // safe mode, * 0.001
    public float sqrMagnitude
    {
        get
        {
            /*double _x = x;
            double _y = y;
            double _z = z;
            return (float)(_x*_x+_y*_y+_z*_z);*/
            float num = (float)this.x * 0.001f;
            float num2 = (float)this.y * 0.001f;
            float num3 = (float)this.z * 0.001f;
            return num * num + num2 * num2 + num3 * num3;
        }
    }

    /** The squared magnitude of the vector */
    public long sqrMagnitudeLong
    {
        get
        {
            long _x = x;
            long _y = y;
            long _z = z;
            return (_x * _x + _y * _y + _z * _z);
        }
    }

    /// <summary>
    ///   <para>Returns the distance between a and b.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static long Distance(VInt3 a, VInt3 b)
    {
        VInt3 vector3 = new VInt3(a.x - b.x, a.y - b.y, a.z - b.z);
        return IntMath.Sqrt((vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z));
    }

    #region Good Game Functions
    // Good Game
    public long sqrMagnitudeLong2D
    {
        get
        {
            long num = (long)this.x;
            long num2 = (long)this.z;
            return num * num + num2 * num2;
        }
    }

    // Good Game
    public VInt3 abs
    {
        get
        {
            return new VInt3(Math.Abs(this.x), Math.Abs(this.y), Math.Abs(this.z));
        }
    }

    // Good Game
    public VInt3 DivBy2()
    {
        this.x >>= 1;
        this.y >>= 1;
        this.z >>= 1;
        return this;
    }

    // Good Game
    public static VFactor AngleInt(VInt3 lhs, VInt3 rhs)
    {
        long den = (long)lhs.magnitude * (long)rhs.magnitude;
        return IntMath.acos((long)VInt3.Dot(ref lhs, ref rhs), den);
    }

    // Good Game
    public static int Dot(ref VInt3 lhs, ref VInt3 rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
    }

    // Good Game
    public static long DotXZLong(ref VInt3 lhs, ref VInt3 rhs)
    {
        return (long)lhs.x * (long)rhs.x + (long)lhs.z * (long)rhs.z;
    }

    // Good Game
    public static long DotXZLong(VInt3 lhs, VInt3 rhs)
    {
        return (long)lhs.x * (long)rhs.x + (long)lhs.z * (long)rhs.z;
    }

    public static VInt3 Cross(ref VInt3 lhs, ref VInt3 rhs)
    {
        return new VInt3(IntMath.Divide(lhs.y * rhs.z - lhs.z * rhs.y, 1000), IntMath.Divide(lhs.z * rhs.x - lhs.x * rhs.z, 1000), IntMath.Divide(lhs.x * rhs.y - lhs.y * rhs.x, 1000));
    }

    public static VInt3 Cross(VInt3 lhs, VInt3 rhs)
    {
        return new VInt3(IntMath.Divide(lhs.y * rhs.z - lhs.z * rhs.y, 1000), IntMath.Divide(lhs.z * rhs.x - lhs.x * rhs.z, 1000), IntMath.Divide(lhs.x * rhs.y - lhs.y * rhs.x, 1000));
    }

    public static VInt3 MoveTowards(VInt3 from, VInt3 to, int dt)
    {
        if ((to - from).sqrMagnitudeLong <= (long)(dt * dt))
        {
            return to;
        }
        return from + (to - from).NormalizeTo(dt);
    }

    public VInt3 NormalizeTo(int newMagn)
    {
        long num = (long)(this.x * 100);
        long num2 = (long)(this.y * 100);
        long num3 = (long)(this.z * 100);
        long num4 = num * num + num2 * num2 + num3 * num3;
        if (num4 == 0L)
        {
            return this;
        }
        long b = (long)IntMath.Sqrt(num4);
        long num5 = (long)newMagn;
        this.x = (int)IntMath.Divide(num * num5, b);
        this.y = (int)IntMath.Divide(num2 * num5, b);
        this.z = (int)IntMath.Divide(num3 * num5, b);
        return this;
    }

    public long Normalize()
    {
        long num = (long)this.x << 7;
        long num2 = (long)this.y << 7;
        long num3 = (long)this.z << 7;
        long num4 = num * num + num2 * num2 + num3 * num3;
        if (num4 == 0L)
        {
            return 0L;
        }
        long num5 = (long)IntMath.Sqrt(num4);
        long num6 = 1000L;
        this.x = (int)IntMath.Divide(num * num6, num5);
        this.y = (int)IntMath.Divide(num2 * num6, num5);
        this.z = (int)IntMath.Divide(num3 * num6, num5);
        return num5 >> 7;
    }

    public VInt3 RotateY(ref VFactor radians)
    {
        VFactor vFactor;
        VFactor vFactor2;
        IntMath.sincos(out vFactor, out vFactor2, radians.nom, radians.den);
        long num = vFactor2.nom * vFactor.den;
        long num2 = vFactor2.den * vFactor.nom;
        long b = vFactor2.den * vFactor.den;
        VInt3 vInt3;
        vInt3.x = (int)IntMath.Divide((long)this.x * num + (long)this.z * num2, b);
        vInt3.z = (int)IntMath.Divide(-(long)this.x * num2 + (long)this.z * num, b);
        vInt3.y = 0;
        return vInt3.NormalizeTo(1000);
    }

    public VInt3 RotateY(int degree)
    {
        VFactor vFactor;
        VFactor vFactor2;
        IntMath.sincos(out vFactor, out vFactor2, (long)(31416 * degree), 1800000L);
        long num = vFactor2.nom * vFactor.den;
        long num2 = vFactor2.den * vFactor.nom;
        long b = vFactor2.den * vFactor.den;
        VInt3 vInt3;
        vInt3.x = (int)IntMath.Divide((long)this.x * num + (long)this.z * num2, b);
        vInt3.z = (int)IntMath.Divide(-(long)this.x * num2 + (long)this.z * num, b);
        vInt3.y = 0;
        return vInt3.NormalizeTo(1000);
    }

    /// <summary>
    /// 球形插值
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="percent"></param>
    /// <returns></returns>
    public static VInt3 Slerp(VInt3 start, VInt3 end, VFactor percent)
    {
        if (percent == VFactor.zero)
        {
            return start;
        }
        else if (percent == VFactor.one)
        {
            return end;
        }

        long den = start.magnitude * end.magnitude;

        VFactor dot = new VFactor((long)Dot(ref start, ref end), den);

        VFactor angle = IntMath.acos(dot.nom, dot.den);

        VFactor theta = angle * percent;

        VInt3 RelativeVec = end - start * dot;
        RelativeVec.NormalizeTo(Precision);

        VFactor sin_theta, cos_thera;
        IntMath.sincos(out sin_theta, out cos_thera, theta);

        return start * cos_thera + RelativeVec * sin_theta;
    }

    /// <summary>
    /// 向量旋转
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="max_radians"></param>
    /// <returns></returns>
    public static VInt3 RotateTowards(VInt3 start, VInt3 end, VFactor max_radians)
    {
        VFactor angle = AngleInt(start, end);
        if (angle == VFactor.zero)
        {
            return end;
        }

        VFactor percent = IntMath.Min(max_radians / angle, VFactor.one);
        return Slerp(start, end, percent);
    }

    public static VInt3 Lerp(VInt3 a, VInt3 b, float f)
    {
        return new VInt3(Mathf.RoundToInt((float)a.x * (1f - f)) + Mathf.RoundToInt((float)b.x * f), Mathf.RoundToInt((float)a.y * (1f - f)) + Mathf.RoundToInt((float)b.y * f), Mathf.RoundToInt((float)a.z * (1f - f)) + Mathf.RoundToInt((float)b.z * f));
    }

    public static VInt3 Lerp(VInt3 a, VInt3 b, VFactor f)
    {
        return new VInt3((int)IntMath.Divide((long)(b.x - a.x) * f.nom, f.den) + a.x, (int)IntMath.Divide((long)(b.y - a.y) * f.nom, f.den) + a.y, (int)IntMath.Divide((long)(b.z - a.z) * f.nom, f.den) + a.z);
    }

    public static VInt3 Lerp(VInt3 a, VInt3 b, int factorNom, int factorDen)
    {
        return new VInt3(IntMath.Divide((b.x - a.x) * factorNom, factorDen) + a.x, IntMath.Divide((b.y - a.y) * factorNom, factorDen) + a.y, IntMath.Divide((b.z - a.z) * factorNom, factorDen) + a.z);
    }

    public long XZSqrMagnitude(VInt3 rhs)
    {
        long num = (long)(this.x - rhs.x);
        long num2 = (long)(this.z - rhs.z);
        return num * num + num2 * num2;
    }

    public long XZSqrMagnitude(ref VInt3 rhs)
    {
        long num = (long)(this.x - rhs.x);
        long num2 = (long)(this.z - rhs.z);
        return num * num + num2 * num2;
    }
    #endregion

    public static implicit operator string(VInt3 obj)
    {
        return obj.ToString();
    }

    /** Returns a nicely formatted string representing the vector */
    public override string ToString()
    {
        return "(" + x + "," + y + "," + z + ")";
    }

    public override bool Equals(System.Object obj)
    {
        if (obj == null) return false;

        var rhs = (VInt3)obj;

        return x == rhs.x &&
               y == rhs.y &&
               z == rhs.z;
    }

    public bool IsEqualXZ(VInt3 rhs)
    {
        return ((this.x == rhs.x) && (this.z == rhs.z));
    }

    public bool IsEqualXZ(ref VInt3 rhs)
    {
        return ((this.x == rhs.x) && (this.z == rhs.z));
    }

    public static VInt3 ProjectOnXZ(VInt3 value)
    {
        value.y = 0;
        return value;
    }

    /// <summary>
    /// Transforms a vector by a quaternion rotation.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="quat">The quaternion to rotate the vector by.</param>
    /// <returns></returns>
    public static VInt3 operator *(VQuaternion quat, VInt3 vec)
    {
        VInt3 result;
        VInt3.Transform(ref vec, ref quat, out result);
        return result;
    }

    /// <summary>
    /// Transforms a vector by a quaternion rotation.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="quat">The quaternion to rotate the vector by.</param>
    /// <returns>The result of the operation.</returns>
    public static VInt3 Transform(VInt3 vec, VQuaternion quat)
    {
        VInt3 result;
        Transform(ref vec, ref quat, out result);
        return result;
    }

    /// <summary>
    /// Transforms a vector by a quaternion rotation.
    /// </summary>
    /// <param name="vec">The vector to transform.</param>
    /// <param name="quat">The quaternion to rotate the vector by.</param>
    /// <param name="result">The result of the operation.</param>
    public static void Transform(ref VInt3 vec, ref VQuaternion quat, out VInt3 result)
    {
        // Since vec.W == 0, we can optimize quat * vec * quat^-1 as follows:
        // vec + 2.0 * cross(quat.xyz, cross(quat.xyz, vec) + quat.w * vec)
        VInt3 xyz = quat.xyz, temp, temp2;
        temp = Cross(ref xyz, ref vec);
        temp2 = vec * quat.w / Precision;
        temp += temp2;
        temp = Cross(ref xyz, ref temp);
        temp *= 2;
        result = vec + temp;
    }

    #region IEquatable implementation

    public bool Equals(VInt3 other)
    {
        return x == other.x && y == other.y && z == other.z;
    }

    #endregion

    public override int GetHashCode()
    {
        return x * 73856093 ^ y * 19349663 ^ z * 83492791;
    }

#if UNITY_EDITOR
    public static VInt3 EditorGUIVInt3Field(GUIContent content, VInt3 vint3)
    {
        UnityEditor.EditorGUIUtility.labelWidth = 10;
        content.text += "(VInt3)";
        UnityEditor.EditorGUILayout.LabelField(content);
        GUILayout.BeginHorizontal();
        vint3.x = UnityEditor.EditorGUILayout.IntField("x", vint3.x);
        vint3.y = UnityEditor.EditorGUILayout.IntField("y", vint3.y);
        vint3.z = UnityEditor.EditorGUILayout.IntField("z", vint3.z);
        GUILayout.EndHorizontal();
        UnityEditor.EditorGUIUtility.labelWidth = 0;
        return vint3;
    }
#endif
}


//}
