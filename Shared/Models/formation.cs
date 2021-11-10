using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mediwatch.Shared.Models
{
  [Table ("Formation")]
  public class formation
  {
    [Key]
    public Guid id {get; set;}
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; }
    public string Contact { get; set; }
    public string OrganizationName { get; set; }
    public decimal Price { get; set; }
    public string Former { get; set; }
    public string Description { get; set; }
    public string Target { get; set; }
    public string ArticleID {get; set;}
    public int QuantityCurrent{get; set;}
    public int QuantityMax{get; set;}
    // public bool IsRegistered { get; set; }
    // public int idCompagny {get; set;}
    // public string formationName {get; set;}
    // public DateTime createdAt{get; set;}
    // public DateTime formationTime {get; set;}
    // public string address {get; set;}
    // public string talker {get; set;}
    // public string description {get; set;}
    // public bool free {get; set;}
    // public List<tag> tagsFormation {get; set;}
    // public List<applicant_session> applicantsSessions {get; set;}
  }
}