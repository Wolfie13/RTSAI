using UnityEngine;

[System.Serializable]
public class IVec2
{
	public IVec2 ()
	{
		x = 0;
		y = 0;
	}

	public IVec2 (int newx, int newy)
	{
		x = newx;
		y = newy;
	}

	public IVec2 (IVec2 newVec)
	{
		x = newVec.x;
		y = newVec.y;
	}
	
	public static IVec2 operator + (IVec2 a, IVec2 b)
	{
		return new IVec2 (a.x + b.x, a.y + b.y);
	}

	public static IVec2 operator - (IVec2 a, IVec2 b)
	{
		return new IVec2 (a.x - b.x, a.y - b.y);
	}

	public static IVec2 operator * (IVec2 a, int b)
	{
		return new IVec2 (a.x * b, a.y * b);
	}

	public static IVec2 operator / (IVec2 a, int b)
	{
		return new IVec2 (a.x / b, a.y / b);
	}

	public static bool operator == (IVec2 a, IVec2 b)
	{
		// If both are null, or both are same instance, return true.
		if (System.Object.ReferenceEquals(a, b))
		{
			return true;
		}
		
		// If one is null, but not both, return false.
		if (((object)a == null) || ((object)b == null))
		{
			return false;
		}

		return a.x == b.x && a.y == b.y;
	}

	public static bool operator != (IVec2 a, IVec2 b)
	{
		return !(a == b);
	}
	
	public float magnitude ()
	{
		return Mathf.Sqrt (Mathf.Pow (x, 2) + Mathf.Pow (y, 2));
	}
	
	public override string ToString ()
	{
		return "(" + x + ", " + y + ")";
	}
	
	public int x, y;
	
	public override bool Equals (System.Object o)
	{
		if (o == null) {
			return false;
		}
		if (!o.GetType ().Equals (this.GetType ()))
			return false;
		IVec2 b = o as IVec2;
		return (this.x == b.x && this.y == b.y);
	}
	
	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}
	
	public int manhatttanDistance (IVec2 other)
	{
		return Mathf.Abs (other.x - this.x) + Mathf.Abs (other.y - this.y);
	}
}