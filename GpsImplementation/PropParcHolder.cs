class PropParcHolder
{
    private Property? _property;
    private Parcel? _parcel;

    internal Property? Property { get => _property; set => _property = value; }
    internal Parcel? Parcel { get => _parcel; set => _parcel = value; }
}