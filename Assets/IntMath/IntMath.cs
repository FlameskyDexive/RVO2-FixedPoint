using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class IntMath
{
    public static VFactor acos(long nom, long den)
    {
        int num = ((int)Divide((long)(nom * AcosLookupTable.HALF_COUNT), den)) + AcosLookupTable.HALF_COUNT;
        num = Mathf.Clamp(num, 0, AcosLookupTable.COUNT);
        return new VFactor { nom = AcosLookupTable.table[num], den = 0x2710L };
    }

    public static VFactor atan2(int y, int x)
    {
        int num;
        int num2;
        if (x < 0)
        {
            if (y < 0)
            {
                x = -x;
                y = -y;
                num2 = 1;
            }
            else
            {
                x = -x;
                num2 = -1;
            }
            num = -31416;
        }
        else
        {
            if (y < 0)
            {
                y = -y;
                num2 = -1;
            }
            else
            {
                num2 = 1;
            }
            num = 0;
        }
        int dIM = Atan2LookupTable.DIM;
        long num4 = dIM - 1;
        long b = (x >= y) ? ((long)x) : ((long)y);
        int num6 = (int)Divide((long)(x * num4), b);
        int num7 = (int)Divide((long)(y * num4), b);
        int num8 = Atan2LookupTable.table[(num7 * dIM) + num6];
        return new VFactor { nom = (num8 + num) * num2, den = 0x2710L };
    }

    public static int CeilPowerOfTwo(int x)
    {
        x--;
        x |= x >> 1;
        x |= x >> 2;
        x |= x >> 4;
        x |= x >> 8;
        x |= x >> 0x10;
        x++;
        return x;
    }

    public static long Clamp(long a, long min, long max)
    {
        if (a < min)
        {
            return min;
        }
        if (a > max)
        {
            return max;
        }
        return a;
    }

    public static VFactor cos(long nom, long den)
    {
        int index = SinCosLookupTable.getIndex(nom, den);
        return new VFactor((long)SinCosLookupTable.cos_table[index], (long)SinCosLookupTable.FACTOR);
    }

    //GG
    public static VFactor cos(VFactor factor)
    {
        int index = SinCosLookupTable.getIndex(factor.nom, factor.den);
        return new VFactor((long)SinCosLookupTable.cos_table[index], (long)SinCosLookupTable.FACTOR);
    }

    //GG
    public static VFactor abs(VFactor factor)
    {
        return new VFactor(Math.Abs(factor.nom), Math.Abs(factor.den));
    }

    public static int Divide(int a, int b)
    {
        //if (Math.Abs(a) == Math.Abs(b))
        //{
        //    return a / b;
        //}
        if (b == 0)
        {
            return 0;
        }
        int num = ((a ^ b) & -2147483648) >> 0x1f;
        int num2 = (num * -2) + 1;
        return ((a + ((b / 2) * num2)) / b);
    }

    public static long Divide(long a, long b)
    {
        //if (Math.Abs(a) == Math.Abs(b))
        //{
        //    return a / b;
        //}
        if (b == 0)
            return 0;

        long num = ((a ^ b) & -9223372036854775808L) >> 0x3f;
        long num2 = (num * -2L) + 1L;
        return ((a + ((b / 2L) * num2)) / b);
    }

    public static VInt2 Divide(VInt2 a, long b)
    {
        a.x = (int)Divide((long)a.x, b);
        a.y = (int)Divide((long)a.y, b);
        return a;
    }

    public static VInt3 Divide(VInt3 a, int b)
    {
        a.x = Divide(a.x, b);
        a.y = Divide(a.y, b);
        a.z = Divide(a.z, b);
        return a;
    }

    public static VInt3 Divide(VInt3 a, long b)
    {
        a.x = (int)Divide((long)a.x, b);
        a.y = (int)Divide((long)a.y, b);
        a.z = (int)Divide((long)a.z, b);
        return a;
    }

    public static VInt2 Divide(VInt2 a, long m, long b)
    {
        a.x = (int)Divide((long)(a.x * m), b);
        a.y = (int)Divide((long)(a.y * m), b);
        return a;
    }

    public static VInt3 Divide(VInt3 a, long m, long b)
    {
        a.x = (int)Divide((long)(a.x * m), b);
        a.y = (int)Divide((long)(a.y * m), b);
        a.z = (int)Divide((long)(a.z * m), b);
        return a;
    }

    /*public static VInt3 Divide(VInt3 a, long m, long b)
    {
        a.x = (int)Divide((long)(a.x * m), b);
        a.y = (int)Divide((long)(a.y * m), b);
        a.z = (int)Divide((long)(a.z * m), b);
        return a;
    }*/

    /// <summary>
    /// 线线相交
    /// </summary>
    /// <param name="seg1Src"></param>
    /// <param name="seg1Vec"></param>
    /// <param name="seg2Src"></param>
    /// <param name="seg2Vec"></param>
    /// <param name="interPoint"></param>
    /// <returns></returns>
    public static bool IntersectSegment(ref VInt2 seg1Src, ref VInt2 seg1Vec, ref VInt2 seg2Src, ref VInt2 seg2Vec, out VInt2 interPoint)
    {
        long num;
        long num2;
        long num3;
        long num4;
        long num5;
        long num6;
        SegvecToLinegen(ref seg1Src, ref seg1Vec, out num, out num2, out num3);
        SegvecToLinegen(ref seg2Src, ref seg2Vec, out num4, out num5, out num6);
        long b = (num * num5) - (num4 * num2);
        if (b != 0)
        {
            long x = Divide((long)((num2 * num6) - (num5 * num3)), b);
            long y = Divide((long)((num4 * num3) - (num * num6)), b);
            bool flag = IsPointOnSegment(ref seg1Src, ref seg1Vec, x, y) && IsPointOnSegment(ref seg2Src, ref seg2Vec, x, y);
            interPoint.x = (int)x;
            interPoint.y = (int)y;
            return flag;
        }
        interPoint = VInt2.zero;
        return false;
    }

    /// <summary>
    /// 点是否在线段上
    /// </summary>
    /// <param name="segSrc"></param>
    /// <param name="segVec"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private static bool IsPointOnSegment(ref VInt2 segSrc, ref VInt2 segVec, long x, long y)
    {
        long num = x - segSrc.x;
        long num2 = y - segSrc.y;
        return ((((segVec.x * num) + (segVec.y * num2)) >= 0L) && (((num * num) + (num2 * num2)) <= segVec.sqrMagnitudeLong));
    }

    public static bool IsPowerOfTwo(int x)
    {
        return ((x & (x - 1)) == 0);
    }

    public static int Lerp(int src, int dest, int nom, int den)
    {
        return Divide((int)((src * den) + ((dest - src) * nom)), den);
    }
    public static int Lerp(int src, int dest, VFactor factor)
    {
        return Divide((int)((src * factor.den) + (int)((dest - src) * factor.nom)), (int)factor.den);
    }

    public static long Lerp(long src, long dest, long nom, long den)
    {
        return Divide((long)((src * den) + ((dest - src) * nom)), den);
    }

    public static long Max(long a, long b)
    {
        return ((a <= b) ? b : a);
    }

    public static VFactor Max(VFactor a, VFactor b)
    {
        return a < b ? b : a;
    }

    public static VFactor Min(VFactor a, VFactor b)
    {
        return a > b ? b : a;
    }

    /// <summary>
    /// 点是否在多边形中,顶点大于3
    /// </summary>
    /// <param name="pnt"></param>
    /// <param name="plg"></param>
    /// <returns></returns>
    public static bool PointInPolygon(ref VInt2 pnt, VInt2[] plg)
    {
        if ((plg == null) || (plg.Length < 3))
        {
            return false;
        }
        bool flag = false;
        int index = 0;
        for (int i = plg.Length - 1; index < plg.Length; i = index++)
        {
            VInt2 num3 = plg[index];
            VInt2 num4 = plg[i];
            if (((num3.y <= pnt.y) && (pnt.y < num4.y)) || ((num4.y <= pnt.y) && (pnt.y < num3.y)))
            {
                int num5 = num4.y - num3.y;
                long num6 = ((pnt.y - num3.y) * (num4.x - num3.x)) - ((pnt.x - num3.x) * num5);
                if (num5 > 0)
                {
                    if (num6 > 0L)
                    {
                        flag = !flag;
                    }
                }
                else if (num6 < 0L)
                {
                    flag = !flag;
                }
            }
        }
        return flag;
    }

    /// <summary>
    ///  线段与多边形相交，多边形顶点大于等于2 
    /// </summary>
    /// <param name="segSrc"></param>
    /// <param name="segVec"></param>
    /// <param name="plg"></param>
    /// <param name="nearPoint"></param>
    /// <param name="projectVec"></param>
    /// <returns></returns>
    public static bool SegIntersectPlg(ref VInt2 segSrc, ref VInt2 segVec, VInt2[] plg, out VInt2 nearPoint, out VInt2 projectVec)
    {
        nearPoint = VInt2.zero;
        projectVec = VInt2.zero;
        if ((plg == null) || (plg.Length < 2))
        {
            return false;
        }
        bool flag = false;
        long num2 = -1L;
        int index = -1;
        for (int i = 0; i < plg.Length; i++)
        {
            VInt2 num;
            VInt2 num5 = plg[(i + 1) % plg.Length] - plg[i];
            if (IntersectSegment(ref segSrc, ref segVec, ref plg[i], ref num5, out num))
            {
                VInt2 num11 = num - segSrc;
                long sqrMagnitudeLong = num11.sqrMagnitudeLong;
                if ((num2 < 0L) || (sqrMagnitudeLong < num2))
                {
                    nearPoint = num;
                    num2 = sqrMagnitudeLong;
                    index = i;
                    flag = true;
                }
            }
        }
        if (index >= 0)
        {
            VInt2 num7 = plg[(index + 1) % plg.Length] - plg[index];
            VInt2 num8 = (segSrc + segVec) - nearPoint;
            long num9 = (num8.x * num7.x) + (num8.y * num7.y);
            if (num9 < 0L)
            {
                num9 = -num9;
                num7 = -num7;
            }
            long b = num7.sqrMagnitudeLong;
            projectVec.x = (int)Divide((long)(num7.x * num9), b);
            projectVec.y = (int)Divide((long)(num7.y * num9), b);
        }
        return flag;
    }

    public static void SegvecToLinegen(ref VInt2 segSrc, ref VInt2 segVec, out long a, out long b, out long c)
    {
        a = segVec.y;
        b = -segVec.x;
        c = (segVec.x * segSrc.y) - (segSrc.x * segVec.y);//v2差集
    }

    public static VFactor sin(long nom, long den)
    {
        int index = SinCosLookupTable.getIndex(nom, den);
        return new VFactor((long)SinCosLookupTable.sin_table[index], (long)SinCosLookupTable.FACTOR);
    }

    //GG
    public static VFactor sin(VFactor a)
    {
        int index = SinCosLookupTable.getIndex(a.nom, a.den);
        return new VFactor((long)SinCosLookupTable.sin_table[index], (long)SinCosLookupTable.FACTOR);
    }

    public static void sincos(out VFactor s, out VFactor c, VFactor angle)
    {
        int index = SinCosLookupTable.getIndex(angle.nom, angle.den);
        s = new VFactor((long)SinCosLookupTable.sin_table[index], (long)SinCosLookupTable.FACTOR);
        c = new VFactor((long)SinCosLookupTable.cos_table[index], (long)SinCosLookupTable.FACTOR);
    }

    public static void sincos(out VFactor s, out VFactor c, long nom, long den)
    {
        int index = SinCosLookupTable.getIndex(nom, den);
        s = new VFactor((long)SinCosLookupTable.sin_table[index], (long)SinCosLookupTable.FACTOR);
        c = new VFactor((long)SinCosLookupTable.cos_table[index], (long)SinCosLookupTable.FACTOR);
    }

    public static int Sqrt(long a)
    {
        if (a <= 0L)
        {
            return 0;
        }
        if (a <= 0xffffffffL)
        {
            return (int)Sqrt32((uint)a);
        }
        return (int)Sqrt64((ulong)a);
    }

    public static uint Sqrt32(uint a)
    {
        uint num = 0;
        uint num2 = 0;
        for (int i = 0; i < 0x10; i++)
        {
            num2 = num2 << 1;
            num = num << 2;
            num += a >> 30;
            a = a << 2;
            if (num2 < num)
            {
                num2++;
                num -= num2;
                num2++;
            }
        }
        return ((num2 >> 1) & 0xffff);
    }

    public static ulong Sqrt64(ulong a)
    {
        ulong num = 0L;
        ulong num2 = 0L;
        for (int i = 0; i < 0x20; i++)
        {
            num2 = num2 << 1;
            num = num << 2;
            num += a >> 0x3e;
            a = a << 2;
            if (num2 < num)
            {
                num2 += (ulong)1L;
                num -= num2;
                num2 += (ulong)1L;
            }
        }
        return ((num2 >> 1) & 0xffffffffL);
    }

    public static long SqrtLong(long a)
    {
        if (a <= 0L)
        {
            return 0L;
        }
        if (a <= 0xffffffffL)
        {
            return (long)Sqrt32((uint)a);
        }
        return (long)Sqrt64((ulong)a);
    }

    public static VInt3 Transform(ref VInt3 point, ref VInt3 forward, ref VInt3 trans)
    {
        VInt3 up = VInt3.up;
        VInt3 num2 = VInt3.Cross(VInt3.up, forward);
        return Transform(ref point, ref num2, ref up, ref forward, ref trans);
    }

    public static VInt3 Transform(VInt3 point, VInt3 forward, VInt3 trans)
    {
        VInt3 up = VInt3.up;
        VInt3 num2 = VInt3.Cross(VInt3.up, forward);
        return Transform(ref point, ref num2, ref up, ref forward, ref trans);
    }

    public static VInt3 Transform(VInt3 point, VInt3 forward, VInt3 trans, VInt3 scale)
    {
        VInt3 up = VInt3.up;
        VInt3 num2 = VInt3.Cross(VInt3.up, forward);
        return Transform(ref point, ref num2, ref up, ref forward, ref trans, ref scale);
    }

    public static VInt3 Transform(ref VInt3 point, ref VInt3 axis_x, ref VInt3 axis_y, ref VInt3 axis_z, ref VInt3 trans)
    {
        return new VInt3(Divide((int)(((axis_x.x * point.x) + (axis_y.x * point.y)) + (axis_z.x * point.z)), 0x3e8) + trans.x, Divide((int)(((axis_x.y * point.x) + (axis_y.y * point.y)) + (axis_z.y * point.z)), 0x3e8) + trans.y, Divide((int)(((axis_x.z * point.x) + (axis_y.z * point.y)) + (axis_z.z * point.z)), 0x3e8) + trans.z);
    }

    public static VInt3 Transform(VInt3 point, ref VInt3 axis_x, ref VInt3 axis_y, ref VInt3 axis_z, ref VInt3 trans)
    {
        return new VInt3(Divide((int)(((axis_x.x * point.x) + (axis_y.x * point.y)) + (axis_z.x * point.z)), 0x3e8) + trans.x, Divide((int)(((axis_x.y * point.x) + (axis_y.y * point.y)) + (axis_z.y * point.z)), 0x3e8) + trans.y, Divide((int)(((axis_x.z * point.x) + (axis_y.z * point.y)) + (axis_z.z * point.z)), 0x3e8) + trans.z);
    }

    public static VInt3 Transform(ref VInt3 point, ref VInt3 axis_x, ref VInt3 axis_y, ref VInt3 axis_z, ref VInt3 trans, ref VInt3 scale)
    {
        long num = point.x * scale.x;
        long num2 = point.y * scale.x;
        long num3 = point.z * scale.x;
        return new VInt3(((int)Divide((long)(((axis_x.x * num) + (axis_y.x * num2)) + (axis_z.x * num3)), (long)0xf4240L)) + trans.x, ((int)Divide((long)(((axis_x.y * num) + (axis_y.y * num2)) + (axis_z.y * num3)), (long)0xf4240L)) + trans.y, ((int)Divide((long)(((axis_x.z * num) + (axis_y.z * num2)) + (axis_z.z * num3)), (long)0xf4240L)) + trans.z);
    }

    /// <summary>
    /// 解二次方程
    /// </summary>
    /// <returns></returns>
    public static void ResolveQuadraticEquation(VFactor a, VFactor b, VFactor c, ref VFactor root1, ref VFactor root2)
    {
        VFactor _delta = new VFactor(Sqrt(((b * b - a * c * 4) * 10000).roundInt), 100);
        root1 = (-b + _delta) / a / 2;
        root2 = (-b - _delta) / a / 2;
    }

    /// <summary>
    /// 只取叉积Y值
    /// </summary>
    /// <param name="l"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public static int CrossY(VInt3 l, VInt3 r)
    {
        return l.z * r.x - l.x * r.z;
    }


    /** Returns the intersection point between the two lines. Lines are treated as infinite. \a start1 is returned if the lines are parallel */
    public static VInt3 LineIntersectionPointXZ(VInt3 start1, VInt3 end1, VInt3 start2, VInt3 end2)
    {
        bool s;

        return LineIntersectionPointXZ(start1, end1, start2, end2, out s);
    }

    /** Returns the intersection point between the two lines. Lines are treated as infinite. \a start1 is returned if the lines are parallel */
    public static VInt3 LineIntersectionPointXZ(VInt3 start1, VInt3 end1, VInt3 start2, VInt3 end2, out bool intersects)
    {
        VInt3 dir1 = end1 - start1;
        VInt3 dir2 = end2 - start2;

        long den = dir2.z * dir1.x - dir2.x * dir1.z;

        if (den == 0)
        {
            intersects = false;
            return start1;
        }

        long nom = dir2.x * (start1.z - start2.z) - dir2.z * (start1.x - start2.x);

        long u = nom / den;

        intersects = true;
        return start1 + dir1 * u;
    }

    /** Returns the intersection point between the two lines. Lines are treated as infinite. \a start1 is returned if the lines are parallel */
    public static VInt2 LineIntersectionPoint(VInt2 start1, VInt2 end1, VInt2 start2, VInt2 end2)
    {
        bool s;

        return LineIntersectionPoint(start1, end1, start2, end2, out s);
    }

    /** Returns the intersection point between the two lines. Lines are treated as infinite. \a start1 is returned if the lines are parallel */
    public static VInt2 LineIntersectionPoint(VInt2 start1, VInt2 end1, VInt2 start2, VInt2 end2, out bool intersects)
    {
        VInt2 dir1 = end1 - start1;
        VInt2 dir2 = end2 - start2;

        long den = dir2.y * dir1.x - dir2.x * dir1.y;

        if (den == 0)
        {
            intersects = false;
            return start1;
        }

        long nom = dir2.x * (start1.y - start2.y) - dir2.y * (start1.x - start2.x);

        long u = nom / den;

        intersects = true;
        return start1 + dir1 * (int)u;
    }

    /** Returns the intersection point between the two line segments in XZ space.
     * Lines are NOT treated as infinite. \a start1 is returned if the line segments do not intersect
     * The point will be returned along the line [start1, end1] (this matters only for the y coordinate).
     */
    public static VInt3 SegmentIntersectionPointXZ(VInt3 start1, VInt3 end1, VInt3 start2, VInt3 end2, out bool intersects)
    {
        VInt3 dir1 = end1 - start1;
        VInt3 dir2 = end2 - start2;

        int den = dir2.z * dir1.x - dir2.x * dir1.z;

        if (den == 0)
        {
            intersects = false;
            return start1;
        }

        int nom = dir2.x * (start1.z - start2.z) - dir2.z * (start1.x - start2.x);
        int nom2 = dir1.x * (start1.z - start2.z) - dir1.z * (start1.x - start2.x);
        int u = nom / den;
        int u2 = nom2 / den;

        if (u < 0F || u > 1F || u2 < 0F || u2 > 1F)
        {
            intersects = false;
            return start1;
        }

        intersects = true;
        return start1 + dir1 * u;
    }

    /** Does the line segment intersect the bounding box.
     * The line is NOT treated as infinite.
     * \author Slightly modified code from http://www.3dkingdoms.com/weekly/weekly.php?a=21
     */
    public static bool SegmentIntersectsBounds(Bounds bounds, VInt3 a, VInt3 b)
    {
        // Put segment in box space
        a -= (VInt3)bounds.center;
        b -= (VInt3)bounds.center;

        // Get line midpoint and extent
        var LMid = (a + b) * 0.5F;
        var L = (a - LMid);
        var LExt = new Vector3(Math.Abs(L.x), Math.Abs(L.y), Math.Abs(L.z));

        Vector3 extent = bounds.extents;

        // Use Separating Axis Test
        // Separation vector from box center to segment center is LMid, since the line is in box space
        if (Math.Abs(LMid.x) > extent.x + LExt.x) return false;
        if (Math.Abs(LMid.y) > extent.y + LExt.y) return false;
        if (Math.Abs(LMid.z) > extent.z + LExt.z) return false;
        // Crossproducts of line and each axis
        if (Math.Abs(LMid.y * L.z - LMid.z * L.y) > (extent.y * LExt.z + extent.z * LExt.y)) return false;
        if (Math.Abs(LMid.x * L.z - LMid.z * L.x) > (extent.x * LExt.z + extent.z * LExt.x)) return false;
        if (Math.Abs(LMid.x * L.y - LMid.y * L.x) > (extent.x * LExt.y + extent.y * LExt.x)) return false;
        // No separating axis, the line intersects
        return true;
    }


    /** Returns the closest point on the segment.
     * The segment is NOT treated as infinite.
     * \see ClosestPointOnLine
     * \see ClosestPointOnSegmentXZ
     */
    public static VInt3 ClosestPointOnSegment(VInt3 lineStart, VInt3 lineEnd, VInt3 point)
    {
        var dir = lineEnd - lineStart;
        long sqrMagn = dir.sqrMagnitudeLong;

        if (sqrMagn <= 1) return lineStart;

        long factor = VInt3.DotLong(point - lineStart, dir) / sqrMagn;
        return (lineStart + dir * IntMath.Clamp(0, 1000,factor));
    }
    public static VInt2 ClosestPointOnSegment(VInt2 lineStart, VInt2 lineEnd, VInt2 point)
    {
        var dir = lineEnd - lineStart;
        long sqrMagn = dir.sqrMagnitudeLong;

        if (sqrMagn <= 1) return lineStart;

        long factor = VInt2.DotLong(point - lineStart, dir) / sqrMagn;
        return (lineStart + dir * (int)IntMath.Clamp(0, 1000,factor));
    }

    /** Returns the closest point on the segment in the XZ plane.
     * The y coordinate of the result will be the same as the y coordinate of the \a point parameter.
     *
     * The segment is NOT treated as infinite.
     * \see ClosestPointOnSegment
     * \see ClosestPointOnLine
     */
    public static VInt3 ClosestPointOnSegmentXZ(VInt3 lineStart, VInt3 lineEnd, VInt3 point)
    {
        lineStart.y = point.y;
        lineEnd.y = point.y;
        VInt3 fullDirection = lineEnd - lineStart;
        VInt3 fullDirection2 = fullDirection;
        fullDirection2.y = 0;
        int magn = fullDirection2.magnitude;
        VInt3 lineDirection = magn > float.Epsilon ? fullDirection2 / magn : VInt3.zero;

        int closestPoint = VInt3.Dot((point - lineStart), lineDirection);
        int a = (int)Clamp(closestPoint, 0, fullDirection2.magnitude);
        return lineStart + ( lineDirection * a);
    }

    /** Returns the closest point on the segment in the XZ plane.
     * The y coordinate of the result will be the same as the y coordinate of the \a point parameter.
     *
     * The segment is NOT treated as infinite.
     * \see ClosestPointOnSegment
     * \see ClosestPointOnLine
     */
    public static VInt2 ClosestPointOnSegmentXZ(VInt2 lineStart, VInt2 lineEnd, VInt2 point)
    {
        lineStart.y = point.y;
        lineEnd.y = point.y;
        VInt2 fullDirection = lineEnd - lineStart;
        VInt2 fullDirection2 = fullDirection;
        fullDirection2.y = 0;
        int magn = fullDirection2.magnitude;
        VInt2 lineDirection = magn > float.Epsilon ? fullDirection2 / magn : VInt2.zero;

        int closestPoint = VInt2.Dot((point - lineStart), lineDirection);
        int a = (int)Clamp(closestPoint, 0, fullDirection2.magnitude);
        return lineStart + ( lineDirection * a);
    }

    /** Returns the intersection factor for line 1 with line 2.
     * The intersection factor is a distance along the line \a start1 - \a end1 where the line \a start2 - \a end2 intersects it.\n
     * \code intersectionPoint = start1 + intersectionFactor * (end1-start1) \endcode.
     * Lines are treated as infinite.\n
     * -1 is returned if the lines are parallel (note that this is a valid return value if they are not parallel too) */
    public static long LineIntersectionFactorXZ(VInt3 start1, VInt3 end1, VInt3 start2, VInt3 end2)
    {
        VInt3 dir1 = end1 - start1;
        VInt3 dir2 = end2 - start2;

        long den = dir2.z * dir1.x - dir2.x * dir1.z;

        if (den == 0)
        {
            return -1;
        }

        long nom = dir2.x * (start1.z - start2.z) - dir2.z * (start1.x - start2.x);
        long u = nom / den;

        return u;
    }


    /** Factor along the line which is closest to the point.
     * Returned value is in the range [0,1] if the point lies on the segment otherwise it just lies on the line.
     * The closest point can be calculated using (end-start)*factor + start
     */
    public static long ClosestPointOnLineFactor(VInt3 lineStart, VInt3 lineEnd, VInt3 point)
    {
        var lineDirection = lineEnd - lineStart;
        long magn = lineDirection.sqrMagnitudeLong;

        long closestPoint = VInt3.Dot((point - lineStart), lineDirection);

        if (magn != 0) closestPoint /= magn;

        return closestPoint;
    }

    /** Factor of the nearest point on the segment.
     * Returned value is in the range [0,1] if the point lies on the segment otherwise it just lies on the line.
     * The closest point can be calculated using (end-start)*factor + start;
     */
    public static long ClosestPointOnLineFactor(VInt2 lineStart, VInt2 lineEnd, VInt2 point)
    {
        var lineDirection = lineEnd - lineStart;
        long magn = lineDirection.sqrMagnitudeLong;

        long closestPoint = VInt2.DotLong(point - lineStart, lineDirection);

        if (magn != 0) closestPoint /= magn;

        return closestPoint;
    }


    public static float NearestPointFactor(VInt2 lineStart, VInt2 lineEnd, VInt2 point)
    {
        VInt2 b = lineEnd - lineStart;
        double sqrMagnitudeLong = b.sqrMagnitudeLong;
        double num3 = VInt2.DotLong(point - lineStart, b);
        if (sqrMagnitudeLong != 0.0)
        {
            num3 /= sqrMagnitudeLong;
        }
        return (float)num3;
    }

    public static float NearestPointFactor(VInt3 lineStart, VInt3 lineEnd, VInt3 point)
    {
        VInt3 rhs = lineEnd - lineStart;
        double sqrMagnitude = rhs.sqrMagnitude;
        double num3 = VInt3.Dot(point - lineStart, rhs);
        if (sqrMagnitude != 0.0)
        {
            num3 /= sqrMagnitude;
        }
        return (float)num3;
    }

    public static VFactor NearestPointFactor(ref VInt3 lineStart, ref VInt3 lineEnd, ref VInt3 point)
    {
        VInt3 rhs = lineEnd - lineStart;
        long sqrMagnitudeLong = rhs.sqrMagnitudeLong;
        VFactor zero = VFactor.zero;
        zero.nom = VInt3.DotLong(point - lineStart, rhs);
        if (sqrMagnitudeLong != 0)
        {
            zero.den = sqrMagnitudeLong;
        }
        return zero;
    }

    public static float NearestPointFactorXZ(VInt3 lineStart, VInt3 lineEnd, VInt3 point)
    {
        VInt2 b = new VInt2(lineEnd.x - lineStart.x, lineEnd.z - lineStart.z);
        double sqrMagnitude = b.sqrMagnitude;
        VInt2 a = new VInt2(point.x - lineStart.x, point.z - lineStart.z);
        double num4 = VInt2.Dot(a, b);
        if (sqrMagnitude != 0.0)
        {
            num4 /= sqrMagnitude;
        }
        return (float)num4;
    }

    public static VFactor NearestPointFactorXZ(ref VInt3 lineStart, ref VInt3 lineEnd, ref VInt3 point)
    {
        VInt2 b = new VInt2(lineEnd.x - lineStart.x, lineEnd.z - lineStart.z);
        VInt2 a = new VInt2(point.x - lineStart.x, point.z - lineStart.z);
        long sqrMagnitudeLong = b.sqrMagnitudeLong;
        VFactor zero = VFactor.zero;
        zero.nom = VInt2.DotLong(a, b);
        if (sqrMagnitudeLong != 0)
        {
            zero.den = sqrMagnitudeLong;
        }
        return zero;
    }

    public static float NearestPointFloatXZ(VInt3 lineStart, VInt3 lineEnd, VInt3 point)
    {
        VInt2 b = new VInt2(lineEnd.x - lineStart.x, lineEnd.z - lineStart.z);
        double num = (double)b.sqrMagnitude;
        VInt2 a = new VInt2(point.x - lineStart.x, point.z - lineStart.z);
        double num2 = (double)VInt2.Dot(a, b);
        if (num != 0.0)
        {
            num2 /= num;
        }
        return (float)num2;
    }

    public static VInt3 NearestPointStrict(VInt3 lineStart, VInt3 lineEnd, VInt3 point)
    {
        return NearestPointStrict(ref lineStart, ref lineEnd, ref point);
    }

    public static VInt3 NearestPointStrict(ref VInt3 lineStart, ref VInt3 lineEnd, ref VInt3 point)
    {
        VInt3 rhs = lineEnd - lineStart;
        long sqrMagnitudeLong = rhs.sqrMagnitudeLong;
        if (sqrMagnitudeLong == 0)
        {
            return lineStart;
        }
        long m = IntMath.Clamp(VInt3.DotLong(point - lineStart, rhs), 0L, sqrMagnitudeLong);
        return (IntMath.Divide(rhs, m, sqrMagnitudeLong) + lineStart);
    }

    public static VInt3 NearestPointStrictXZ(ref VInt3 lineStart, ref VInt3 lineEnd, ref VInt3 point)
    {
        VInt3 rhs = lineEnd - lineStart;
        long max = rhs.sqrMagnitudeLong2D;
        if (max == 0)
        {
            return lineStart;
        }
        long m = IntMath.Clamp(VInt3.DotXZLong(point - lineStart, rhs), 0L, max);
        return (IntMath.Divide(rhs, m, max) + lineStart);
    }


    /** Closest point on the triangle \a abc to the point \a p.
     * \see 'Real Time Collision Detection' by Christer Ericson, chapter 5.1, page 141
     */
    public static VInt2 ClosestPointOnTriangle(VInt2 a, VInt2 b, VInt2 c, VInt2 p)
    {
        // Check if p is in vertex region outside A
        var ab = b - a;
        var ac = c - a;
        var ap = p - a;

        var d1 = VInt2.Dot(ab, ap);
        var d2 = VInt2.Dot(ac, ap);

        // Barycentric coordinates (1,0,0)
        if (d1 <= 0 && d2 <= 0)
        {
            return a;
        }

        // Check if p is in vertex region outside B
        var bp = p - b;
        var d3 = VInt2.Dot(ab, bp);
        var d4 = VInt2.Dot(ac, bp);

        // Barycentric coordinates (0,1,0)
        if (d3 >= 0 && d4 <= d3)
        {
            return b;
        }

        // Check if p is in edge region outside AB, if so return a projection of p onto AB
        if (d1 >= 0 && d3 <= 0)
        {
            var vc = d1 * d4 - d3 * d2;
            if (vc <= 0)
            {
                // Barycentric coordinates (1-v, v, 0)
                var v = d1 / (d1 - d3);
                return a + ab * v;
            }
        }

        // Check if p is in vertex region outside C
        var cp = p - c;
        var d5 = VInt2.Dot(ab, cp);
        var d6 = VInt2.Dot(ac, cp);

        // Barycentric coordinates (0,0,1)
        if (d6 >= 0 && d5 <= d6)
        {
            return c;
        }

        // Check if p is in edge region of AC, if so return a projection of p onto AC
        if (d2 >= 0 && d6 <= 0)
        {
            var vb = d5 * d2 - d1 * d6;
            if (vb <= 0)
            {
                // Barycentric coordinates (1-v, 0, v)
                var v = d2 / (d2 - d6);
                return a + ac * v;
            }
        }

        // Check if p is in edge region of BC, if so return projection of p onto BC
        if ((d4 - d3) >= 0 && (d5 - d6) >= 0)
        {
            var va = d3 * d6 - d5 * d4;
            if (va <= 0)
            {
                var v = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return b + (c - b) * v;
            }
        }

        return p;
    }

    /** Closest point on the triangle \a abc to the point \a p when seen from above.
     * \see 'Real Time Collision Detection' by Christer Ericson, chapter 5.1, page 141
     */
    public static VInt3 ClosestPointOnTriangleXZ(VInt3 a, VInt3 b, VInt3 c, VInt3 p)
    {
        // Check if p is in vertex region outside A
        var ab = new VInt2(b.x - a.x, b.z - a.z);
        var ac = new VInt2(c.x - a.x, c.z - a.z);
        var ap = new VInt2(p.x - a.x, p.z - a.z);

        var d1 = VInt2.Dot(ab, ap);
        var d2 = VInt2.Dot(ac, ap);

        // Barycentric coordinates (1,0,0)
        if (d1 <= 0 && d2 <= 0)
        {
            return a;
        }

        // Check if p is in vertex region outside B
        var bp = new VInt2(p.x - b.x, p.z - b.z);
        var d3 = VInt2.Dot(ab, bp);
        var d4 = VInt2.Dot(ac, bp);

        // Barycentric coordinates (0,1,0)
        if (d3 >= 0 && d4 <= d3)
        {
            return b;
        }

        // Check if p is in edge region outside AB, if so return a projection of p onto AB
        var vc = d1 * d4 - d3 * d2;
        if (d1 >= 0 && d3 <= 0 && vc <= 0)
        {
            // Barycentric coordinates (1-v, v, 0)
            var v = d1 / (d1 - d3);
            return a * (1 - v) + b * v;
        }

        // Check if p is in vertex region outside C
        var cp = new VInt2(p.x - c.x, p.z - c.z);
        var d5 = VInt2.Dot(ab, cp);
        var d6 = VInt2.Dot(ac, cp);

        // Barycentric coordinates (0,0,1)
        if (d6 >= 0 && d5 <= d6)
        {
            return c;
        }

        // Check if p is in edge region of AC, if so return a projection of p onto AC
        var vb = d5 * d2 - d1 * d6;
        if (d2 >= 0 && d6 <= 0 && vb <= 0)
        {
            // Barycentric coordinates (1-v, 0, v)
            var v = d2 / (d2 - d6);
            return a * (1 - v) + c * v;
        }

        // Check if p is in edge region of BC, if so return projection of p onto BC
        var va = d3 * d6 - d5 * d4;
        if ((d4 - d3) >= 0 && (d5 - d6) >= 0 && va <= 0)
        {
            var v = (d4 - d3) / ((d4 - d3) + (d5 - d6));
            return b + (c - b) * v;
        }
        else
        {
            // P is inside the face region. Compute the point using its barycentric coordinates (u, v, w)
            // Note that the x and z coordinates will be exactly the same as P's x and z coordinates
            var denom = 1000 / (va + vb + vc);
            var v = vb * denom;
            var w = vc * denom;

            return new VInt3(p.x, (1 - v - w) * a.y + v * b.y + w * c.y, p.z);
        }
    }

    /** Closest point on the triangle \a abc to the point \a p.
     * \see 'Real Time Collision Detection' by Christer Ericson, chapter 5.1, page 141
     * 需要考虑溢出原因，所以平方和的单精度只放大10倍
     */
    public static VInt3 ClosestPointOnTriangle(VInt3 a, VInt3 b, VInt3 c, VInt3 p)
    {
        // Check if p is in vertex region outside A
        var ab = b - a;
        var ac = c - a;
        var ap = p - a;

        var d1 = VInt3.DotLongSafe(ab, ap);
        var d2 = VInt3.DotLongSafe(ac, ap);

        // Barycentric coordinates (1,0,0)
        if (d1 <= 0 && d2 <= 0)
            return a;

        // Check if p is in vertex region outside B
        var bp = p - b;
        var d3 = VInt3.DotLongSafe(ab, bp);
        var d4 = VInt3.DotLongSafe(ac, bp);

        // Barycentric coordinates (0,1,0)
        if (d3 >= 0 && d4 <= d3)
            return b;

        /*CheckIntOverFlow(d1);
        CheckIntOverFlow(d2);
        CheckIntOverFlow(d3);
        CheckIntOverFlow(d4);*/
        // Check if p is in edge region outside AB, if so return a projection of p onto AB
        var vc = d1 * d4 - d3 * d2;
        if (d1 >= 0 && d3 <= 0 && vc <= 0)
        {
            // Barycentric coordinates (1-v, v, 0)
            /*var v = d1 / (d1 - d3);
            return a + ab * v;*/
            //Modify
            VFactor vf = new VFactor(d1, d1 - d3);
            //Debug.Log("==type 1==" + (a + ab * vf));
            return a + ab * vf;
        }

        // Check if p is in vertex region outside C
        var cp = p - c;
        var d5 = VInt3.DotLongSafe(ab, cp);
        var d6 = VInt3.DotLongSafe(ac, cp);

        /*CheckIntOverFlow(d5);
        CheckIntOverFlow(d6);*/
        // Barycentric coordinates (0,0,1)
        if (d6 >= 0 && d5 <= d6)
            return c;

        // Check if p is in edge region of AC, if so return a projection of p onto AC
        var vb = d5 * d2 - d1 * d6;
        if (d2 >= 0 && d6 <= 0 && vb <= 0)
        {
            // Barycentric coordinates (1-v, 0, v)
            /*var v = d2 / (d2 - d6);
            return a + ac * v;*/
            //Modify
            VFactor vf = new VFactor(d2, d2 - d6);
            //Debug.Log("==type 2==" + (a + ac * vf));
            return a + ac * vf;
        }

        // Check if p is in edge region of BC, if so return projection of p onto BC
        var va = d3 * d6 - d5 * d4;
        if ((d4 - d3) >= 0 && (d5 - d6) >= 0 && va <= 0)
        {
            /*var v = (d4 - d3) / ((d4 - d3) + (d5 - d6));
            return b + (c - b) * v;*/
            //Modify
            VFactor vf = new VFactor((d4 - d3), (d4 - d3) + (d5 - d6));
            //Debug.Log("==type 3==" + (b + (c - b) * vf));
            return b + (c - b) * vf;
        }
        else
        {
            // P is inside the face region. Compute the point using its barycentric coordinates (u, v, w)
            //var denom = 1f / (va + vb + vc);
            //var v = vb * denom;
            //var w = vc * denom;
            //Modify
            VFactor vf = new VFactor(1, va + vb + vc);
            var v = vf * vb ;
            var w = vf * vc;

            // This is equal to: u*a + v*b + w*c, u = va*denom = 1 - v - w;
            //Debug.Log("==type 4==" + (a + ab * v + ac * w));
            return a + ab * v + ac * w;
        }
    }

    static void CheckIntOverFlow(long value)
    {
        //Debug.Log("===============" + value);
        if (value > int.MaxValue)
            Debug.LogError("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
    }

    public static List<Vector3> Int3s2Vector3s(List<VInt3> int3s)
    {
        List<Vector3> vector3s = new List<Vector3>();
        for (int i = 0; i < int3s.Count; i++)
        {
            vector3s.Add((Vector3)int3s[i]);
        }

        return vector3s;
    }

    public static List<VInt3> Vector3s2Int3s(List<Vector3> vector3s)
    {
        List<VInt3> int3s = new List<VInt3>();
        for (int i = 0; i < vector3s.Count; i++)
        {
            int3s.Add((VInt3)vector3s[i]);
        }

        return int3s;
    }
    /** Complex number multiplication.
     * \returns a * b
     *
     * Used to rotate vectors in an efficient way.
     *
     * \see https://en.wikipedia.org/wiki/Complex_number#Multiplication_and_division
     */
    public static VInt2 ComplexMultiply(VInt2 a, VInt2 b)
    {
        return new VInt2(a.x * b.x - a.y * b.y, a.x * b.y + a.y * b.x);
    }
}

