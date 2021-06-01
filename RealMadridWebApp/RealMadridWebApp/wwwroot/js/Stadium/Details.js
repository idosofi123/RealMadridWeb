const INITIAL_ZOOM = 4;

// Initialize and add the map.
function initMap() {

    const lat = parseFloat($('#map').attr('lat'));
    const lng = parseFloat($('#map').attr('lng'));

    const map = new Microsoft.Maps.Map('#map', {
        center: new Microsoft.Maps.Location(lat, lng)
    });

    const center = map.getCenter();

    //Create custom Pushpin
    const pin = new Microsoft.Maps.Pushpin(center);

    //Add the pushpin to the map
    map.entities.push(pin);

}