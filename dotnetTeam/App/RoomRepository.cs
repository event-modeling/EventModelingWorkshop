using System;
using System.Linq;
using System.Threading.Tasks;
using App.EventStore;
using Domain;

namespace App
{
	public class RoomRepository : IRoomRepository
	{
		private readonly IEventStore _eventStore;

		public RoomRepository(IEventStore eventStore)
		{
			_eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
		}
		public async Task<RoomId[]> GetNotCheckedRoomIds()
		{
			return (await _eventStore.GetAggregateHistory())
				.OfType<IRoomDomainEvent>()
				.GroupBy(o => o.RoomId)
				.Where(o => o.Last() is GuestCheckedOut)
				.Select(o => o.Key)
				.ToArray();
		}

		public async Task<RoomId[]> GetCheckedInRoomIds()
		{
			return (await _eventStore.GetAggregateHistory())
				.OfType<IRoomDomainEvent>()
				.GroupBy(o => o.RoomId)
				.Where(o => o.Last() is RoomCheckedIn)
				.Select(o => o.Key)
				.ToArray();
		}

		public async Task<RoomId[]> GetDirtyRoomIds()
		{
			return (await _eventStore.GetAggregateHistory())
				.OfType<IRoomDomainEvent>()
				.GroupBy(o => o.RoomId)
				.Where(o => o.Last() is RoomCleaningRequested)
				.Select(o => o.Key)
				.ToArray();
		}

		public async Task<RoomId[]> GetCheckInRoomIds()
		{
			return (await _eventStore.GetAggregateHistory())
				.OfType<IRoomDomainEvent>()
				.GroupBy(o => o.RoomId)
				.Where(o => o.Last() is RoomCheckedIn)
				.Select(o => o.Key)
				.ToArray();
		}
	}
}