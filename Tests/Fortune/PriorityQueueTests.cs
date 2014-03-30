using NUnit.Framework;
using System;
using System.Collections.Generic;
using VoronoiGenerators.Fortune;

namespace Tests.Fortune
{
	[TestFixture]
	public class PriorityQueueTests
	{
		private const int Seed = 81019491;

		[Test]
		public void OrdersItemsCorrectly()
		{
			const int Cases = 20, InputsPerCase = 10;
			var rand = new Random(Seed);

			var inputs = new byte[InputsPerCase];
			for(int @case = 0; @case < Cases; @case++)
			{
				var pq = MakeMaxPQ();
				rand.NextBytes(inputs);
				foreach (var input in inputs)
					pq.Enqueue(input);
				// Invert the comparison to get descending order.
				Array.Sort(inputs, (a, b) => b - a);
				foreach (var expectedOutput in inputs)
					Assert.That(expectedOutput == pq.Dequeue(), Is.True);
			}
		}

		[Test]
		public void GivesSameNumberOfOutputsAsInputs()
		{
			const int Cases = 20, MaxInputsPerCase = 20;
			var rand = new Random(Seed);

			for(int @case = 0; @case < Cases; @case++)
			{
				var pq = MakeMaxPQ();
				int inputCount = rand.Next(1, MaxInputsPerCase);
				for (int i = 0; i < inputCount; i++)
					pq.Enqueue(rand.Next());
				Assert.That(pq.Count == inputCount, Is.True);
				for (int i = 0; i < inputCount; i++)
					pq.Dequeue();
				Assert.That(pq.Count == 0, Is.True);
			}
		}

		[Test]
		public void CountStartsAtZero()
		{
			var pq = MakeMaxPQ();
			Assert.That(pq.Count == 0, Is.True);
		}

		[Test]
		public void CountIncrementsAndDecrementsCorrectly()
		{
			const int Cases = 20, MaxInputsPerCase = 30;
			var rand = new Random(Seed);

			for(int @case = 0; @case < Cases; @case++)
			{
				var pq = MakeMaxPQ();
				int inputCount = rand.Next(1, MaxInputsPerCase);
				for(int i = 0; i < inputCount; i++)
				{
					pq.Enqueue(rand.Next());
					Assert.That(pq.Count == i + 1, Is.True);
				}
				for(int i = 0; i < inputCount; i++)
				{
					pq.Dequeue();
					Assert.That(pq.Count == inputCount - i - 1, Is.True);
				}
			}
		}

		[Test]
		public void TopMatchesDequeuedElement()
		{
			const int Cases = 20, MaxInputsPerCase = 30;
			var rand = new Random(Seed);

			for(int @case = 0; @case < Cases; @case++)
			{
				var pq = MakeMaxPQ();
				int inputCount = rand.Next(1, MaxInputsPerCase);
				for (int i = 0; i < inputCount; i++)
					pq.Enqueue(rand.Next());
				for (int i = 0; i < inputCount; i++)
					Assert.That(pq.Top == pq.Dequeue(), Is.True);
			}
		}

		[Test]
		public void CorrectlyRemovesItems()
		{
			const int MaxCount = 20;
			var rand = new Random(Seed);
			var items = new List<PriorityQueueItem<int>>(MaxCount);

			for(int count = 3; count < MaxCount; count++)
			{
				var pq = MakeMaxPQ();
				items.Clear();

				// Prepare
				for (int i = 0; i < count; i++)
					items.Add(pq.Enqueue(rand.Next()));
				// Invert the comparison to get descending order.
				items.Sort((a, b) => b.Item - a.Item);
				// Remove all the middle elements.
				for (int i = 1; i < count - 1; i++)
					pq.Remove(items[i]);

				// Test
				Assert.That(pq.Dequeue() == items[0].Item, Is.True);
				Assert.That(pq.Dequeue() == items[count - 1].Item, Is.True);
			}
		}

		// Creates a priorityqueue where entries closer to positive
		// infinity have priority over entries closer to negative infinity.
		private PriorityQueue<int> MakeMaxPQ()
		{
			return new PriorityQueue<int>((a, b) => a - b);
		}
	}
}