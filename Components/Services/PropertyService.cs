public class PropertyService
{
    private KDTree<Property> _propertyTree;
    private int _actualPropId;

    public PropertyService()
    {
        _propertyTree = new KDTree<Property>();
        _actualPropId = 0;
    }

    public PropertyService(int actualPropId)
    {
        _propertyTree = new KDTree<Property>();
        _actualPropId = actualPropId;
    }

    internal KDTree<Property> GetAllProperties()
    {
        return _propertyTree;
    }

    internal void AddProperty(int inventNo, string propDesc, char gps1Width, double gps1WidthPosition, char gps1Length, double gps1LengthPosition, char gps2Width, double gps2WidthPosition, char gps2Length, double gps2LengthPosition)
    {
        Property property = new Property(_actualPropId, inventNo, propDesc, gps1Width, gps1WidthPosition, gps1Length, gps1LengthPosition, gps2Width, gps2WidthPosition, gps2Length, gps2LengthPosition);

        Key key1 = new Key(property.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(property.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];
        _propertyTree.AddElement(keys, property); 

        Key key3 = new Key(property.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(property.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];
        _propertyTree.AddElement(keys2, property);
        ++_actualPropId;
    }

    internal List<Property> SearchProperties(double gpsWToSearch, double gpsLToSearch) {
        
        List<Key> keys = [new Key(gpsWToSearch),  new Key(gpsLToSearch)];

        return _propertyTree.FindElement(keys);
    }

    internal void RemoveProperty(Property property) {
        Key key1 = new Key(property.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(property.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];

        _propertyTree.RemoveExactElement(keys, property);

        Key key3 = new Key(property.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(property.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];

        _propertyTree.RemoveExactElement(keys2, property);
    }

    internal void EditProperty(Property oldProp, Property newProp) {

        Key key1 = new Key(oldProp.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(oldProp.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];

        List<Node<Property>> node1 = _propertyTree.FindExactNode(keys, oldProp);

        if(node1 is not null) {
            if(newProp.InventNo != 0) {
                node1[0].Data.InventNo = newProp.InventNo;
            }
            if(newProp.PropDesc != "") {
                node1[0].Data.PropDesc = newProp.PropDesc;
            }
            node1[0].Data.GpsPosHandler.GetGpsPosition(0).Width = newProp.GpsPosHandler.GetGpsPosition(0).Width;
            node1[0].Data.GpsPosHandler.GetGpsPosition(1).Width = newProp.GpsPosHandler.GetGpsPosition(1).Width;
            node1[0].Data.GpsPosHandler.GetGpsPosition(0).Length = newProp.GpsPosHandler.GetGpsPosition(0).Length;
            node1[0].Data.GpsPosHandler.GetGpsPosition(1).Width = newProp.GpsPosHandler.GetGpsPosition(1).Width;

            if(node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition != newProp.GpsPosHandler.GetGpsPosition(0).WidthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition != newProp.GpsPosHandler.GetGpsPosition(1).WidthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition != newProp.GpsPosHandler.GetGpsPosition(0).LengthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition != newProp.GpsPosHandler.GetGpsPosition(0).LengthPosition) {

                Key key9 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition);
                Key key10 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition);

                List<Key> keys5 = [key9, key10];

                _propertyTree.RemoveExactElement(keys5, node1[0].Data);

                Key key3 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition);
                Key key4 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition);

                List<Key> keys2 = [key3, key4];

                _propertyTree.RemoveExactElement(keys2, node1[0].Data);

                node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition = newProp.GpsPosHandler.GetGpsPosition(0).WidthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition = newProp.GpsPosHandler.GetGpsPosition(0).LengthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition = newProp.GpsPosHandler.GetGpsPosition(1).WidthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition = newProp.GpsPosHandler.GetGpsPosition(1).LengthPosition;

                Key key5 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition);
                Key key6 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition);

                List<Key> keys3 = [key5, key6];

                _propertyTree.AddElement(keys3, node1[0].Data);

                Key key7 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition);
                Key key8 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition);

                List<Key> keys4 = [key7, key8];

                _propertyTree.AddElement(keys4, node1[0].Data);
            }
        }
    }
}
