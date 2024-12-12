class RightSide : Keymap
{
    //Attr
    private List<int> _keyIndexes = new List<int>
        {
            7, 8, 9, 10, 11, 12, 13,
            21, 22, 23, 24, 25, 26, 27,
            39, 40, 41, 42, 43, 44, 45,
            54, 55, 56, 57, 58, 59,
            71, 72, 73, 74, 75
        };
    private List<int> _thumbClusterIndexes = new List<int>
        {
            37, 38,
            53,
            68, 69, 70
        };

    private List<int> _rowEnds = new List<int>
        {
            13, 27, 45, 59, 75
        };

    //Methods 
    public RightSide(List<Key> layout)
    {
        _layout = layout;
    }

    override public int GetKeyPermanentIndex(int relativeIndex)
    {
        return _keyIndexes[relativeIndex];
    }

    override public void Disp()
    {
        int xStartPos = DispThumbCluster(0);
        DispMainKeys(xStartPos, _thumbClusterIndexes, _keyIndexes, _rowEnds);
    }

    override public int DispThumbCluster(int startingXPos)
    {
        int longestRow = 0;
        foreach(int index in _thumbClusterIndexes)
        {
            Key currentKey = _layout[index];
            int currentRowLen = DisplayKey(currentKey, startingXPos, 0, index);
            if(currentRowLen > longestRow)
            {
                longestRow = currentRowLen;
            }
        }

        return longestRow;
    }
}
