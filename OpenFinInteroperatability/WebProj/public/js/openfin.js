(function () {
    'use strict';
	var subscribeToInterAppBus = function () {

	    fin.desktop.InterApplicationBus.subscribe('*', 'winform-click', function (msg) {
			console.log('Message Received: ' + msg.symbolName);
			txtArea.textContent = msg;
        });
    };

    //event listeners.
    document.addEventListener('DOMContentLoaded', function () {
        //OpenFin is ready
        fin.desktop.main(function () {
			subscribeToInterAppBus();
        });
    });
}());