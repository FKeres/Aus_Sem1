class GpsPosHandler
{
    /// <summary>
    /// Handles positions in Parcel and Property
    /// </summary>
    #region Attributes

    private GpsPosition[] _gpsPositions = new GpsPosition[2];

    #endregion

    #region  Constructor

    public GpsPosHandler(GpsPosition[] gpsPositions)
    {
        _gpsPositions = gpsPositions;
    }

    public GpsPosHandler() {
        _gpsPositions = new GpsPosition[2];
    }

    #endregion

    #region Get/Set

    internal GpsPosition[] GpsPositioons { get => _gpsPositions; set => _gpsPositions = value; }
    
    public GpsPosition GetGpsPosition(int position) {
        if(this.CheckCorrectPos(position)){
            return this._gpsPositions[position];
        } else {
            throw new ArithmeticException("Position out of bounds.");
        }

    }
    #endregion

     #region Methods

    /// <summary>
    /// checks if position is not out of range
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CheckCorrectPos(int position) {
        if(position < 0 && position > _gpsPositions.Length -1) {
            return false;
        }
        return true;
    }
    
    /// <summary>
    /// creates new Gps Position and assigns to given position  
    /// </summary>
    /// <param name="position"></param>
    /// <param name="width"></param>
    /// <param name="widthPosition"></param>
    /// <param name="length"></param>
    /// <param name="lengthPosition"></param>
    /// <returns></returns>
    public bool AddNewGpsPosition(int position, char width, double widthPosition, char length, double lengthPosition) {

        if(this.CheckCorrectPos(position)) {

            GpsPosition newGpsPos = new GpsPosition(width, widthPosition, length, lengthPosition);
            _gpsPositions[position] = newGpsPos;

            return true;
        }

        return false;

    }

    /// <summary>
    ///  adds already created Gps position to given position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="gpsPosition"></param>
    /// <returns></returns>
   
    public bool AddExistingGpsPos(int position, GpsPosition gpsPosition) {
        if(this.CheckCorrectPos(position)) {
            _gpsPositions[position] = gpsPosition;

            return true;
        }

        return false;

    }

    #endregion
}