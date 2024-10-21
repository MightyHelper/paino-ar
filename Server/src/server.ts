import { WebSocketServer } from 'ws';

const wss = new WebSocketServer({ port: 6444 });

wss.on('connection', (ws) => {
	console.log('Client connected');

	ws.on('message', (message) => {
		console.log(`Received: ${message}`);
		ws.send(`Echo: ${message}`);
	});

	ws.on('close', () => {
		console.log('Client disconnected');
	});
	ws.send(JSON.stringify({
		type: 'status',
		payload: 'Welcome to the WebSocket server!'
	}));
	ws.send(JSON.stringify({
		type: 'status',
		payload: 'Welcome to THE WebSocket server!'
	}));
});

console.log('WebSocket server running on ws://localhost:6444');
