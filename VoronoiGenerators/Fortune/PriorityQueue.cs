using System;
using System.Collections.Generic;

namespace VoronoiGenerators.Fortune
{
	internal class PriorityQueue<T>
	{
		private Comparison<T> orderer;
		private T[] heap;
		private int firstEmptySlot;

		public PriorityQueue(Comparison<T> orderer)
		{
			this.orderer = orderer;
			heap = new T[16];
			firstEmptySlot = 0;
		}

		public int Count
		{
			get
			{
				return firstEmptySlot;
			}
		}

		public void Enqueue(T item)
		{
			if(firstEmptySlot >= heap.Length)
			{
				var newHeap = new T[Math.Max(16, heap.Length * 2)];
				for (int i = 0; i < heap.Length; i++)
					newHeap[i] = heap[i];
				heap = newHeap;
			}
			heap[firstEmptySlot] = item;
			firstEmptySlot++;
			WalkUp(firstEmptySlot - 1);
		}

		public T Top
		{
			get
			{
				if (firstEmptySlot == 0)
					throw new InvalidOperationException("Cannot retrieve the top element from an empty PriorityQueue.");
				return heap[0];
			}
		}

		public T Dequeue()
		{
			if (firstEmptySlot == 0)
				throw new InvalidOperationException("Cannot dequeue the top element from an empty PriorityQueue.");
			T top = heap[0];
			heap[0] = heap[firstEmptySlot - 1];
			heap[firstEmptySlot - 1] = default(T);
			firstEmptySlot--;
			WalkDown(0);
			return top;
		}

		private void WalkUp(int startSlot)
		{
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
			T tempT = heap[slotA];
			heap[slotA] = heap[slotB];
			heap[slotB] = tempT;
			int temp = slotA;
			slotA = slotB;
			slotB = temp;
		}

		private bool ProperlyOrdered(int higherSlot, int lowerSlot)
		{
			return orderer(heap[higherSlot], heap[lowerSlot]) >= 0;
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