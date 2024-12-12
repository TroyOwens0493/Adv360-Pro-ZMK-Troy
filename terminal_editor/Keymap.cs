abstract class Keymap
{
    //Attr
    protected List<Key> _layout = new();

    //Methods
    public void SetLayout(List<Key> layout)
    {
        _layout = layout;
    }

    public List<Key> GetLayout()
    {
        return _layout;
    }

    abstract public int GetKeyPermanentIndex(int relativeIndex);

    public int DispMainKeys(int cursorXStartPos, List<int> thumbClusterIndexes, List<int> keyIndexes, List<int> rowends)
    {
        Console.Clear();
        int keyDisplayNumber = 1;
        int currentKeyIndex = 0;
        int cursorYPosAfterWrite = 0;
        int currentRow = 0;
        int longestRow = 0;

        foreach (Key key in _layout) //Loop through keys
        {
            int cursorColumn = 0;

            if (currentKeyIndex == rowends[0]
                    || currentKeyIndex == rowends[1]
                    || currentKeyIndex == rowends[2]
                    || currentKeyIndex == rowends[3]
                    || currentKeyIndex == rowends[4])
            {
                currentRow += 1;
            }
            else if (keyIndexes.Contains(currentKeyIndex)
                    && !thumbClusterIndexes.Contains(currentKeyIndex))
            {
                cursorColumn += DisplayKey(key, cursorColumn, currentRow, currentKeyIndex);
                if(cursorColumn > longestRow)
                {
                    longestRow = cursorColumn;
                }
                keyDisplayNumber++;
                currentKeyIndex++;
            }
            cursorYPosAfterWrite = ((currentRow) * 4) + 5;
        }
        Console.SetCursorPosition(cursorXStartPos, cursorYPosAfterWrite);

        return longestRow;
    }

    abstract public void Disp();

    abstract public int DispThumbCluster(int startingXPos);

    protected int DisplayKey(Key keyToDisp, int cursorColumn, int currentRow, int currentKeyIndex)
    {
        int cursorRow = currentRow * 4;
        int shiftForBar = 0;
        int longestLine = 0;
        List<string> keyInfo = new();
        keyInfo.Add(currentKeyIndex.ToString());
        keyInfo.Add(keyToDisp.GetKeyPress());
        keyInfo.Add(keyToDisp.GetKeyAction());

        //Loop through and print key attr
        for (var item = 0; item < keyInfo.Count(); item++)
        {
            Console.SetCursorPosition(cursorColumn, cursorRow);

            if (cursorColumn > 0)
            {
                Console.Write(" | ");
                shiftForBar = 3;
            }

            int currentLineLength = keyInfo[item]?.Length ?? 0;
            if (currentLineLength > longestLine)
            {
                longestLine = currentLineLength;
            }

            if (item == 0)
            {
                Console.Write(currentKeyIndex);
            }
            else
            {
                Console.Write(keyInfo[item]);
            }
            cursorRow++;
        }
        Console.SetCursorPosition(0, cursorRow);
        Console.Write(new string('-', cursorColumn + longestLine + shiftForBar));


        return longestLine + shiftForBar;
    }
}
