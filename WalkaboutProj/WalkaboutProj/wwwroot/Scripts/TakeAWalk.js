function createNewMarker(marker, markerName, markerCategory, markerDescription, markerLat, markerLong, routeId) {
    var newMarker = createMarkerObject(markerName, markerCategory, markerDescription, markerLat, markerLong, routeId);
    if (markerCategory == "StartPoint" || markerCategory == "EndPoint") {
        checkForDuplicates(marker,newMarker);
    }
    else {
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
            getRouteMarkers();
        });
    }
   
}
//this one gets all markers then passes them into 
function checkForDuplicates(marker,newMarker) {
    var RouteId = document.getElementById('currentRouteId').value;
    $(document).ready(function () {
        $.ajax({
            type: 'GET',
            url: '/Wanderers/GetRouteMarkers',
            data: { RouteId: RouteId },
            success: function (data) {
                var markerExists = markerCategoryCheck(newMarker, data);
                if (!markerExists) {
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
                        getRouteMarkers();
                    });
                }
                else {
                    alert("Whoops!  Make sure you only have 1 start/end point!")
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("We hit a problem with CHECKING FOR DUPLICATES");
            }
        })
    })
}
function markerCategoryCheck(newMarker, data) {
    var currentMarkerCategory = newMarker.MarkerCategory;
    for (var i = 0; i < data.length; i++)
        if (data[i].markerCategory == currentMarkerCategory) {
            return true;
        }
    return false;
}

function updateMarker() {
    var updatedMarker = {
        "MarkerId": parseInt(document.getElementById('hiddenMarkerId').value),
        "RouteId": parseInt(document.getElementById('hiddenRouteId').value),
        "MarkerName": document.getElementById('editMarkerName').value,
        "MarkerCategory": document.getElementById('editMarkerCategory').value,
        "PicturePath": document.getElementById('hiddenPicturePath').value,
        "MarkerDescription": document.getElementById('editMarkerDescription').value,
        "IsFavorite": JSON.parse(document.getElementById('hiddenIsFavorite').value),
        "PointValue": parseFloat(document.getElementById('hiddenPointValue').value),
        "MarkerLat": parseFloat(document.getElementById('hiddenMarkerLat').value),
        "MarkerLong": parseFloat(document.getElementById('hiddenMarkerLong').value),
    };
    $(document).ready(function () {
        $.ajax({
            type: 'PUT',
            url: '/Wanderers/PutMarker',
            data: JSON.stringify(updatedMarker),
            contentType: "application/json; charset=utf-8",
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

function createMarkerObject(markerName, markerCategory, markerDescription, markerLat, markerLong, routeId) {
    var markerInfo = {
        "MarkerName": markerName,
        "RouteId" : routeId,
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
    var RouteId = document.getElementById('currentRouteId').value;
    $(document).ready(function () {
        $.ajax({
            type: 'GET',
            url: '/Wanderers/GetRouteMarkers',
            data: { RouteId: RouteId },
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

function editSingleMarker(MarkerId) {
    $(document).ready(function () {
        $.ajax({
            type: 'GET',
            data: { MarkerId: MarkerId },
            url: '/Wanderers/GetMarker',
            success: function () {
                alert("Got the marker!");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("We hit a problem with GETTING");
            }
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



function removeMarker(MarkerId) {
    var RouteId = RouteId;
        $.ajax({
            type: "DELETE",
            url: '/Wanderers/DeleteMarker',
            data: { MarkerId: MarkerId },

            success: function () {
                alert("Your marker has been removed!");
                getRouteMarkers();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("We hit a problem...");
            }
        });
}

function createNewRoute(WandererId) {
    var dateTime = Date.now();
    var newRoute = createRouteObject(WandererId, dateTime);
    $.ajax({
        type: 'POST',
        url: '/Wanderers/PostRoute',
        data: JSON.stringify(newRoute),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            var currentRouteId = document.getElementById('currentRouteId');
            currentRouteId.value = result.routeId;
            alert("Your route has been created!!")
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("We hit a problem with POSTING"); //throw any errors
        }
    }).then(function () {
    });
}

function createRouteObject(WandererId, dateTime) {
    var routeInfo = {
        "WandererId": WandererId,
        "RouteName": "New Route",
        "RouteDescription": "Enter a description for your route",
        "TotalDistance": 0,
        "TotalTimeMilliseconds": dateTime,
    }
    return routeInfo;
}

function initialRouteGet(dateTime) {
    $(document).ready(function () {
        $.ajax({
            type: 'GET',
            url: '/Wanderers/InitialRouteGet',
            data: { TotalTimeMilliseconds: dateTime },
            success: function () {
                alert("initial GET successful");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("We hit a problem with GETTING");
            }
        }).then(function (data) {
            $('#hiddenRouteId').val(data['RouteId'])
        })
    })
}

function endYourWalk() {
    prepareRouteWaypoints();

    //Will have ajax call to get all markers, run confirmation check, 
}
function prepareRouteWaypoints() {
    var RouteId = document.getElementById('currentRouteId').value;
    $(document).ready(function () {
        $.ajax({
            type: 'GET',
            url: '/Wanderers/PrepareRouteWaypoints',
            data: { RouteId: RouteId },
            success: function (data) {
                parseWaypoints(data);
                //Will be returning a list of all waypoints.  Index 0 is origin, last index is endpoint.
                
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("We hit a problem with GETTING ALL WAYPOINTS");
            }
        })
    })
}

function parseWaypoints(data) {
    var waypts = [];
    var resultLength = data.length - 1;
    var startPoint = {
        location: { lat: data[0].markerLat, lng: data[0].markerLong }
    }
    var endPoint = {
        location: { lat: data[resultLength].markerLat, lng: data[resultLength].markerLong }
    }
    for (let i = 1; i < resultLength; i++) {
        waypts.push({
            location: { lat: data[i].markerLat, lng: data[i].markerLong },
            stopover: true
        });
    }
    createDirections(startPoint, endPoint, waypts);
}
