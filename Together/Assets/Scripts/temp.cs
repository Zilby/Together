using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Solution
{
	public static ulong total;

	private static List<ulong> LeftSkyline(List<ulong> a)
	{
		if (a.Count <= 1)
		{
			return a;
		}
		List<ulong> l = new List<ulong>();
		List<ulong> r = new List<ulong>();

		int middle = a.Count / 2;
		for (int i = 0; i < middle; i++)
		{
			l.Add(a[i]);
		}
		for (int i = middle; i < a.Count; i++)
		{
			r.Add(a[i]);
		}

		l = LeftSkyline(l);
		r = LeftSkyline(r);
		return LeftMerge(l, r);
	}

	private static List<ulong> LeftMerge(List<ulong> left, List<ulong> right)
	{
		List<ulong> result = new List<ulong>();
		int l = 0;
		int r = 0;
		while (l < left.Count || r < right.Count)
		{
			if (l < left.Count && r < right.Count)
			{
				if (left[l] <= right[r])
				{
					result.Add(left[l]);
					l++;
				}
				else
				{
					total += (ulong)(result.Count - r);
					result.Add(right[r]);
					r++;
				}
			}
			else if (l < left.Count)
			{
				result.Add(left[l]);
				l++;
			}
			else if (r < right.Count)
			{
				total += (ulong)(result.Count - r);
				result.Add(right[r]);
				r++;
			}
		}
		return result;
	}


	private static List<ulong> RightSkyline(List<ulong> a)
	{
		if (a.Count <= 1)
		{
			return a;
		}
		List<ulong> l = new List<ulong>();
		List<ulong> r = new List<ulong>();

		int middle = a.Count / 2;
		for (int i = 0; i < middle; i++)
		{
			l.Add(a[i]);
		}
		for (int i = middle; i < a.Count; i++)
		{
			r.Add(a[i]);
		}

		l = RightSkyline(l);
		r = RightSkyline(r);
		return RightMerge(l, r);
	}

	private static List<ulong> RightMerge(List<ulong> left, List<ulong> right)
	{
		List<ulong> result = new List<ulong>();
		int l = 0;
		int r = 0;
		while (l < left.Count || r < right.Count)
		{
			if (l < left.Count && r < right.Count)
			{
				if (left[l] <= right[r])
				{
					total += (ulong)(result.Count - l);
					result.Add(left[l]);
					l++;
				}
				else
				{
					result.Add(right[r]);
					r++;
				}
			}
			else if (l < left.Count)
			{
				total += (ulong)(result.Count - l);
				result.Add(left[l]);
				l++;
			}
			else if (r < right.Count)
			{
				result.Add(right[r]);
				r++;
			}
		}
		return result;
	}


	static void Main(String[] args)
	{
		System.Console.ReadLine().Trim();
		string intString = System.Console.ReadLine().Trim();
		List<string> intArray = intString.Split(new char[] { ' ' }).ToList();
		List<ulong> m = intArray.Select(ulong.Parse).ToList();
		total = 0;
		LeftSkyline(m);
		ulong result1 = total;
		total = 0;
		RightSkyline(m);
		ulong result2 = total;
		Console.WriteLine(String.Format("{0} {1}", result1, result2));
	}
}