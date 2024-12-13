class RightSide : Keymap
{
    public RightSide(List<Key> layout)
    {
        _layout = layout;
        _keyIndexes = new List<int>
        {
            7, 8, 9, 10, 11, 12, 13,
            21, 22, 23, 24, 25, 26, 27,
            39, 40, 41, 42, 43, 44, 45,
            54, 55, 56, 57, 58, 59,
            71, 72, 73, 74, 75
        };
        _thumbClusterIndexes = new List<int>
        {
            37, 38,
            53,
            68, 69, 70
        };
        _clusterEnds = new List<int>
        {
            38, 53, 70
        };
        _rowEnds = new List<int>
        {
            13, 27, 45, 59, 75
        };
    }

    override public int GetKeyPermanentIndex(int relativeIndex)
    {
        return _keyIndexes[relativeIndex];
    }

    public List<int> GetRowEnds()
    {
        return _rowEnds;
    }
}
