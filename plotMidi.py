import pretty_midi
import matplotlib.pyplot as plt
import numpy as np
import argparse

def main(args):
	midi_data = pretty_midi.PrettyMIDI(args.midiPath)
	for instrument in midi_data.instruments:
		#print(instrument.notes)
		print(instrument)
		plotPitch(instrument.name,instrument.notes)
		plotVelocity(instrument.name,instrument.notes)
def plotPitch(instrumentName, instrumentNotes):
	#print(notes)
	times = []
	notes = []
	for note in instrumentNotes:
		#print(note)
		#time start time
		#note, pitch
		times.append(note.start)
		notes.append(note.pitch)
	notesAtTime = list(zip(times,notes))

	sortedStarts = sorted(notesAtTime, key = lambda x:x[0])
	#print(sortedStarts)
	#print(times)
	#print(notes)
	#y = notes vs x = times
	plt.figure(figsize=(200,11))
	plt.xlabel('time')
	plt.ylabel('pitch')
	plt.title(instrumentName)
	sortedStarts = list(zip(*sortedStarts))
	#print(len(sortedStarts))
	plt.plot(sortedStarts[0],sortedStarts[1], markeredgewidth=20)
	plt.tight_layout()
	#plt.show()
	plt.savefig(instrumentName + '_pitch')
	plt.clf()

def plotVelocity(instrumentName, instrumentNotes):
	times = []
	notes = []
	for note in instrumentNotes:
		#print(note)
		#time start time
		#note, pitch
		times.append(note.start)
		notes.append(note.velocity)
	notesAtTime = list(zip(times,notes))

	sortedStarts = sorted(notesAtTime, key = lambda x:x[0])
	#print(sortedStarts)
	#print(times)
	#print(notes)
	#y = notes vs x = times
	plt.figure(figsize=(200,11))
	plt.xlabel('time')
	plt.ylabel('velocity')
	plt.title(instrumentName)
	sortedStarts = list(zip(*sortedStarts))
	#print(len(sortedStarts))
	plt.plot(sortedStarts[0],sortedStarts[1], markeredgewidth=20)
	plt.tight_layout()
	#plt.show()
	plt.savefig(instrumentName + '_velocity')
	plt.clf()

if __name__ == '__main__':
	parser = argparse.ArgumentParser(description='Choose a midi file to plot')
	parser.add_argument('midiPath', type=str, help='path to midi file')
	args = parser.parse_args()
	main(args)

