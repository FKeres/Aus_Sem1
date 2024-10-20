public class PropertyService
{
    private KDTree<Property> _propertyTree;

    public PropertyService()
    {
        _propertyTree = new KDTree<Property>();

        Property property = new Property(100, "new property", 'N', 100, 'S', 50, 'W', 150, 'N', 300);

        Key key1 = new Key(property.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(property.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];
        _propertyTree.AddElement(keys, property); 

        Key key3 = new Key(property.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(property.GpsPosHandler.GetGpsPosition(1).LengthPosition);
        List<Key> keys2 = [key3, key4];
        _propertyTree.AddElement(keys2, property); 
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
        Key key1 = new Key(gpsWToSearch);
        Key key2 = new Key(gpsLToSearch);

        List<Key> keys = [key1, key2];

        return _propertyTree.FindElement(keys);
    }
}
