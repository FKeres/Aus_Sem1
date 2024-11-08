public class PropertyParcelMediator
{
    private readonly IServiceProvider _serviceProvider;

    public PropertyParcelMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    internal List<Property> SearchProperties(char width, double widthPosition, char length, double lengthPosition)
    {
        var propertyService = _serviceProvider.GetService<PropertyService>();
        return propertyService.SearchProperties(width, widthPosition, length, lengthPosition);
    }

    internal List<Parcel> SearchParcels(char width, double widthPosition, char length, double lengthPosition)
    {
        var parcelService = _serviceProvider.GetService<ParcelService>();
        return parcelService.SearchParcels(width, widthPosition, length, lengthPosition);
    }
}
