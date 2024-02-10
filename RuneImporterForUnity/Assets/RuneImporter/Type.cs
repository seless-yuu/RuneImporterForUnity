using System;

namespace RuneImporter
{
    [Serializable]
    public struct Int2
    {
        public int x,y;

        public static readonly Int2 zero = new Int2(0, 0);
        public static readonly Int2 one = new Int2(1, 1);

        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    [Serializable]
    public struct Int3
    {
        public int x,y,z;

        public static readonly Int3 zero = new Int3(0, 0, 0);
        public static readonly Int3 one = new Int3(1, 1, 1);

        public Int3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
