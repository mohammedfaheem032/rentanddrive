// Initialize and add the map
function initMap(latval,lngval) {
    // The location of Uluru
    //const uluru = { lat: 40.7128, lng: -74.0060 };
    const uluru = { lat: latval, lng: lngval };
    // The map, centered at Uluru
    const map = new google.maps.Map(document.getElementById("map"), {
        zoom: 4,
        center: uluru,
    });
    // The marker, positioned at Uluru
    const marker = new google.maps.Marker({
        position: uluru,
        map: map,
    });
}

//window.initMap = initMap;