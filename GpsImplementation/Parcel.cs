class Parcel :TerritorialUnit<Property>
{
    #region Attributes

    private int _parcelId;
    private int _parcNo;
    private string? _parcDesc;
    private GpsPosHandler _gpsPosHandler;

    #endregion

    #region Constructor

    public Parcel() {
        _gpsPosHandler = new GpsPosHandler();
    }

    public Parcel(int parcelId, int parcNo, string parcDesc, GpsPosition[] gpsPositions)
    {
        _parcelId = parcelId;
        _parcNo = parcNo;
        _parcDesc = parcDesc;
        _gpsPosHandler = new GpsPosHandler(gpsPositions);
    }

    public Parcel(int parcelId, int parcNo, string parcDesc, char width1, double widthPosition1, char length1, double lengthPosition1, char width2, double widthPosition2, char length2, double lengthPosition2)
    {
        GpsPosition[] gpsPositions = new GpsPosition[2];
        GpsPosition gps1 = new GpsPosition(width1, widthPosition1, length1, lengthPosition1);
        GpsPosition gps2 = new GpsPosition(width2, widthPosition2, length2, lengthPosition2);

        gpsPositions[0] = gps1;
        gpsPositions[1] = gps2;

        _parcelId = parcelId;
        _parcNo = parcNo;
        _parcDesc = parcDesc;
        _gpsPosHandler = new GpsPosHandler( gpsPositions);
    }


    public Parcel(int parcelId, int parcNo, string parcDesc)
    {
        _parcelId = parcelId;
        _parcNo = parcNo;
        _parcDesc = parcDesc;
        _gpsPosHandler = new GpsPosHandler();
    }

    #endregion

    #region Get/Set

    public int ParcNo { get => _parcNo; set => _parcNo = value; }
    public string? ParcDesc { get => _parcDesc; set => _parcDesc = value; }
    internal GpsPosHandler GpsPosHandler { get => _gpsPosHandler; set => _gpsPosHandler = value; }
    public int ParcelId { get => _parcelId; set => _parcelId = value; }

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

    public void RemovePropertyEq(Property property) {
        this.RemoveListElementEq(property);
    }

    public List<Property> GetProperties() {
        return Items;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Parcel other)
        {
            return _parcelId == other.ParcelId;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_parcelId);
    }

    public string Serialize() {
        return $"{_parcelId},{_parcNo},{_parcDesc},{_gpsPosHandler.GpsPositioons[0].Width},{_gpsPosHandler.GpsPositioons[0].WidthPosition},{_gpsPosHandler.GpsPositioons[0].Length},{_gpsPosHandler.GpsPositioons[0].LengthPosition},{_gpsPosHandler.GpsPositioons[1].Width},{_gpsPosHandler.GpsPositioons[1].WidthPosition},{_gpsPosHandler.GpsPositioons[1].Length},{_gpsPosHandler.GpsPositioons[1].LengthPosition}";
    }

    public void DeSerialize(string attributes) {
        var fieldValues = attributes.Split(',');

        _parcelId = int.Parse(fieldValues[0]);
        _parcNo = int.Parse(fieldValues[1]);
        _parcDesc = fieldValues[2];

        char w1 = char.Parse(fieldValues[3]);
        double wP1 = double.Parse(fieldValues[4]);
        char l1 = char.Parse(fieldValues[5]);
        double lP1 = double.Parse(fieldValues[6]);

        char w2 = char.Parse(fieldValues[7]);
        double wP2 = double.Parse(fieldValues[8]);
        char l2 = char.Parse(fieldValues[9]);
        double lP2 = double.Parse(fieldValues[10]);

        GpsPosition gps1 = new GpsPosition(w1, wP1, l1, lP1);
        _gpsPosHandler.GpsPositioons[0] = gps1;
        GpsPosition gps2 = new GpsPosition(w2, wP2, l2, lP2);
        _gpsPosHandler.GpsPositioons[1] = gps2;
    }

    #endregion

}