using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class FormationTemplate
{
	[Required]
	public string Name { get; set; }

	[Required]
	public DateTime StartDate { get; set; } = DateTime.Today;

	[Required]
	public DateTime EndDate { get; set; } = DateTime.Today;

	[Required]
	public string Location { get; set; }

	[Required]
	public string Contact { get; set; }

	[Required]
	public string OrganizationName { get; set; }

	[Required]
	public decimal Price { get; set; }

	[Required]
	public string Former { get; set; }

	[Required]
	public string Description { get; set; }

	public List<string> Target { get; set; }

	public bool IsRegistered { get; set; }

	public int Id { get; set; }
}