using UnityEngine;
using System.Collections;
namespace FreeLives
{
    public static class Math
    {

        static public float GetAngle(float x, float y)
        {
            return Mathf.Atan2(y, x);
        }

        static public float GetAngle(Vector2 v)
        {
            return Mathf.Atan2(v.y, v.x);
        }

        static public float GetAngle(Vector3 v)
        {
            return Mathf.Atan2(v.y, v.x);
        }

        public static Vector2 NearestPointOnLine(Vector2 start, Vector2 end, Vector2 point,
         bool clampToSegment)
        {
            // Thanks StackOverflow!
            // http://stackoverflow.com/questions/1459368/snap-point-to-a-line-java
           
            float apx = point.x - start.x;
            float apy = point.y - start.y;
            float abx = end.x - start.x;
            float aby = end.y - start.y;

            float ab2 = abx * abx + aby * aby;
            float ap_ab = apx * abx + apy * aby;
            float t = ap_ab / ab2;
            if (clampToSegment)
            {
                if (t < 0)
                {
                    t = 0;
                }
                else if (t > 1)
                {
                    t = 1;
                }
            }
            return new Vector2(start.x + abx * t, start.y + aby * t);
        }


    }
}