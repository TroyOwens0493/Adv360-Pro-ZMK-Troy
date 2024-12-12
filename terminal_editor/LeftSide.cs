class LeftSide : Keymap
{
    //Attr
    private List<int> _keyIndexes = new List<int>
        {
            0, 1, 2, 3, 4, 5, 6,
            14, 15, 16, 17, 18, 19, 20,
            28, 29, 30, 31, 32, 33, 34,
            46, 47, 48, 49, 50, 51,
            60, 61, 62, 63, 64
        };
    private List<int> _thumbClusterIndexes = new List<int>
        {
            35, 36,
            52,
            65, 66, 67
        };

    private List<int> _clusterEnds = new List<int>
        {
            36, 52, 67
        };

    private List<int> _rowEnds = new List<int>
        {
            6, 20, 34, 51, 64
        };

    //Methods
    public LeftSide(List<Key> layout)
    {
        _layout = layout;
    }

    override public int GetKeyPermanentIndex(int relativeIndex)
    {
        return _keyIndexes[relativeIndex];
    }

    override public void Disp()
    {
        int startingXPos = DispMainKeys(0, _thumbClusterIndexes, _keyIndexes, _rowEnds);
        DispThumbCluster(startingXPos + 10);//Add some padding
    }

    override public int DispThumbCluster(int startingXPos)
    {
        foreach(int index in _thumbClusterIndexes)
        {
            Key currentKey = _layout[index];
            DisplayKey(currentKey, startingXPos, 0, index);
        }

        return 1;
    }
}
