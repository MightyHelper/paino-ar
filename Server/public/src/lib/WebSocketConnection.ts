import { writable } from 'svelte/store';
import { isRunningOnServer } from '$lib/env';

type MessageType = 'status'

type MessageSchema = {
	type: MessageType
	payload: string | [string: any]
}

export const connected = writable(false)
export const lastMessage = writable<MessageSchema | null>(null)

if (!isRunningOnServer) {
	function connectToWebsocket() {
		let websocket = new WebSocket('ws://localhost:6444');
		websocket.onopen = () => {
			console.debug('Connected to WebSocket server');
			connected.set(true);
		};
		websocket.onclose = () => {
			console.debug('Disconnected from WebSocket server');
			connected.set(false);
			websocket.close()
			setTimeout(connectToWebsocket, 1000);
		};
		websocket.onmessage = (event) => {
			console.log('Message received:', event.data);
			let message: MessageSchema = JSON.parse(event.data);
			lastMessage.set(message);
		};
	}
	connectToWebsocket()
}