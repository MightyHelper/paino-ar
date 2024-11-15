import { writable } from 'svelte/store';

export type ParsedMidiMessage = { type: string, channel: number | null, key: number, state: number };
export const midi = writable<MIDIAccess | null>(null);
export const activeMidiInputs = writable<{ [id: string]: boolean }>({});
export const activeMidiOutputs = writable<{ [id: string]: boolean }>({});
export const midiInputs = writable<MIDIInput[]>([]);
export const midiOutputs = writable<MIDIOutput[]>([]);
export const lastMidiMessage = writable<MIDIMessageEvent | null>(null);
export const lastParsedMidiMessage = writable<ParsedMidiMessage | null>(null);
export const activeKeys = writable<{ [key: string]: number }>({});

const isNoteOff = (evt: number): boolean => (evt & 0b11110000) === 0b10000000;
const isNoteOn = (evt: number): boolean => (evt & 0b11110000) === 0b10010000;
const isPressure = (evt: number): boolean => (evt & 0b11110000) === 0b10100000;
const isControlChange = (evt: number): boolean => (evt & 0b11110000) === 0b10110000;
const isProgramChange = (evt: number): boolean => (evt & 0b11110000) === 0b11000000;
const isChannelPressure = (evt: number): boolean => (evt & 0b11110000) === 0b11010000;
const isPitchBend = (evt: number): boolean => (evt & 0b11110000) === 0b11100000;

const getType = (evt: number): 'noteOff' | 'noteOn' | 'pressure' | 'controlChange' | 'programChange' | 'channelPressure' | 'pitchBend' | 'unknown' => {
	if (isNoteOff(evt)) return 'noteOff';
	if (isNoteOn(evt)) return 'noteOn';
	if (isPressure(evt)) return 'pressure';
	if (isControlChange(evt)) return 'controlChange';
	if (isProgramChange(evt)) return 'programChange';
	if (isChannelPressure(evt)) return 'channelPressure';
	if (isPitchBend(evt)) return 'pitchBend';
	return 'unknown';
};

if (typeof window !== 'undefined') {
	async function main(){
		let midiAccess = await navigator.requestMIDIAccess();
		let listeners: { [key: string]: ((event: MIDIMessageEvent) => void) } = {};
		midi.set(midiAccess);
		midiInputs.set(Array.from(midiAccess.inputs.values()));
		midiOutputs.set(Array.from(midiAccess.outputs.values()));
		midiAccess.addEventListener('statechange', () => {
			midiInputs.set(Array.from(midiAccess.inputs.values()));
			midiOutputs.set(Array.from(midiAccess.outputs.values()));
		});
		console.log('MIDI access:', midiAccess);
		activeMidiInputs.subscribe(async (value) => {
			for (let input of midiAccess.inputs.values()) {
				if (value[input.id]) {
					if (input.state === 'disconnected') await input.open();
					let listener = (event: MIDIMessageEvent) => {
						if (event.data == null) return;
						if (event.data[0] == 254) return; // Ignore active sensing
						console.debug('MIDI message received:', event);
						lastMidiMessage.set(event);
					};
					input.addEventListener('midimessage', listener);
					listeners[input.id] = listener;
					console.log('Listening to MIDI input:', input.id);
				} else {
					if (input.state === 'connected') await input.close();
					input.removeEventListener('midimessage', listeners[input.id]);
					console.log('Stopped listening to MIDI input:', input.id);
				}
			}
		});
		lastMidiMessage.subscribe((value) => {
			if (value?.data == null) return;
			let type = getType(value.data[0]);
			let channel = value.data[0] & 0b00001111;
			switch (type) {
				case 'noteOn':
					if (value?.data == null) return;
					console.debug(`Note on [${channel}]:`, value.data[1], value.data[2]);
					lastParsedMidiMessage.set({ type, channel, key: value.data[1], state: value.data[2] });
					break;
				case 'noteOff':
					if (value?.data == null) return;
					console.debug(`Note off [${channel}]:`, value.data[1], value.data[2]);
					lastParsedMidiMessage.set({ type, channel, key: value.data[1], state: value.data[2] });
					break;
				case 'controlChange':
					console.debug('ControlChange:', value.data[1], value.data[2]);
					if (value.data[1] === 64) {
						lastParsedMidiMessage.set({
							'type': value.data[2] == 0 ? 'noteOff' : 'noteOn',
							'channel': channel,
							'key': -1,
							'state': value.data[2]
						})
					}
					break;
			}
		});
	}
	lastParsedMidiMessage.subscribe((value) => {
		switch (value?.type) {
			case 'noteOn':
			activeKeys.update((keys) => {
				keys[value.key] = value.state;
				return keys;
			});
			break;
			case 'noteOff':
			activeKeys.update((keys) => {
				keys[value.key] = 0;
				return keys;
			});
			break;
		}
	})
	main().then(() => console.log('MIDI initialized'));
}