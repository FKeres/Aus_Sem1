class PropParcHolder
{
    private Property? _property;
    private Parcel? _parcel;

    internal Property? Property { get => _property; set => _property = value; }
    internal Parcel? Parcel { get => _parcel; set => _parcel = value; }

    public override bool Equals(object? obj)
    {
        if (obj is PropParcHolder other)
        {
            if(_property != null && _parcel != null) {
                return _property.Equals(other.Property) && _parcel.Equals(other.Parcel);
            } else if(_property != null) {
                return _property.Equals(other.Property);
            } else {
               return _parcel.Equals(other.Parcel);
            }
            
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_property, _parcel);
    }
}