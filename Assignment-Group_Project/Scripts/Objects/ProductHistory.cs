using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Assignment_Group_Project.Scripts.Objects
{
    public class ProductHistory
    {

        public int ArticalNumber { get; set; } = 0;
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public int Day { get; set; } = 0;
        //public Vector3 Time { get; set; } = new Vector3();
        public Vector3C Time { get; set; }
        public string? Name { get; set; } = null;
        public string? Company { get; set; } = null;
        public enum Status
        {
            Undefind,
            Create,
            Added,
            Removed,
            Updated,
            Cancelled

        };

        public Status status { get; set; } = Status.Undefind;

        public struct Vector3C
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

            public Vector3C(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

    }
}
