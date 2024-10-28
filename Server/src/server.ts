import { WebSocketServer } from 'ws';
import type { ParsedMidiMessage } from '../public/src/lib/MidiStore';

const wss = new WebSocketServer({ port: 6443 });


type StatusMessage = {
	type: 'status'
	payload: string
}
type KeysMessage = {
	type: 'keys'
	payload: ParsedMidiMessage
}
type MessageSchema = StatusMessage | KeysMessage;

let clients: any[] = [];

wss.on('connection', (ws) => {
	console.log('Client connected');
	const sendMessage = (msg: MessageSchema) => ws.send(JSON.stringify(msg));
	const updateStatus = (status: string) => sendMessage({ type: 'status', payload: status });
	ws.on('message', (message) => {
		let msg: MessageSchema = JSON.parse(message.toString());
		console.log('Message type:', msg.type, ' payload:', msg.payload);
		if (msg.type === 'keys') {
			console.log('Broadcasting keys to all clients');
			clients.forEach(client => client.send(JSON.stringify(msg)));
		}
	});

	ws.on('close', () => {
		console.log('Client disconnected');
		clients = clients.filter(client => client !== ws);
	});

	clients.push(ws);

	updateStatus('Welcome to the WebSocket server!');
});

console.log('WebSocket server running on ws://localhost:6443');
