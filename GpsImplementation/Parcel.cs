class Parcel :TerritorialUnit<Property>
{
    #region Attributes

    private int _parcNo;
    private string _parcDesc;
    private GpsPosHandler _gpsPosHandler;

    #endregion

    #region Constructor

    public Parcel(int parcNo, string parcDesc, GpsPosition[] gpsPositions)
    {
        _parcNo = parcNo;
        _parcDesc = parcDesc;
        _gpsPosHandler = new GpsPosHandler(gpsPositions);
    }

    public Parcel(int parcNo, string parcDesc, char width1, double widthPosition1, char length1, double lengthPosition1, char width2, double widthPosition2, char length2, double lengthPosition2)
    {
        GpsPosition[] gpsPositions = new GpsPosition[2];
        GpsPosition gps1 = new GpsPosition(width1, widthPosition1, length1, lengthPosition1);
        GpsPosition gps2 = new GpsPosition(width2, widthPosition2, length2, lengthPosition2);

        gpsPositions[0] = gps1;
        gpsPositions[1] = gps2;

        _parcNo = parcNo;
        _parcDesc = parcDesc;
        _gpsPosHandler = new GpsPosHandler( gpsPositions);
    }


    public Parcel(int parcNo, string parcDesc)
    {
        _parcNo = parcNo;
        _parcDesc = parcDesc;
        _gpsPosHandler = new GpsPosHandler();
    }

    #endregion

    #region Get/Set

    public int ParcNo { get => _parcNo; set => _parcNo = value; }
    public string ParcDesc { get => _parcDesc; set => _parcDesc = value; }
    internal GpsPosHandler GpsPosHandler { get => _gpsPosHandler; set => _gpsPosHandler = value; }

    #endregion

    #region Methods
    public void AddExistingGpsPos(int position, GpsPosition gpsPosition){
        if(!this._gpsPosHandler.AddExistingGpsPos(position, gpsPosition)){
            throw new ArithmeticException("Cannot insert this gps position to index " + position);
        }
    }

    public void AddNewGpsPosition(int position, char width, double widthPosition, char length, double lengthPosition){
        if(!this._gpsPosHandler.AddNewGpsPosition(position, width, widthPosition, length, lengthPosition)){
            throw new ArithmeticException("Cannot insert this gps position to index " + position);
        }
    }

    public void AddProperty( Property property) {
        this.AddListElement(property);
    }

    public Property GetProperty(int position) {
        return this.GetListItem(position);
    }

    public void RemoveProperty(int position) {
        this.RemoveListElement(position);
    }
    #endregion

}