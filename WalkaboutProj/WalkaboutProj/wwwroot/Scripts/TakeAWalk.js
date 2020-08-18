function createNewMarker(marker, markerName, markerCategory, markerDescription, markerLat, markerLong) {
    var newMarker = createMarkerObject(markerName, markerCategory, markerDescription, markerLat, markerLong);
    $.ajax({
        type: 'POST',
        url: '/Wanderers/PostMarker',
        data: JSON.stringify(newMarker),
        contentType: 'application/json; charset=utf-8',
        success: function () {
            alert("Your marker has been saved!")
            marker.setDraggable(false); //set marker to fixed
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("We hit a problem with POSTING"); //throw any errors
        }
    }).then(function () {
        getRouteMarkers(newMarker.RouteId);
    });
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
function getRouteMarkers() {
    $(document).ready(function () {
        $.ajax({
            type: 'GET',
            url: '/Wanderers/GetAllMarkers',
            contentType: "application/json; charset=utf-8",

            dataType: 'json',
            success: function () {
                $('#markerTableBody').html(``);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("We hit a problem with GETTING");
            }
        }).then(function (data) {
            addMarkersToTable(data);
        })
    })
}

function addMarkersToTable(data) {
    for (let i = 0; i < data.length; i++) {
        $("#markerTableBody").append(`
                    <tr><td>${data[i].markerName}</td>
                    <td>${data[i].markerCategory}</td>
                    <td>${data[i].markerDescription}</td>
                    <td><button type="submit" class="btn btn-outline-danger"onclick="editSingleMarker(${data[i].markerId})">Edit</button>
                    <td><button type="submit" class="btn btn-outline-danger"onclick="removeMarker(${data[i].markerId})">Delete</button></tr>
                    </tr>`)
    }
}

function editSingleMarker(markerId) {
    $(document).ready(function () {
        $ajax({
            type: 'GET',
            url: '/Wanderers/GetMarker/' + markerId,
            dataType: 'json',
        }).then(function (data){
            $('#hiddenMarkerId').val(data['markerId'])
            $('#hiddenRouteId').val(data['routeId'])
            $('#hiddenIsFavorite').val(data['isFavorite']).text()
            $('#hiddenPointValue').val(data['pointValue'])
            $('#hiddenPicturePath').val(data['picturePath'])
            $('#hiddenMarkerLat').val(data['markerLat'])
            $('#hiddenMarkerLong').val(data['markerLong'])
            $('#editMarkerName').val(data['markerName']).text()
            $('#editMarkerCategory').val(data['markerCategory']).text()
            $('#editMarkerDescription').val(data['markerDescription']).text()
            $('#editImg').val(data['imgPath']).text()
        })
    })
}

function updateMarker() {
    var markerToUpdate = {
        "MarkerId": parseInt(document.getElementById('hiddenMarkerId').value),
        "RouteId": parseInt(document.getElementById('hiddenRouteId').value),
        "MarkerName": document.getElementById('editMarkerName').value,
        "MarkerCategory": document.getElementById('editMarkerCategory').value,
        "PicturePath": document.getElementById('hiddenPicturePath').value,
        "MarkerDescription": document.getElementById('editMarkerDesription').value,
        "IsFavorite": document.getElementById('hiddenIsFavorite').value,
        "PointValue": parseInt(document.getElementById('hiddenPointValue').value),
        "MarkerLat": parseInt(document.getElementById('hiddenMarkerLat').value),
        "MarkerLong": parseInt(document.getElementById('hiddenMarkerLong').value),
    };
    $(document).ready(function () {
        $ajax({
            url: '/Wanderers/PutMarker',
            type: 'PUT',
            data: JSON.stringify(markerToUpdate),
            contentType: 'application/json; charset=utf-8',
            success: function () {
                alert("Your marker has been updated!");
                $(document.getElementById('edit-marker').reset());
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("We hit a problem with PUTTING");
            }
        }).then(function () {
            getRouteMarkers();
        });
    });
}

function removeMarker(MarkerId) {

        //will need to remove from DB AND map
        $.ajax({
            type: "DELETE",
            url: '/Wanderers/DeleteMarker',
            data: { MarkerId: MarkerId },

            success: function () {
                alert("Your marker has been removed!");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("We hit a problem...");
            }
            });
    }