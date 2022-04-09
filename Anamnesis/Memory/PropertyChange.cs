﻿// © Anamnesis.
// Licensed under the MIT license.

namespace Anamnesis.Memory
{
	using System.Collections.Generic;

	public struct PropertyChange
	{
		public readonly List<BindInfo> BindPath;
		public readonly object? OldValue;
		public readonly object? NewValue;
		public readonly Origins Origin;

		private string path;

		public PropertyChange(BindInfo bind, object? oldValue, object? newValue, Origins origin)
		{
			this.BindPath = new();
			this.BindPath.Add(bind);

			this.OldValue = oldValue;
			this.NewValue = newValue;

			this.path = bind.Path;

			this.Origin = origin;
		}

		public enum Origins
		{
			Anamnesis,
			Game,
		}

		public readonly BindInfo OriginBind => this.BindPath[0];

		public string TerminalPropertyName => this.BindPath[0].Name;

		public bool ShouldRecord()
		{
			// Don't record changes that originate in game
			if (this.Origin == Origins.Game)
				return false;

			foreach (BindInfo bind in this.BindPath)
			{
				if (bind.Flags.HasFlag(BindFlags.DontRecordHistory))
					return false;
			}

			return true;
		}

		public void AddPath(BindInfo bind)
		{
			this.BindPath.Add(bind);
			this.path = bind.Path + this.path;
		}

		public override string ToString()
		{
			return $"{this.path}: {this.OldValue} -> {this.NewValue}";
		}
	}
}
