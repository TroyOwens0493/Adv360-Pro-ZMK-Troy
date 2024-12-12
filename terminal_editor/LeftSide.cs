class LeftSide : Keymap
{
    public LeftSide(List<Key> layout)
    {
        _layout = layout;
        _keyIndexes = new List<int>
        {
            0, 1, 2, 3, 4, 5, 6,
            14, 15, 16, 17, 18, 19, 20,
            28, 29, 30, 31, 32, 33, 34,
            46, 47, 48, 49, 50, 51,
            60, 61, 62, 63, 64
        };
        _thumbClusterIndexes = new List<int>
        {
            35, 36,
            52,
            65, 66, 67
        };
        _clusterEnds = new List<int>
        {
            36, 52, 67
        };
        _rowEnds = new List<int>
        {
            6, 20, 34, 51, 64
        };
    }

    override public int GetKeyPermanentIndex(int relativeIndex)
    {
        return _keyIndexes[relativeIndex];
    }
}
