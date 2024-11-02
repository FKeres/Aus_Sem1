public abstract class TerritorialUnit<T>
{
    #region Attributes
    private List<T> _items;
    #endregion

    #region Constructor
    public TerritorialUnit()
    {
        _items = new List<T>();
    }
    #endregion

    #region Get/Set
    protected List<T> Items { get => _items; set => _items = value; }

    protected T GetListItem(int position) {
        return _items[position];
    }

    #endregion

    #region Methods

    protected void AddListElement(T listItem){
        _items.Add(listItem);
    }

    protected void RemoveListElement(int position) {
        _items.RemoveAt(position);
    }

     protected void RemoveListElementEq(T item) {
        int index = 0;
        foreach(var thing in _items) {
            if(_items.Equals(item)) {
                _items.RemoveAt(index);
                break;
            }
            ++index;
        }
    }

    #endregion
}