class Data<T>
{
    #region Attributes

    private T _data;

    #endregion

    #region Constructor

    public Data(T data)
    {
        _data = data;
    }

    #endregion

    #region Get/Set

    public T DataAttr { get => _data; set => _data = value; }

    #endregion
}