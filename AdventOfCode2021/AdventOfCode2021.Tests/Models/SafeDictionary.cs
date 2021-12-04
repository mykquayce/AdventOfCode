using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2021.Tests.Models;

public class SafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
	where TKey : notnull
{
	private readonly TValue _defaultValue;
	private readonly IDictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

	public SafeDictionary(TValue defaultValue) => _defaultValue = defaultValue;

	public TValue this[TKey key]
	{
		get => TryGetValue(key, out var value) ? value : _defaultValue;
		set => _dictionary[key] = value;
	}

	public ICollection<TKey> Keys => _dictionary.Keys;
	public ICollection<TValue> Values => _dictionary.Values;
	public int Count => _dictionary.Count;
	public bool IsReadOnly => _dictionary.IsReadOnly;
	public void Add(TKey key, TValue value) => _dictionary.Add(key, value);
	public void Add(KeyValuePair<TKey, TValue> item) => _dictionary.Add(item);
	public void Clear() => _dictionary.Clear();
	public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);
	public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);
	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _dictionary.CopyTo(array, arrayIndex);
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();
	public bool Remove(TKey key) => _dictionary.Remove(key);
	public bool Remove(KeyValuePair<TKey, TValue> item) => _dictionary.Remove(item);
	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _dictionary.TryGetValue(key, out value);
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
