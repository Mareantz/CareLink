using Domain.Entities;

namespace Domain.Common
{
	public class PagedResult<T>
	{
        private List<MedicalHistory> data;
        private Task<int> totalItems;

        public List<T> Data { get; }
		public int TotalCount { get; }
		public PagedResult(List<T> data, int totalCount)
		{
			Data = data;
			TotalCount = totalCount;
		}

        public PagedResult(List<MedicalHistory> data, Task<int> totalItems)
        {
            this.data = data;
            this.totalItems = totalItems;
        }
    }
}
