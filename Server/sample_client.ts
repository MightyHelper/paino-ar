// Connect using websocket to localhost:6443

import WebSocket from 'ws';
const readline = require('readline').createInterface({
	input: process.stdin,
	output: process.stdout
});

const ws = new WebSocket('ws://localhost:6443');

function question() {
	readline.question('', (msg: string) => {
		if (msg.length === 0){
			readline.close();
			ws.close()
			return
		}
		ws.send(msg);
		question();
	});
}

ws.on('open', function open() {
	console.log('connected');
	ws.send('something');

	// Get user input
	question();
});

ws.on('message', function incoming(data: WebSocket.RawData) {
	console.log(data.toString());
})
