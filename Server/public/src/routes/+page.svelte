<script lang="ts">
	import {
		midiInputs,
		midiOutputs,
		activeMidiInputs,
		activeMidiOutputs,
		activeKeys,
		activePedal, lastParsedMidiMessage
	} from '$lib/MidiStore';
	import { websocket } from '$lib/WebSocketConnection';

	let message = '???...';
	$websocket?.lastMessage?.subscribe?.(value => {
		if (!value) return;
		switch (value.type) {
			case 'status':
				message = value.payload as string;
				break;
		}
	});
	let websocketConnected = $websocket?.connected;

	lastParsedMidiMessage.subscribe(value => {
		if (!value) return;
		$websocket?.sendMessage?.({ type: 'keys', payload: value });
	});

	function countWhiteKeysBelow(midiNote: number): number {
		const whiteKeysPerOctave = 7;
		const octaves = Math.floor(midiNote / 12);
		const remainingNotes = midiNote % 12;
		const whiteKeysPattern = [0, 2, 4, 5, 7, 9, 11];

		const whiteKeysInRemaining = whiteKeysPattern.filter(note => note < remainingNotes).length;
		return octaves * whiteKeysPerOctave + whiteKeysInRemaining;
	}

	let whiteKeyWidthCM = 30.5 / 13;
	let blackKeyWidthCM = 1.5;
	let whiteKeyHeightCM = 15;
	let blackKeyHeightCM = 10;
	let octaveWidthCM = 7 * whiteKeyWidthCM;
	let blackKeyOffsetsCM = [1.35, 4.15, 8.3, 11.1, 13.7];
	let whiteKeyIdxs = [0, 2, 4, 5, 7, 9, 11];
	let blackKeyIdxs = [1, 3, 6, 8, 10];
	let cm_scale = 0.25;
	let midiLowestNote = 21;
	let midiHighestNote = 108;
	let whiteKeysBeforeLowestNote = countWhiteKeysBelow(midiLowestNote);
	let keyboardOffsetCM = -whiteKeysBeforeLowestNote * whiteKeyWidthCM;
	let extra = (x: number) => x == 0 ? 0 : x + 0x3f;
	let asHex = (x: number) => x.toString(16).padStart(2, '0');
</script>
<h1>Welcome to PianoAR [{$websocketConnected ? 'ok' : 'er'}]: '{message}'</h1>

Inputs:
<ul>
  {#each $midiInputs as input}
    <li><input type="checkbox" bind:checked={$activeMidiInputs[input.id]}><strong>{input.id}</strong>: {input.name}</li>
  {/each}
</ul>

Outputs:
<ul>
  {#each $midiOutputs as output}
    <li><input type="checkbox" bind:checked={$activeMidiOutputs[output.id]}><strong>{output.id}</strong>: {output.name}
    </li>
  {/each}
</ul>

Piano:
<div style="position: relative">
  {#each Array.from({ length: 10 }) as _, octave}
    {#each Array.from({ length: 7 }).map((_, i) => whiteKeyIdxs[i] + octave * 12) as note, i}
      {#if note >= midiLowestNote && note <= midiHighestNote}
        <div class="keyWhite" style="
				  width: calc({whiteKeyWidthCM * cm_scale}cm - 2px);
				  height: calc({whiteKeyHeightCM * cm_scale}cm - 2px);
				  left: calc({(i * whiteKeyWidthCM + octave * octaveWidthCM + keyboardOffsetCM) * cm_scale}cm + 1px);
				  top: 1px;
				  background: #{asHex(0xff - extra($activeKeys[note] || 0)).repeat(2) + asHex(0xff-2*$activePedal)};
				"
        ></div>
      {/if}
    {/each}
    {#each blackKeyOffsetsCM.map((x, i) => [x, blackKeyIdxs[i] + octave * 12]) as [offsetCM, note], i}
      {#if note >= midiLowestNote && note <= midiHighestNote}
        <div class="keyBlack" style="
            width: {blackKeyWidthCM * cm_scale}cm;
            height: calc({blackKeyHeightCM * cm_scale}cm - 2px);
            left: {(offsetCM + octave * octaveWidthCM + keyboardOffsetCM) * cm_scale}cm;
            top: 1px;
            background: #{asHex(extra($activeKeys[note] || 0)).repeat(2) + asHex(2*$activePedal)};
          "
        ></div>
      {/if}
    {/each}
  {/each}
</div>
<style>
    div.keyBlack {
        background: black;
        position: absolute;
        border: 1px solid white;
        border-top: none;
        box-sizing: content-box;
    }

    div.keyWhite {
        background: white;
        position: absolute;
        border: 1px solid black;
        box-sizing: content-box;
    }
</style>