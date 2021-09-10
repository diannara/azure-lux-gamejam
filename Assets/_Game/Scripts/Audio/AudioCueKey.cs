using System;

using Diannara.ScriptableObjects.Audio;

namespace Diannara.Audio
{
	public struct AudioCueKey
	{
		public static AudioCueKey Invalid = new AudioCueKey(Guid.Empty, null);

		internal Guid Guid;
		internal AudioCue AudioCue;

		internal AudioCueKey(Guid guid, AudioCue audioCue)
		{
			Guid = guid;
			AudioCue = audioCue;
		}

		public override bool Equals(object obj)
		{
			return obj is AudioCueKey x && Guid == x.Guid && AudioCue == x.AudioCue;
		}
		public override int GetHashCode()
		{
			return Guid.GetHashCode() ^ AudioCue.GetHashCode();
		}
		public static bool operator ==(AudioCueKey x, AudioCueKey y)
		{
			return x.Guid == y.Guid && x.AudioCue == y.AudioCue;
		}
		public static bool operator !=(AudioCueKey x, AudioCueKey y)
		{
			return !(x == y);
		}
	}
}