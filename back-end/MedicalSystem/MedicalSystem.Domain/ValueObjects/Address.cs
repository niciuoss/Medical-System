using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Domain.ValueObjects
{
  public class Address
  {
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Complement { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public string GetFullAddress()
    {
      var parts = new List<string>();

      if (!string.IsNullOrEmpty(Street))
        parts.Add($"{Street}, {Number}");

      if (!string.IsNullOrEmpty(Complement))
        parts.Add(Complement);

      if (!string.IsNullOrEmpty(Neighborhood))
        parts.Add(Neighborhood);

      if (!string.IsNullOrEmpty(City))
        parts.Add(City);

      if (!string.IsNullOrEmpty(State))
        parts.Add(State);

      if (!string.IsNullOrEmpty(ZipCode))
        parts.Add(ZipCode);

      return string.Join(", ", parts);
    }
  }
}