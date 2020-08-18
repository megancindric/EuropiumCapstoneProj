function createNewMarker(marker, markerName, markerCategory, markerDescription, markerLat, markerLong) {
    var newMarker = createMarkerObject(markerName, markerCategory, markerDescription, markerLat, markerLong);
    $.ajax({
        type: "POST",
        url: '/Wanderers/PostMarker',
        data: JSON.stringify(newMarker),
        contentType: 'application/json; charset=utf-8',
        success: function () {
            alert("Your marker has been saved!")
            marker.setDraggable(false); //set marker to fixed
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("Nope"); //throw any errors
        }
    }).then(function (data) { addMarkersToTable(data); })
}
function createMarkerObject(markerName, markerCategory, markerDescription, markerLat, markerLong) {
    var markerInfo = {
        "RouteId": 1,
        "MarkerName": markerName,
        "MarkerCategory": markerCategory,
        "PicturePath": "",
        "MarkerDescription": markerDescription,
        "IsFavorite": false,
        "PointValue": 0,
        "MarkerLat": markerLat,
        "MarkerLong": markerLong,
    }
    return markerInfo;
}


function addMarkersToTable(data) {
    for (let i = 0; i < data.length; i++) {
        $("#markerTableBody").append(`
                    <tr><td>${data[i].MarkerName}</td>
                    <td>${data[i].MarkerCategory}</td>
                    <td>${data[i].MarkerDescription}</td>
                    <td><button type="submit" class="btn btn-outline-danger"onclick="EditMarker(${data[i].MarkerId})">Edit</button>
                    <td><button type="submit" class="btn btn-outline-danger"onclick="DeleteMarker(${data[i].MarkerId})">Delete</button></tr>
                    </tr>`)
    }
}