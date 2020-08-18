// Initialize and add the map

    function initMap() {
        // The location of Uluru
        var myPlace = {lat: 43.064880, lng: -87.880110};
        // The map, centered at Uluru
        var map = new google.maps.Map(
            document.getElementById('marker-map'), {zoom: 16, center: myPlace
          });

          google.maps.event.addListener(map, 'rightclick', function(event) {		
            var marker = new google.maps.Marker({
                position: event.latLng, //map Coordinates where user right clicked
                map: map,
                draggable:true, //set marker draggable 
                animation: google.maps.Animation.DROP, //bounce animation
                title:"Hello World!",
            });
            
            //Content structure of info Window for the Markersc
            var contentString = $('<div class="marker-info-win">'+
            '<div class="marker-inner-win"><span class="info-content">'+
            '<h2 class="marker-heading">Add a Marker</h2>'+
            '<p><div class="marker-edit">'+
            '<form action="ajax-save.php" method="POST" name="SaveMarker" id="SaveMarker">'+
            '<label for="markerName"><span>Name :</span><input type="text" name="markerName" class="save-name" placeholder="Name your Marker" maxlength="40" /></label>'+
            '<label for="markerDescription"><span>Description :</span><textarea name="markerDescription" class="save-desc" placeholder="Enter a descrption" maxlength="150"></textarea></label>'+
            '<label for="markerCategory"><span>Category:</span> <select name="markerCategory" class="save-type"><option value="Wildlife">Wildlife</option><option value="Landmark">Landmark</option>'+
            '<option value="Highlight">Highlight</option></select></label>'+
            '</form>'+
            '</div></p><button name="save-marker" class="save-marker">Save Marker</button>'+ 
            '</span><button name="remove-marker" class="remove-marker" title="Remove Marker">Remove Marker</button></div></div>');
                
            //Create an infoWindow
            var infowindow = new google.maps.InfoWindow();
            
            //set the content of infoWindow
            infowindow.setContent(contentString[0]);
            
            //add click listner to marker which will open infoWindow 		 
            google.maps.event.addListener(marker, 'click', function() {
                    infowindow.open(map,marker); // click on marker opens info window 
            });
            
    
            //###### remove marker #########/
            var removeBtn = contentString.find('button.remove-marker')[0];
            google.maps.event.addDomListener(removeBtn, "click", function(event) {
                marker.setMap(null);
            });
        });
    }