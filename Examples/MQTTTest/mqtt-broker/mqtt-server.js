var mosca = require('mosca');
var settings = {
		port : 18833
		}

var server = new mosca.Server(settings);

// fired client is connected
server.on('clientConnected', function(client) {
    console.log('Client connected', client.id);
});

// fired when a message is received
server.on('published', function(packet, client) {
    console.log('Message Received ', packet.payload);
  });

  server.on('ready', setup);

  // fired when the mqtt server is ready
  function setup() {
    console.log('Mosca MQTT server is up and running at ' + settings.port);
  }