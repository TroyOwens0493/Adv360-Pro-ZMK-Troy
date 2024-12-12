abstract class Keymap
{
    //Attr
    protected List<Key> _layout = new();
    protected List<int> _keyIndexes = new();
    protected List<int> _thumbClusterIndexes = new();
    protected List<int> _clusterEnds = new();
    protected List<int> _rowEnds = new();

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

    public int DispMainKeys(int cursorYStartPos, List<int> keysToAvoidIndexes, List<int> keysToIncludeIndexes, List<int> rowEnds)
    {
        int cursorXPos = 0;
        int cursorYPos = cursorYStartPos;
        int currentKeyIndex = 0;

        foreach (Key key in _layout)
        {
            // Skip keys that are part of the thumb cluster or other side
            if (keysToAvoidIndexes.Contains(currentKeyIndex) || !keysToIncludeIndexes.Contains(currentKeyIndex))
            {
                currentKeyIndex++;
                continue;
            }

            // Display the current key
            int keyWidth = DisplayKey(key, cursorXPos, cursorYPos, currentKeyIndex + 1);
            cursorXPos += keyWidth + 2; // Add padding between keys

            // Check if this is the end of a row
            if (rowEnds.Contains(currentKeyIndex))
            {
                cursorYPos += 1; // Move to the next line
                Console.Write(cursorYPos);
                cursorXPos = 0; // Reset X position for the new row
            }

            currentKeyIndex++;
        }
        Console.Write(cursorYStartPos);

        return cursorYPos;
    }

    public void Disp()
    {
        Console.Clear();
        // Display main keys and calculate the starting X position for the thumb cluster
        var finalYCoord = DispMainKeys(0, _thumbClusterIndexes, _keyIndexes, _rowEnds);

        //Add padding after diplaying main keys
        finalYCoord += 1;
        // Display the thumb cluster
        Console.WriteLine("\n-----------Thumbcluster keys----------");
        finalYCoord = DispMainKeys(finalYCoord, _keyIndexes, _thumbClusterIndexes, _clusterEnds);

        Console.WriteLine();
    }

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
