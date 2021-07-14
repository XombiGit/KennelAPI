using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Models.Data
{
	public static class DogStatus
	{
		public enum Status
		{
			FRIENDLY,
			FRIGHTEN,
			NERVOUS,
			BARK,
			TEMPERMENTAL
		}

		static Dictionary<Status, string> statusDictionary = new Dictionary<Status, string>() {
			{Status.FRIENDLY, "Friendly with other dogs and people"},
			{Status.FRIGHTEN,  "Frightened by other dogs, best not to approach"},
			{Status.NERVOUS, "Nervous with some dogs, use caution when approaching" },
			{Status.BARK, "Barks at other dogs, best not to approach"},
			{Status.TEMPERMENTAL, "Has bitten other dogs before, do not approach"}
		};
	}
}