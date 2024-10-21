import { writable } from 'svelte/store';
import { isRunningOnServer } from '$lib/env';
import type { ParsedMidiMessage } from '$lib/MidiStore';


type StatusMessage = {
	type: 'status'
	payload: string
}
type KeysMessage = {
	type: 'keys'
	payload: ParsedMidiMessage
}
type MessageSchema = StatusMessage | KeysMessage;


class WebsocketConnection {
	private websocket: WebSocket;
	public connected = writable(false);
	public lastMessage = writable<MessageSchema | null>(null);

	constructor() {
		this.websocket = this.connectToWebsocket();
	}

	onOpen() {
		console.debug('Connected to WebSocket server');
		this.connected.set(true);
	}

	onClose() {
		console.debug('Disconnected from WebSocket server');
		this.connected.set(false);
		this.websocket.removeEventListener('open', this.onOpen.bind(this));
		this.websocket.removeEventListener('close', this.onClose.bind(this));
		this.websocket.removeEventListener('message', this.onMessage.bind(this));
		this.websocket.close();
		setTimeout(() => this.websocket = this.connectToWebsocket(), 1000);
	}

	onMessage(event: MessageEvent) {
		console.log('Message received:', event.data);
		let message: MessageSchema = JSON.parse(event.data);
		this.lastMessage.set(message);
	}

	connectToWebsocket(): WebSocket {
		let websocket = new WebSocket('ws://localhost:6444');
		websocket.addEventListener('open', this.onOpen.bind(this));
		websocket.addEventListener('close', this.onClose.bind(this));
		websocket.addEventListener('message', this.onMessage.bind(this));
		return websocket;
	}

	sendMessage(message: MessageSchema) {
		if (this.websocket.readyState !== this.websocket.OPEN) {
			console.warn('WebSocket is not connected');
			return;
		}
		this.websocket.send(JSON.stringify(message));
	}
}


export const websocket = writable<WebsocketConnection>();

if (!isRunningOnServer) {
	websocket.set(new WebsocketConnection());
}