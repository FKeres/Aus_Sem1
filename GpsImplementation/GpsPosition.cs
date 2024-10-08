class GpsPosition
{
    #region Attribute
    
    private char _width;
    private double _widthPosition;
    private char _length;
    private double _lengthPosition;
    
    #endregion 

    #region Get/Set

    public char Width { get => _width; set => _width = value; }
    public double WidthPosition { get => _widthPosition; set => _widthPosition = value; }
    public char Length { get => _length; set => _length = value; }
    public double LengthPosition { get => _lengthPosition; set => _lengthPosition = value; }

    #endregion

    #region Constructor
    public GpsPosition(char width, double widthPosition, char length, double lengthPosition)
    {
        _width = width;
        _widthPosition = widthPosition;
        _length = length;
        _lengthPosition = lengthPosition;
    }

    public GpsPosition(GpsPosition other) {
        _width = other.Width;
        _widthPosition = other.WidthPosition;
        _length = other.Length;
        _lengthPosition = other.LengthPosition;

    }
    #endregion


}