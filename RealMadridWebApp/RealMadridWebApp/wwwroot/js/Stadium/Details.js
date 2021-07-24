const INITIAL_ZOOM = 4;

const MEDIUM_TEMP = 20;
const HOT_TEMP = 30;

// Decide appropriate temperature image on mount.
$(function () {

    const temperature = parseFloat($('#tempIcon').attr('temperature'));
    let iconClass;

    if (temperature >= HOT_TEMP) {
        iconClass = "bi-sun-fill";
    } else if (temperature >= MEDIUM_TEMP) {
        iconClass = 'bi-cloud-sun-fill';
    } else {
        iconClass = 'bi-cloud-fill';
    }

    $('#tempIcon').addClass(iconClass);
});

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