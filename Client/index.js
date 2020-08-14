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
          
          google.maps.event.addListener(marker, 'click', function() {
            infowindow.open(map,marker); // click on marker opens info window 
    });
          //Content structure of info Window for the Markers
     //Content structure of info Window for the Markers
		var contentString = $('<div class="marker-info-win">'+
		'<div class="marker-inner-win"><span class="info-content">'+
		'<h1 class="marker-heading">New Marker</h1>'+
		'This is a new marker infoWindow'+ 
		'</span>'+
		'<br /><button name="remove-marker" class="remove-marker" title="Remove Marker">Remove Marker</button></div></div>');
			
		//Create an infoWindow
		var infowindow = new google.maps.InfoWindow();
		
		//set the content of infoWindow
        infowindow.setContent(contentString[0]);
        
        var removeBtn 	= contentString.find('button.remove-marker')[0];
		google.maps.event.addDomListener(removeBtn, "click", function(event) {
			marker.setMap(null);
		});
 });

}
