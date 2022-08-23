using System;
namespace Mancala
{
    public class BoardSpace
    {
        public int Index;
        public short Stones;
        public int Next;

        public BoardSpace(int index)
        {
            Next = new();
            Index = index;
            if (index != 6 && index != 13)
            {
                Stones = 4;
            }
            else
            {
                Stones = 0;
            }
        }
    }
}

