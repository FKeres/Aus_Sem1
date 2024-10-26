public class HomeService
{
    private KDTree<PropParcHolder> _tree;
    private int _actualPropId;
    private int _actualParcId;

    public HomeService()
    {
        _tree = new KDTree<PropParcHolder>();
        _actualPropId = 0;
        _actualParcId = 0;
    }

    public HomeService(int actualParcId, int actualPropId)
    {
        _tree = new KDTree<PropParcHolder>();
        _actualParcId = actualParcId;
        _actualPropId = actualPropId;
    }

    internal KDTree<PropParcHolder> GetAllParcels()
    {
        return _tree;
    }

    internal void AddParcel(int parcNo, string parcDesc, char gps1Width, double gps1WidthPosition, char gps1Length, double gps1LengthPosition, char gps2Width, double gps2WidthPosition, char gps2Length, double gps2LengthPosition)
    {
        Parcel parcel = new Parcel(_actualParcId, parcNo, parcDesc, gps1Width, gps1WidthPosition, gps1Length, gps1LengthPosition, gps2Width, gps2WidthPosition, gps2Length, gps2LengthPosition);
        PropParcHolder holder = new PropParcHolder();
        holder.Parcel = parcel;

        Key key1 = new Key(parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];
        _tree.AddElement(keys, holder); 

        Key key3 = new Key(parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];
        _tree.AddElement(keys2, holder);
        ++_actualParcId;
    }

    internal void AddProperty(int inventNo, string propDesc, char gps1Width, double gps1WidthPosition, char gps1Length, double gps1LengthPosition, char gps2Width, double gps2WidthPosition, char gps2Length, double gps2LengthPosition)
    {
        Property property = new Property(_actualPropId, inventNo, propDesc, gps1Width, gps1WidthPosition, gps1Length, gps1LengthPosition, gps2Width, gps2WidthPosition, gps2Length, gps2LengthPosition);
        PropParcHolder holder = new PropParcHolder();
        holder.Property = property;

        Key key1 = new Key(property.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(property.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];
        _tree.AddElement(keys, holder); 

        Key key3 = new Key(property.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(property.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];
        _tree.AddElement(keys2, holder);
        
        ++_actualPropId;
    }

    internal List<PropParcHolder> SearchAll(double gpsWToSearch, double gpsLToSearch) {
        Key key1 = new Key(gpsWToSearch);
        Key key2 = new Key(gpsLToSearch);

        List<Key> keys = [key1, key2];

        List<PropParcHolder> copyHolderList = new List<PropParcHolder>();

        foreach(var actualHolder in _tree.FindElement(keys)) {
            Parcel copyParcel = new Parcel(actualHolder.Parcel.ParcelId, actualHolder.Parcel.ParcNo, actualHolder.Parcel.ParcDesc, actualHolder.Parcel.GpsPosHandler.GpsPositioons);
            Property copyProperty = new Property(actualHolder.Property.PropertyId, actualHolder.Property.InventNo, actualHolder.Property.PropDesc, actualHolder.Property.GpsPosHandler.GpsPositioons);

            PropParcHolder copyholder = new PropParcHolder();
            copyholder.Parcel = copyParcel;
            copyholder.Property = copyProperty;
            copyHolderList.Add(copyholder);
        }

        return copyHolderList;
    }

    internal void RemoveParc(Parcel parcel) {
        PropParcHolder holder = new PropParcHolder();
        holder.Parcel = parcel;

        Key key1 = new Key(parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];

        _tree.RemoveExactElement(keys, holder);

        Key key3 = new Key(parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];


        _tree.RemoveExactElement(keys2, holder);
    }

    internal void RemoveProp(Property property) {
        PropParcHolder holder = new PropParcHolder();
        holder.Property = property;

        Key key1 = new Key(property.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(property.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];

        _tree.RemoveExactElement(keys, holder);

        Key key3 = new Key(property.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(property.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];

        _tree.RemoveExactElement(keys2, holder);
    }

    internal void EditProperty(Property oldProp, Property newProp) {

        Key key1 = new Key(oldProp.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(oldProp.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];

        PropParcHolder holder = new PropParcHolder();
        holder.Property = oldProp;

        List<Node<PropParcHolder>> node1 = _tree.FindExactNode(keys, holder);

        if(node1 is not null && node1[0].Data.Property is not null) {
            if(newProp.InventNo != 0) {
                node1[0].Data.Property.InventNo = newProp.InventNo;
            }
            if(newProp.PropDesc != "") {
                node1[0].Data.Property.PropDesc = newProp.PropDesc;
            }
            node1[0].Data.Property.GpsPosHandler.GetGpsPosition(0).Width = newProp.GpsPosHandler.GetGpsPosition(0).Width;
            node1[0].Data.Property.GpsPosHandler.GetGpsPosition(1).Width = newProp.GpsPosHandler.GetGpsPosition(1).Width;
            node1[0].Data.Property.GpsPosHandler.GetGpsPosition(0).Length = newProp.GpsPosHandler.GetGpsPosition(0).Length;
            node1[0].Data.Property.GpsPosHandler.GetGpsPosition(1).Width = newProp.GpsPosHandler.GetGpsPosition(1).Width;

            if(node1[0].Data.Property.GpsPosHandler.GetGpsPosition(0).WidthPosition != newProp.GpsPosHandler.GetGpsPosition(0).WidthPosition ||
                node1[0].Data.Property.GpsPosHandler.GetGpsPosition(1).WidthPosition != newProp.GpsPosHandler.GetGpsPosition(1).WidthPosition ||
                node1[0].Data.Property.GpsPosHandler.GetGpsPosition(0).LengthPosition != newProp.GpsPosHandler.GetGpsPosition(0).LengthPosition ||
                node1[0].Data.Property.GpsPosHandler.GetGpsPosition(1).LengthPosition != newProp.GpsPosHandler.GetGpsPosition(0).LengthPosition) {

                Key key9 = new Key(node1[0].Data.Property.GpsPosHandler.GetGpsPosition(0).WidthPosition);
                Key key10 = new Key(node1[0].Data.Property.GpsPosHandler.GetGpsPosition(0).LengthPosition);

                List<Key> keys5 = [key9, key10];

                _tree.RemoveExactElement(keys5, node1[0].Data);

                Key key3 = new Key(node1[0].Data.Property.GpsPosHandler.GetGpsPosition(1).WidthPosition);
                Key key4 = new Key(node1[0].Data.Property.GpsPosHandler.GetGpsPosition(1).LengthPosition);

                List<Key> keys2 = [key3, key4];

                _tree.RemoveExactElement(keys2, node1[0].Data);

                node1[0].Data.Property.GpsPosHandler.GetGpsPosition(0).WidthPosition = newProp.GpsPosHandler.GetGpsPosition(0).WidthPosition;
                node1[0].Data.Property.GpsPosHandler.GetGpsPosition(0).LengthPosition = newProp.GpsPosHandler.GetGpsPosition(0).LengthPosition;
                node1[0].Data.Property.GpsPosHandler.GetGpsPosition(1).WidthPosition = newProp.GpsPosHandler.GetGpsPosition(1).WidthPosition;
                node1[0].Data.Property.GpsPosHandler.GetGpsPosition(1).LengthPosition = newProp.GpsPosHandler.GetGpsPosition(1).LengthPosition;

                Key key5 = new Key(node1[0].Data.Property.GpsPosHandler.GetGpsPosition(0).WidthPosition);
                Key key6 = new Key(node1[0].Data.Property.GpsPosHandler.GetGpsPosition(0).LengthPosition);

                List<Key> keys3 = [key5, key6];

                _tree.AddElement(keys3, node1[0].Data);

                Key key7 = new Key(node1[0].Data.Property.GpsPosHandler.GetGpsPosition(1).WidthPosition);
                Key key8 = new Key(node1[0].Data.Property.GpsPosHandler.GetGpsPosition(1).LengthPosition);

                List<Key> keys4 = [key7, key8];

                _tree.AddElement(keys4, node1[0].Data);
            }
        }
    }

    internal void EditParcel(Parcel oldParc, Parcel newParc) {
        Key key1 = new Key(oldParc.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(oldParc.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];

        PropParcHolder holder = new PropParcHolder();
        holder.Parcel = oldParc;

        List<Node<PropParcHolder>> node1 = _tree.FindExactNode(keys, holder);

        if(node1 is not null) {
            if(newParc.ParcNo != 0) {
                node1[0].Data.Parcel.ParcNo = newParc.ParcNo;
            }
            if(newParc.ParcDesc != "") {
                node1[0].Data.Parcel.ParcDesc = newParc.ParcDesc;
            }
            node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(0).Width = newParc.GpsPosHandler.GetGpsPosition(0).Width;
            node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(1).Width = newParc.GpsPosHandler.GetGpsPosition(1).Width;
            node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(0).Length = newParc.GpsPosHandler.GetGpsPosition(0).Length;
            node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(1).Width = newParc.GpsPosHandler.GetGpsPosition(1).Width;

            if(node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition != newParc.GpsPosHandler.GetGpsPosition(0).WidthPosition ||
                node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition != newParc.GpsPosHandler.GetGpsPosition(1).WidthPosition ||
                node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition != newParc.GpsPosHandler.GetGpsPosition(0).LengthPosition ||
                node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition != newParc.GpsPosHandler.GetGpsPosition(0).LengthPosition) {

                Key key9 = new Key(node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition);
                Key key10 = new Key(node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition);

                List<Key> keys5 = [key9, key10];

                _tree.RemoveExactElement(keys5, node1[0].Data);

                Key key3 = new Key(node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition);
                Key key4 = new Key(node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition);

                List<Key> keys2 = [key3, key4];

                _tree.RemoveExactElement(keys2, node1[0].Data);

                node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition = newParc.GpsPosHandler.GetGpsPosition(0).WidthPosition;
                node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition = newParc.GpsPosHandler.GetGpsPosition(0).LengthPosition;
                node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition = newParc.GpsPosHandler.GetGpsPosition(1).WidthPosition;
                node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition = newParc.GpsPosHandler.GetGpsPosition(1).LengthPosition;

                Key key5 = new Key(node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition);
                Key key6 = new Key(node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition);

                List<Key> keys3 = [key5, key6];

                _tree.AddElement(keys3, node1[0].Data);

                Key key7 = new Key(node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition);
                Key key8 = new Key(node1[0].Data.Parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition);

                List<Key> keys4 = [key7, key8];

                _tree.AddElement(keys4, node1[0].Data);
            }
        }

    }


}
