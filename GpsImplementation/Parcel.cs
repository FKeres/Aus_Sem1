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
        return $"PARCELID:{_parcelId};PARCNO:{_parcNo};PARCDESC:{_parcDesc};GPSW1:{_gpsPosHandler.GpsPositioons[0].Width};GPSWP1:{_gpsPosHandler.GpsPositioons[0].WidthPosition};GPSL1:{_gpsPosHandler.GpsPositioons[0].Length};GPSLP1:{_gpsPosHandler.GpsPositioons[0].LengthPosition};GPSW2:{_gpsPosHandler.GpsPositioons[1].Width};GPSWP2:{_gpsPosHandler.GpsPositioons[1].WidthPosition};GPSL2:{_gpsPosHandler.GpsPositioons[1].Length};GPSLP2:{_gpsPosHandler.GpsPositioons[1].LengthPosition};";
    }

    public void DeSerialize(string attributes) {
        var fieldSeparator = attributes.Split(';');

        char w1 = ' ';
        double wP1 = 0;
        char l1 = ' ';
        double lP1 = 0;

        char w2 = ' ';
        double wP2 = 0;
        char l2 = ' ';
        double lP2 = 0;

        foreach (var attr in fieldSeparator) {
            if (string.IsNullOrWhiteSpace(attr)) continue;

            var keyValue = attr.Split(':');
            var key = keyValue[0];
            var value = keyValue[1];


            switch (key) {
                case "PARCELID":
                    _parcelId = int.Parse(value);
                    break;
                case "PARCNO":
                    _parcNo = int.Parse(value);
                    break;
                case "PARCDESC":
                    _parcDesc = value;
                    break;
                case "GPSW1":
                    w1 = char.Parse(value);
                    break;
                case "GPSWP1":
                    wP1 = double.Parse(value);
                    break;
                case "GPSL1":
                    l1 = char.Parse(value);
                    break;
                case "GPSLP1":
                    lP1 = double.Parse(value);
                    break;
                case "GPSW2":
                    w2 = char.Parse(value);
                    break;
                case "GPSWP2":
                   wP2 = double.Parse(value);
                    break;
                case "GPSL2":
                    l2 = char.Parse(value);
                    break;
                case "GPSLP2":
                    lP2 = double.Parse(value);
                    break;
            }
        }

        GpsPosition gps1 = new GpsPosition(w1, wP1, l1, lP1);
        _gpsPosHandler.GpsPositioons[0] = gps1;
        GpsPosition gps2 = new GpsPosition(w2, wP2, l2, lP2);
        _gpsPosHandler.GpsPositioons[1] = gps2;
    }

    #endregion

}