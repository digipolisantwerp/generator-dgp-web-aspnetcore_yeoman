using System;

namespace StarterKit.DataAccess.Exceptions
{
    public class RepositoryNotFoundException : Exception
    {
		public RepositoryNotFoundException(string repositoryName, string message) : base(message)
		{
			if ( String.IsNullOrWhiteSpace(repositoryName) ) throw new ArgumentException("repositoryName mag niet null of leeg zijn.", "repositoryName");
			this.RepositoryName = repositoryName;
		}

		public string RepositoryName { get; private set; }
    }
}