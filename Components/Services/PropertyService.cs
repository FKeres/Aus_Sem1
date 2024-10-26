public class PropertyService
{
    private KDTree<Property> _propertyTree;

    public PropertyService()
    {
        _propertyTree = new KDTree<Property>();
    }

    internal KDTree<Property> GetAllProperties()
    {
        return _propertyTree;
    }

    internal void AddProperty(Property property)
    {
        Key key1 = new Key(property.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(property.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];
        _propertyTree.AddElement(keys, property); 

        Key key3 = new Key(property.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(property.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];
        _propertyTree.AddElement(keys2, property);
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

        _propertyTree.RemoveExactElement(keys, oldProp);

        Key key3 = new Key(oldProp.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(oldProp.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];

        _propertyTree.RemoveExactElement(keys2, oldProp);

        oldProp.GpsPosHandler.GetGpsPosition(0).WidthPosition = newProp.GpsPosHandler.GetGpsPosition(0).WidthPosition;
        oldProp.GpsPosHandler.GetGpsPosition(0).LengthPosition = newProp.GpsPosHandler.GetGpsPosition(0).LengthPosition;
        oldProp.GpsPosHandler.GetGpsPosition(1).WidthPosition = newProp.GpsPosHandler.GetGpsPosition(1).WidthPosition;
        oldProp.GpsPosHandler.GetGpsPosition(1).LengthPosition = newProp.GpsPosHandler.GetGpsPosition(1).LengthPosition;

        Key key5 = new Key(oldProp.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key6 = new Key(oldProp.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys3 = [key5, key6];

        _propertyTree.AddElement(keys3, oldProp);

        Key key7 = new Key(oldProp.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key8 = new Key(oldProp.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys4 = [key7, key8];

        _propertyTree.AddElement(keys4, oldProp);
    }
}
