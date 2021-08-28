<Query Kind="Program" />

public class Wrap: Try {
	public Wrap(Action<string> action): base(action) {
	}
	public override void Handle(string msg) {
		"start".Dump();
		_action(msg);
		"end".Dump();
	}
}

public class Try: Pipe {
	public Try(Action<string> action): base(action) {
	
	}

	public override void Handle(string msg) {
		try {
			"trying".Dump();
			_action(msg);
		} catch (Exception ex) {
		}
	}
}

public abstract class Pipe {
	// action should be accessed by inherited classes
	protected Action<string> _action;
	public Pipe(Action<string> action) {
		this._action = action;
	}
	
	public abstract void Handle(string msg);
}

void Main()
{
	Action<string> next = (msg) => { msg.Dump(); };
	var tryPipe = new Try(next);
	tryPipe.Handle("use try pipe");
	
	var wrapPipe = new Wrap(next);
	wrapPipe.Handle("use wrap pipe");
	
	
}

