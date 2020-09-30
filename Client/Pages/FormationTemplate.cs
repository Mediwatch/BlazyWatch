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

	[Required]
	public List<string> Target { get; set; }

	public bool IsRegistered { get; set; }

	public FormationTemplate() {}
	public FormationTemplate(FormationTemplateTab1 tab1, FormationTemplateTab2 tab2, FormationTemplateTab3 tab3) {
		Name = tab1.Name;
		StartDate = tab1.StartDate;
		EndDate = tab1.EndDate;
		Description = tab1.Description;

		OrganizationName = tab2.OrganizationName;
		Price = tab2.Price;
		Former = tab2.Former;

		Location = tab3.Location;
		Contact = tab3.Contact;
	}
}

public class FormationTemplateTab1
{
	[Required]
	public string Name { get; set; }

	[Required]
	public DateTime StartDate { get; set; } = DateTime.Today;

	[Required]
	public DateTime EndDate { get; set; } = DateTime.Today;

	[Required]
	public string Description { get; set; }
}

public class FormationTemplateTab2
{

	[Required]
	public string OrganizationName { get; set; }

	[Required]
	public decimal Price { get; set; }

	[Required]
	public string Former { get; set; }
}

public class FormationTemplateTab3
{
	
	[Required]
	public string Location { get; set; }

	[Required]
	[Phone]
	public string Contact { get; set; }
}