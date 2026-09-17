using System;
using System.Collections.Generic;
using System.Text;

namespace ScoreCrafter.Domain.Entities;

public class User : BaseEntity
{
    public Grade? UserGrade { get; set; }
    public Guid? UserGradeId { get; set; }
    public decimal UserScore { get; set; }
}
public class UserGrade : BaseEntity
{
    public Guid UserId { get; set; }
    public int GradeId { get; set; }
    public bool IsCurrent { get; set; }
    public Grade Grade { get; set; }
    public User User { get; set; }
}
public class UserScore : BaseEntity
{
    public Guid UserId { get; set; }
    public decimal Score { get; set; }
    public Guid FormulaId { get; set; }
    public int FormulaVersion { get; set; }
    public DateTime CalculatedAt { get; set; }
    public bool IsCurrent { get; set; }
    public User User { get; set; }
    public Formula Formula { get; set; }
}
public class Purchase : BaseEntity
{
    public User User { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string ExternalId { get; set; }
    public Dictionary<string, string> Metadata { get; set; }

}
public class Grade : BaseEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
}
public class Formula : BaseEntity
{
    public int Version { get; set; }
    public Grade Grade { get; set; }
    public int GradeId { get; set; }
    public string Definition { get; set; }
    public bool IsActive { get; set; }
}


public abstract class BaseEntity : BaseEntity<Guid>
{
    public Guid Id { get; set; }
}

public abstract class BaseEntity<T>
{
    public T Id { get; set; }
    public DateTime CreatedTime { get; set; }
}