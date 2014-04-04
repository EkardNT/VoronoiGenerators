using System;

namespace VoronoiGenerators.Fortune
{
	internal class PriorityQueueItem<T>
	{
		public T Item { get; private set; }
		public int Index { get; set; }

		public PriorityQueueItem(T item, int index)
		{
			Item = item;
			Index = index;
		}
	}

	internal class PriorityQueue<T>
	{
		private Comparison<T> orderer;
		private PriorityQueueItem<T>[] heap;
		private int firstEmptySlot;

		public PriorityQueue(Comparison<T> orderer)
		{
			this.orderer = orderer;
			heap = new PriorityQueueItem<T>[16];
			firstEmptySlot = 0;
		}

		public int Count
		{
			get
			{
				return firstEmptySlot;
			}
		}

		public PriorityQueueItem<T> Enqueue(T item)
		{
			if(firstEmptySlot >= heap.Length)
			{
				var newHeap = new PriorityQueueItem<T>[Math.Max(16, heap.Length * 2)];
				for (int i = 0; i < heap.Length; i++)
					newHeap[i] = heap[i];
				heap = newHeap;
			}
			var pqItem = heap[firstEmptySlot] = new PriorityQueueItem<T>(item, firstEmptySlot);
			firstEmptySlot++;
			WalkUp(firstEmptySlot - 1);
			return pqItem;
		}

		public T Top
		{
			get
			{
				if (firstEmptySlot == 0)
					throw new InvalidOperationException("Cannot retrieve the top element from an empty PriorityQueue.");
				return heap[0].Item;
			}
		}

		public T Dequeue()
		{
			if (firstEmptySlot == 0)
				throw new InvalidOperationException("Cannot dequeue the top element from an empty PriorityQueue.");
			var top = heap[0];
			heap[0] = heap[firstEmptySlot - 1];
			heap[0].Index = 0;
			heap[firstEmptySlot - 1] = null;
			firstEmptySlot--;
			WalkDown(0);
			return top.Item;
		}

		public void Remove(int slot)
		{
			if (firstEmptySlot == 0)
				throw new InvalidOperationException("Cannot remove an item from an empty PriorityQueue.");
			heap[slot] = heap[firstEmptySlot - 1];
			heap[slot].Index = slot;
			heap[firstEmptySlot - 1] = null;
			firstEmptySlot--;
			WalkDown(slot);
			WalkUp(slot);
		}

		private void WalkUp(int startSlot)
		{
			if (startSlot >= firstEmptySlot)
				return;
			int currentSlot = startSlot,
				parentSlot = ParentSlot(startSlot);
			while (currentSlot > 0 && !ProperlyOrdered(parentSlot, currentSlot))
			{ 
				SwapElements(ref parentSlot, ref currentSlot);
				parentSlot = ParentSlot(currentSlot);
			}
		}

		private void WalkDown(int startSlot)
		{
			int currentSlot = startSlot,
				leftChildSlot = LeftChildSlot(startSlot),
				rightChildSlot = RightChildSlot(startSlot),
				bestLowerSwapSlot;
			while(leftChildSlot < firstEmptySlot && !ProperlyOrdered(currentSlot, bestLowerSwapSlot = FindBestLowerSwapSlot(leftChildSlot, rightChildSlot)))
			{
				SwapElements(ref currentSlot, ref bestLowerSwapSlot);
				leftChildSlot = LeftChildSlot(currentSlot);
				rightChildSlot = RightChildSlot(currentSlot);
			}
		}

		private int FindBestLowerSwapSlot(int leftChildSlot, int rightChildSlot)
		{
			if (rightChildSlot >= firstEmptySlot)
				return leftChildSlot;
			return ProperlyOrdered(leftChildSlot, rightChildSlot) 
				? leftChildSlot 
				: rightChildSlot;
		}

		private void SwapElements(ref int slotA, ref int slotB)
		{
			var tempT = heap[slotA];
			heap[slotA] = heap[slotB];
			heap[slotB] = tempT;
			int temp = slotA;
			slotA = slotB;
			slotB = temp;
			heap[slotA].Index = slotA;
			heap[slotB].Index = slotB;
		}

		private bool ProperlyOrdered(int higherSlot, int lowerSlot)
		{
			return orderer(heap[higherSlot].Item, heap[lowerSlot].Item) >= 0;
		}

		private static int ParentSlot(int currentSlot)
		{
			return (currentSlot - 1) / 2;
		}

		private static int LeftChildSlot(int currentSlot)
		{
			return currentSlot * 2 + 1;
		}

		private static int RightChildSlot(int currentSlot)
		{
			return currentSlot * 2 + 2;
		}
	}
}