using IdylAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities.Syst
{
	[Table("LogInHistory")]
	public class LogInHistory: BaseEntity
	{
		public string Username { get; set; }
		public DateTime LoginDate { get; set; }
		public string IPAddress { get; set; }
		public string Status { get; set; }
	}
}
