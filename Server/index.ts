import express from 'express';
import {createServer} from 'http';
import WebSocket from 'ws';
import easymidi from 'easymidi';
const app = express();
const port = 6443;

const server = createServer(app);
const wss = new WebSocket.Server({server});
console.log(easymidi.getInputs())

wss.on('connection', (ws: WebSocket) => {
	console.log("client joined.");
	ws.on('message', (data: WebSocket.RawData) => {
		console.log(data.toString());
		ws.send("You sent: " + data.toString());
	});

	ws.on('close', () => {
		console.log("client left.");
	});
});

server.listen(port, () => {
	console.log(`Listening on http://localhost:${port}`);
});
