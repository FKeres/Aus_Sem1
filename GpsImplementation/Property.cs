class Property :TerritorialUnit<Parcel>
{
    #region Attributes

    private int _inventNo;
    private string? _propDesc;
    private GpsPosHandler _gpsPosHandler;

    #endregion

    #region Constructor

    public Property(int inventNo, string? propDesc, GpsPosition[] gpsPositions)
    {
        _inventNo = inventNo;
        _propDesc = propDesc;
        _gpsPosHandler = new GpsPosHandler(gpsPositions);
    }

    public Property(int inventNo, string propDesc, char width1, double widthPosition1, char length1, double lengthPosition1, char width2, double widthPosition2, char length2, double lengthPosition2)
    {
        GpsPosition[] gpsPositions = new GpsPosition[2];
        GpsPosition gps1 = new GpsPosition(width1, widthPosition1, length1, lengthPosition1);
        GpsPosition gps2 = new GpsPosition(width2, widthPosition2, length2, lengthPosition2);

        gpsPositions[0] = gps1;
        gpsPositions[1] = gps2;

        _inventNo = inventNo;
        _propDesc = propDesc;
        _gpsPosHandler = new GpsPosHandler(gpsPositions);
    }

    public Property(int inventNo, string? propDesc)
    {
        _inventNo = inventNo;
        _propDesc = propDesc;
        _gpsPosHandler = new GpsPosHandler();
    }

    #endregion

    
    #region Get/Set
    public int InventNo { get => _inventNo; set => _inventNo = value; }
    public string? PropDesc { get => _propDesc; set => _propDesc = value; }
    internal GpsPosHandler GpsPosHandler { get => _gpsPosHandler; set => _gpsPosHandler = value; }
    #endregion

    #region Methods
    public void AddGpsPosition(int position, GpsPosition gpsPosition){
        if(!this._gpsPosHandler.AddExistingGpsPos(position, gpsPosition)) {
            throw new ArithmeticException("Cannot insert this gps position to index " + position);
        }
    }

    public void AddNewGpsPosition(int position, char width, double widthPosition, char length, double lengthPosition){
        if(!this._gpsPosHandler.AddNewGpsPosition(position, width, widthPosition, length, lengthPosition)) {
            throw new ArithmeticException("Cannot insert this gps position to index " + position);
        }
    }

    public void AddParcel(Parcel parcel) {
        this.AddListElement(parcel);
    }

    public Parcel GetParcel(int position) {
        return this.GetListItem(position);
    }

    public void RemoveParcel(int position) {
        this.RemoveListElement(position);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Property other)
        {
            return _inventNo == other.InventNo && _propDesc == other.PropDesc;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_inventNo, _propDesc);
    }

    #endregion

}