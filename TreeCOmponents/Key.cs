class Key :  IComparable<Key>
{
    #region Attributes

    
    private IComparable _key;

    #endregion

    #region Constructor

    public Key(Object key)
    {
        if (key is not IComparable keyIC)
        {
            throw new ArgumentException("Key must implement IComparable.");
        }

        _key = keyIC;
    }

    #endregion

    #region Get/Set

    public IComparable KeyAttr { 
        get => _key; 
    }

    public int CompareTo(Key? other){
         if (other == null)
        {
            return 1;
        }

        if(!(other.KeyAttr.GetType() == _key.GetType())) {
            throw new ArgumentException("Key types must be the same.");
        }

        return _key.CompareTo(other._key);
    }

    #endregion

}

