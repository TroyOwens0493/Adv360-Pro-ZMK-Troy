abstract class KeyLayout
{
    //Attr
    private List<Key> _layout = new();

    //Methods
    abstract public void Disp();
    
    public void DispMainKeys()
    {
    }

    abstract public void DispThumbCluster();
}
