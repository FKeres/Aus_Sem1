class Key :  IComparable<Key>
{
    #region Attributes

    
    private Object _key;

    #endregion

    #region Constructor

    public Key(Object key)
    {
        if (!(key is IComparable))
        {
            throw new ArgumentException("Key must implement IComparable.");
        }

        _key = key;
    }

    #endregion

    #region Get/Set

    public Object KeyAttr { 
        get => _key; 
        set
        {
            if (!(value is IComparable))
            {
                throw new ArgumentException("Key must implement IComparable.");
            }
            _key = value;
        } 
    }

    public int CompareTo(Key? other){
         if (other == null)
        {
            return 1;
        }

        return ((IComparable)_key).CompareTo(other._key);
    }

    #endregion

}

