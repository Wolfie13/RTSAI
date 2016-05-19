using UnityEngine;

[System.Serializable]
public class IVec2
{
	public IVec2(){x = 0; y = 0;}
	public IVec2(int newx, int newy) { x = newx; y = newy; }
	public IVec2(IVec2 newVec) { x = newVec.x; y = newVec.y; }
	
	public static IVec2 operator +(IVec2 a, IVec2 b) { return new IVec2(a.x + b.x, a.y + b.y); }
	public static IVec2 operator -(IVec2 a, IVec2 b) { return new IVec2(a.x - b.x, a.y - b.y); }
	public static IVec2 operator *(IVec2 a, int b) { return new IVec2(a.x * b, a.y * b); }
	public static IVec2 operator /(IVec2 a, int b) { return new IVec2(a.x / b, a.y / b); }
	public static bool operator ==(IVec2 a, IVec2 b) { return (a.x==b.x && a.y == b.y);}
	public static bool operator !=(IVec2 a, IVec2 b) { return !(a==b); }
	
	public float magnitude() { return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2)); }
	
	public override string ToString() { return "(" + x + ", " + y + ")"; }
	
	public int x, y;
	
	public override bool Equals(System.Object o)
	{
		return o.GetType().Equals(this.GetType ()) && (o as IVec2) == this;
	}
	
	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}
	
	public int manhatttanDistance(IVec2 other)
	{
		return Mathf.Abs(other.x - this.x) + Mathf.Abs(other.y - this.y);
	}
}