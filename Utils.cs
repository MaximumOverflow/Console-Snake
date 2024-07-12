using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Snakes;

internal static class Utils
{
	public delegate bool AnyDelegate<TValue, TContext>(in TContext ctx, in TValue v);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool Any<TValue, TContext>(this TValue[] collection, in TContext ctx, AnyDelegate<TValue, TContext> cond)
		=> Any((ReadOnlySpan<TValue>)collection, in ctx, cond);
	
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool Any<TValue, TContext>(this List<TValue> collection, in TContext ctx, AnyDelegate<TValue, TContext> cond)
		=> Any(CollectionsMarshal.AsSpan(collection), in ctx, cond);
	
	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static bool Any<TValue, TContext>(this ReadOnlySpan<TValue> collection, in TContext ctx, AnyDelegate<TValue, TContext> cond)
	{
		for (var index = 0; index < collection.Length; index++)
		{
			var value = collection[index];
			if (cond(in ctx, in value))
				return true;
		}

		return false;
	}
}