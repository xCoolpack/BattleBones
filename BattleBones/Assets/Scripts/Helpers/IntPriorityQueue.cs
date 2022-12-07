using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntPriorityQueue<T>
{
    private readonly List<(T element, int priority)> _queue;
    public int Count { get { return _queue.Count; } }

    public IntPriorityQueue()
    {
        _queue = new List<(T element, int priority)>();
    }

    public void Enqueue(T element, int priority)
    {
        _queue.Add((element, priority));
        int ci = _queue.Count - 1;
        while (ci > 0)
        {
            int pi = (ci - 1) / 2;
            if (_queue[ci].priority >= _queue[pi].priority)
                break;
            var tmp = _queue[ci]; 
            _queue[ci] = _queue[pi];
            _queue[pi] = tmp;
            ci = pi;
        }
    }

    public (T, int) Dequeue()
    {
        int li = _queue.Count - 1;
        var frontItem = _queue[0];
        _queue[0] = _queue[li];
        _queue.RemoveAt(li);

        --li;
        int pi = 0;
        while (true)
        {
            int ci = pi * 2 + 1;
            if (ci > li) break;
            int rc = ci + 1;
            if (rc <= li && _queue[rc].priority < _queue[ci].priority)
                ci = rc;
            if (_queue[pi].priority <= _queue[ci].priority)
                break;
            var tmp = _queue[pi]; 
            _queue[pi] = _queue[ci]; 
            _queue[ci] = tmp;
            pi = ci;
        }
        return frontItem;
    }

    public (T, int) Peek()
    {
        return _queue[0];
    }
}
